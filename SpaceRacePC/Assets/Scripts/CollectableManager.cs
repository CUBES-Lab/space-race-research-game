using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   The CollectableManager compenent spawns collectable GameObjects every lap.
/// </summary>
public class CollectableManager : MonoBehaviour
{

    public GameObject coinPrefab;

    public GameObject cokeCapPrefab;
    public GameObject drThundeCapPrefab;

    // This has all the individual tracks as children
    public Transform trackParent;

    public Transform finishLine;
    public int coinClustersPerLap = 25;
    //public int capClustersPerLap = 5;

    // Enforce that collectables cannot be placed too close to the finish line
    public float minDistanceFromFinishLine = 40;
    public int clusterSize = 5;
    public float clusterOffset = 1f;
    public float curveOffset = 2.5f;
    public LayerMask coinLayer;

    private List<Transform> tracks;
    private GameObject collectParent;
    private List<GameObject> collectables;
    private bool doRemoveCollectables;
    private bool doPlaceCollectables;

    // Start is called before the first frame update
    void Start()
    {        
        collectables = new List<GameObject>();
        if (finishLine == null)
        {
            finishLine = GameObject.Find("StartFinishLine").transform;
        }

        if (trackParent == null)
        {
            trackParent = GameObject.Find("ModularTrack").transform;

        }
        // Get all tracks
        tracks = new List<Transform>();
        GetAllTracks(trackParent);
        FilterTracks();

        doRemoveCollectables = false;
        doPlaceCollectables = false;

        collectParent = new GameObject("Collectables");
        PlaceCollectables();
    }

    public void LateUpdate()
    {
        // BB - Reset collectables needs to delay placement until cleanup has happened
        if (doRemoveCollectables)
        {
            RemoveCollectables();
            doRemoveCollectables = false;
        }
        else if (doPlaceCollectables)
        {
            PlaceCollectables();
            doPlaceCollectables = false;
        }
    }

    public void ResetCollectables()
    {
        doRemoveCollectables = true;
        doPlaceCollectables = true;
    }

    public void PlaceCollectables()
    {
        Shuffle(tracks);

        GameObject gameManager = GameObject.Find("GameManager");
        GameModeManager gameModeManager = gameManager.GetComponent<GameModeManager>();

        Vector3 coinPos;
        // Place coins up until coinClustersPerLap or all the track space has been used up
        //for (int i = capClustersPerLap; i < Mathf.Min(capClustersPerLap + coinClustersPerLap, tracks.Count); i++)
        int numClustersPlaced = 0;
        for (int i = 0; i < tracks.Count; i++)
        {
            if (numClustersPlaced >= coinClustersPerLap)
            {
                break;
            }

            if (tracks[i].name.Contains("NOCOINS"))
            {
                continue;
            }
            else if (tracks[i].name.Contains("Curve"))
            {
                if (tracks[i].name.Contains("Left"))
                {
                    if (tracks[i].name.Contains("Ramp"))
                    {
                        coinPos = tracks[i].transform.TransformPoint(Vector3.forward * 15f + Vector3.right * -10f + Vector3.up * 20f);
                    }
                    else
                    {
                        coinPos = tracks[i].transform.TransformPoint(Vector3.forward * 20f + Vector3.right * -20f + Vector3.up * 20f);
                    }
                }
                else
                {
                    coinPos = tracks[i].transform.TransformPoint(Vector3.up * 20f);
                }
                
            }
            else 
            {
                coinPos = tracks[i].GetComponent<Renderer>().bounds.center + Vector3.up * 20f;
            }

            float laneDirectionScalar = 1.0f;
            if (gameModeManager.GetUseRandomizedCoins())
            {
                float randVal = Random.value;
                if (randVal < .34)
                {
                    laneDirectionScalar = 1.0f;
                }
                else if (randVal < .67)
                {
                    laneDirectionScalar = 0.0f;
                }
                else
                {
                    laneDirectionScalar = -1.0f;
                }
            }

            if (tracks[i].name.Contains("Curve") && tracks[i].name.Contains("Left"))
            {
                // Randomize the lane
                if (gameModeManager.GetUseRandomizedCoins())
                {
                    coinPos += tracks[i].transform.TransformDirection(Vector3.forward * laneDirectionScalar * 3.0f);
                }

                Quaternion rot = Quaternion.Euler(
                coinPrefab.transform.rotation.eulerAngles.x,
                coinPrefab.transform.rotation.eulerAngles.y + tracks[i].rotation.eulerAngles.y + 90,
                coinPrefab.transform.rotation.eulerAngles.z);
                for (int j = 0; j < clusterSize; j++)
                {
                    collectables.Add(
                        Instantiate(coinPrefab,
                            coinPos,
                            rot,
                            collectParent.transform));
                    coinPos += tracks[i].transform.TransformDirection(-Vector3.right) * clusterOffset;
                }
                numClustersPlaced += 1;
            }
            else
            {
                // Randomize the lane
                if (gameModeManager.GetUseRandomizedCoins())
                {
                    coinPos += tracks[i].transform.TransformDirection(Vector3.right * laneDirectionScalar * 3.0f);
                }

                Quaternion rot = Quaternion.Euler(
                coinPrefab.transform.rotation.eulerAngles.x,
                coinPrefab.transform.rotation.eulerAngles.y + tracks[i].rotation.eulerAngles.y,
                coinPrefab.transform.rotation.eulerAngles.z);
                for (int j = 0; j < clusterSize; j++)
                {
                    collectables.Add(
                        Instantiate(coinPrefab,
                            coinPos,
                            rot,
                            collectParent.transform));
                    coinPos += tracks[i].transform.TransformDirection(-Vector3.forward) * clusterOffset;
                }
                numClustersPlaced += 1;
            }
        }
        AdjustCollectables();
    }


    public void RemoveCollectables()
    {
        foreach (GameObject collectable in collectables)
        {
            if (collectable != null)
            {
                Destroy(collectable);
            }
        }
        collectables.Clear();
    }

    private void GetAllTracks(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == "Track" || child.name.Contains("Track"))
            {
                tracks.Add(child);
            }
            else if (child.childCount > 0)
            {
                GetAllTracks(child);
            }
        }
    }

    private void FilterTracks()
    {
        List<Transform> tempTracks = new List<Transform>();
        for (int i = 0; i < tracks.Count; i++)
        {
            if (Vector3.Distance(finishLine.position, tracks[i].position) > minDistanceFromFinishLine)
            {
                tempTracks.Add(tracks[i]);
            }
        }
        tracks = tempTracks;

    }

    private void AdjustCollectables()
    {
        RaycastHit hit;
        for (int i = 0; i < collectables.Count; i++)
        {

            if (Physics.Raycast(collectables[i].transform.position, -Vector3.up, out hit, coinLayer))
            {
                collectables[i].transform.position = new Vector3(collectables[i].transform.position.x, hit.point.y  + 1, collectables[i].transform.position.z);
            }
        }
    }

    // Fisher-Yates shuffle
    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
