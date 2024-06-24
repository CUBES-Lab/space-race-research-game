using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Unity.Services.Leaderboards.Models;

public class EndGameScene : MonoBehaviour
{
    const string LeaderboardIDFixedCoins = "Level1Leaderboard";
    const string LeaderboardIDRandomizedCoins = "Level1RandomizedCoins";

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
            GameObject.Find("Score").GetComponent<TMP_InputField>().text = "Coins: " + score.ToString() + "/100";
            GameObject.Find("FinalScore").GetComponent<TMP_InputField>().text = "Final Time: "+ finalScore.ToString("0.000");

            if (participantInfo.GetUsingLeaderboard())
            {
                string LeaderboardID = participantInfo.GetUsingRandomizedCoins() ? LeaderboardIDRandomizedCoins : LeaderboardIDFixedCoins;
                AsyncServices asyncServices = GameObject.Find("AsyncServices").GetComponent<AsyncServices>();
                var playerScore = await asyncServices.PostLeaderboardPlayerScore(LeaderboardID, finalScore);
                var scores = await asyncServices.GetLeaderboardScores(LeaderboardID);

                GameObject leaderboard = GameObject.Find("Canvas").transform.Find("Leaderboard").gameObject; // BB - Use the parent Canvas to find the inactive MainMenu
                TMP_InputField firstRankText = leaderboard.transform.Find("FirstRank").GetComponent<TMP_InputField>();
                TMP_InputField secondRankText = leaderboard.transform.Find("SecondRank").GetComponent<TMP_InputField>();
                TMP_InputField thirdRankText = leaderboard.transform.Find("ThirdRank").GetComponent<TMP_InputField>();
                TMP_InputField[] rankTexts = new TMP_InputField[] { firstRankText, secondRankText, thirdRankText };
                for (int i = 0; i < 3; ++i)
                {
                    if (i < scores.Results.Count)
                    {
                        rankTexts[i].text = (scores.Results[i].Rank + 1).ToString() + ") " + scores.Results[i].Score.ToString("0.000") + " [" + scores.Results[i].PlayerName.Split('#')[0] + "]";
                    }
                }

                TMP_InputField playerRankText = leaderboard.transform.Find("PlayerLeaderboard").transform.Find("PlayerRank").GetComponent<TMP_InputField>();
                playerRankText.text = "You are rank " + (playerScore.Rank + 1).ToString();

                leaderboard.active = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}