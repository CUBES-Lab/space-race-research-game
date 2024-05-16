using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ShipMovment component is what make the spaceships move between different positions and adjust their rotation accordingly.
/// </summary>
public class ShipMovment : MonoBehaviour
{
    public Vector3[] targetPos;
    public float minDistanceToPos = 0.5f;
    public float speed = 5f;
    public float rotSpeed = 5f;
    
    
    private int curPos = 0;
    public bool stopAtDest;

    private Vector3 lookDir;
    private Quaternion lookRot;
    private bool reachedDest;
    private bool activatedShip;

    // Update is called once per frame
    void Update()
    {
        if ((!stopAtDest || !reachedDest))
        {
            if (Vector3.Distance(transform.position, targetPos[curPos]) <= minDistanceToPos)
            {
                if (curPos + 1 == targetPos.Length && stopAtDest)
                {
                    reachedDest = true;
                    return;
                }
                curPos = (curPos + 1) % targetPos.Length;
                
            }

            // Update position
            transform.position = Vector3.MoveTowards(transform.position, targetPos[curPos], speed * Time.deltaTime);

            // Update  rotation
            Vector3 lookDir = (targetPos[curPos] - transform.position).normalized;
            lookRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
        }
        else
        {
            if (!activatedShip)
            {
                GetComponent<Ship>().enabled = true;
                activatedShip = true;
            }
            
            lookRot = Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * rotSpeed);
        }
    }
}
