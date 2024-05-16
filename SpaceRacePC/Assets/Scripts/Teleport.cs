using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used on the portal for level 2 to make the kart teleport to a different part of the level.
/// </summary>
public class Teleport : MonoBehaviour
{
    public Transform teleportPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.position = teleportPos.position;
            other.transform.rotation = teleportPos.rotation;
        }
    }
}
