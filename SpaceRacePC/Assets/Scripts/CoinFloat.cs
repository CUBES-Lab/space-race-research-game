using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This component is what allows for the coins to float at random different speeds. Makes them look more lively.
/// </summary>
public class CoinFloat : MonoBehaviour
{

    public float yOffset = 2f;
    
    public float minSpeed = 0.5f, maxSpeed = 1f;
    public float distThreshold = 0.25f;

    private Vector3 initPos, downPos, upPos;
    private float initTime;

    private bool goingDown;

        private float speed;

    private void Start()
    {
        initPos = transform.position;
        initTime = Time.realtimeSinceStartup;
        downPos = initPos + Vector3.down * yOffset;
        upPos = initPos + -Vector3.down * yOffset;
        // Add some randomization in their floating
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float vertOffset = yOffset*Mathf.Sin(2*Mathf.PI*(Time.realtimeSinceStartup-initTime)*speed);
        transform.position = initPos + new Vector3(0, vertOffset, 0);
        /*
        if (goingDown)
        {
            transform.position = Vector3.Lerp(transform.position, downPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, downPos) < distThreshold)
            {
                goingDown = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, upPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, upPos) < distThreshold)
            {
                goingDown = true;
            }
        }
        */
    }
}
