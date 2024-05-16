using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allows for procedural floating of the ships. It makes them look less static.
/// </summary>
public class Ship : MonoBehaviour
{
    // Yes I know Ship and ShipMovement should be one script...

    public float yOffset = 2f;

    public float speed = 1f;
    
    private Vector3 initPos, downPos, upPos;
    
    private bool goingDown;

    private float randomThreshold;


    private void OnEnable()
    {
        initPos = transform.position;
        downPos = initPos + Vector3.down * yOffset;
        upPos = initPos + -Vector3.down * yOffset;
        // Add some randomization in their floating
        randomThreshold = Random.Range(0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (goingDown)
        {
            transform.position = Vector3.Lerp(transform.position, downPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, downPos) < randomThreshold)
            {
                goingDown = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, upPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, upPos) < randomThreshold)
            {
                goingDown = true;
            }
        }
        
    }
}
