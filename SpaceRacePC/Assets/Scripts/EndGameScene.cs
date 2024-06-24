using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class EndGameScene : MonoBehaviour
{
    const string LeaderboardId = "Level1Leaderboard";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        if (GameObject.Find("GameManager") != null)
        {
            ParticipantInfo participantInfo = GameObject.Find("ParticipantInfo").GetComponent<ParticipantInfo>();
            RaceSession raceSession = GameObject.Find("GameManager").GetComponent<RaceSession>();

            string uid = participantInfo.GetUID();
            float raceTime = raceSession.GetRaceTime();
            int score = raceSession.GetScore();
            float finalScore = raceTime - score;
            GameObject.Find("UID").GetComponent<TMP_InputField>().text = "UID: "+uid;
            GameObject.Find("RaceTime").GetComponent<TMP_InputField>().text = "Time: "+raceTime.ToString("0.000");
            GameObject.Find("Score").GetComponent<TMP_InputField>().text = "Coins: " + score.ToString();
            GameObject.Find("FinalScore").GetComponent<TMP_InputField>().text = "Final Time: "+ finalScore.ToString("0.000");

            if (participantInfo.GetUsingLeaderboard())
            {
                //AsyncServices asyncServices = GameObject.Find("AsyncServices").GetComponent<AsyncServices>();
                var scores = await GameObject.Find("AsyncServices").GetComponent<AsyncServices>().Test();
                Debug.Log(JsonConvert.SerializeObject(scores));
                print("AFTER TEST");
                //Task.Run(() => UpdateAndShowLeaderboard(asyncServices, finalScore));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void UpdateAndShowLeaderboard(AsyncServices asyncServices, float newPlayerScore)
    {
        Debug.Log("Getting RESULTS");
        //var leaderboardData = await asyncServices.GetLeaderboardScores(LeaderboardId);
        //var leaderboardData = asyncServices.Test();
        asyncServices.Test();
        Debug.Log("GOT RESULTS");
        //Debug.Log(JsonConvert.SerializeObject(leaderboardData));
        //await asyncServices.PostLeaderboardPlayerScore(LeaderboardId, newPlayerScore);
        //GameObject.Find("Canvas").transform.Find("Leaderboard").gameObject.active = true;
    }
}