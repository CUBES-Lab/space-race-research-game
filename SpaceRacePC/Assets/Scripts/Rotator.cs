using UnityEngine;
using System.Collections;

/// <summary>
/// The Rotator component was used to rotate the collectables.
/// </summary>
public class Rotator : MonoBehaviour {


    public Vector3 rotation;
    void Update () 
    {
        transform.Rotate (rotation * Time.deltaTime);
    }
}
