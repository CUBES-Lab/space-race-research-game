using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Key Down", "Left");
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Key Up", "Left");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Key Down", "Right");
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Key Up", "Right");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Key Down", "Space");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Key Up", "Space");
        }
    }
}
