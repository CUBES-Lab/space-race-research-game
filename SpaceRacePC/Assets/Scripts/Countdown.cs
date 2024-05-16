using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour

    //This class will control the delta  time countdown for VR players to get their headsets ready for VR play//


{
    float currentTime;
    [SerializeField] float startingTime;

    [SerializeField] TextMeshProUGUI countdownText;

    [SerializeField] GameObject gameManager;

    bool canCount = false;

    private void Start()
    {
        //The gameobject hosting the scene control is the gamemanager so we grab the gamemanager so it can switch scenes once the coutdown is done.
        gameManager = GameObject.Find("GameManager");
        //Make the current time the specified amount of time we want to countdown from.
        currentTime = startingTime;
    }

    private void Update()
    {
        //Only want countdown to happen once per title screen scene so check if countdown has been played before or not.
        if (canCount)
        {
            //decrement current time by 1 every real world second by using delta time.
            currentTime -= 1 * Time.deltaTime;
            //To string the current time so its changes on the UI elements.
            countdownText.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                //Once the countdown reaches 0 change to the next scene.
                currentTime = 0;
                SceneController sn = gameManager.GetComponent<SceneController>();
                sn.LoadScene("Level1");
            }

            //For looks when the countdown reaches 5.5 seconds the text on the screen will turn red.
            if (currentTime <= 5.5)
            {
                countdownText.color = Color.red;
            }
        }
    }


    //Funciton to change the bool value of canCount
    public void CountBtn()
    {
        canCount = true;
    }
}
