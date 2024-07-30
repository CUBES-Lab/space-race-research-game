using UnityEngine;

public class BoostpadManager : MonoBehaviour
{
    private bool doLogCollectablePositions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        doLogCollectablePositions = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (doLogCollectablePositions)
        {
            for (int i = 0; i < this.transform.childCount; ++i)
            {
                GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Init Boost", transform.GetChild(i).position.ToString());
            }
            doLogCollectablePositions = false;
        }
    }
}
