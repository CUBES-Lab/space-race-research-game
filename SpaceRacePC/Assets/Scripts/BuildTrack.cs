using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   The BuildTrack compenent was used to quickly create straight tracks.
/// </summary>
public class BuildTrack : MonoBehaviour
{
    /* */
    public GameObject straightTrack;

    public Vector3 initRampPos;

    public float yRampOffset = 4.8f;
    public float zRampOffset = -8.3f;
    public float xRot = 30f;

    public int rampSize = 10;
    public bool shouldBuildRamp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBuildRamp)
        {
            BuildRamp();
            shouldBuildRamp = false;
        }
    }

    public void BuildRamp()
    {
        Vector3 curPos = initRampPos;
        for (int i = 0; i < rampSize; i++)
        {
            Quaternion rot = Quaternion.Euler(xRot, 0, 0);
            Vector3 pos = curPos;
            Instantiate(straightTrack, pos, rot, transform);
            curPos.y += yRampOffset;
            curPos.z += zRampOffset;
        }
    }
}
