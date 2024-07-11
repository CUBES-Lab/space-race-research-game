using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.Text;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

public class RaceSession : MonoBehaviour
{
    private string uid;
    private string sessionID;
    private int score;
    private float raceTime;
    private int sessionCount;

    private string FORM_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScdN_c6MG1zYBGxKiXkw40638VtxJXaQ-1zfj6GlUoJT1MFYQ/formResponse";
    private string SHEET_QUERY_URL = "https://sheets.googleapis.com/v4/spreadsheets/1-ZbloFuaSxy8jxOQQlK3O6scjx5R4C6rc7tYs_ZYBKM/values/Form%20Responses%201!B1:B?key=AIzaSyBQ9jmBrITO6k0h2aDEqLx21cyaT3boBDk";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.uid = null;
        this.sessionID = null;
        this.score = 0;
        this.raceTime = 0.0f;
        this.sessionCount = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetUID()
    {
        return uid;
    }

    public string GetSessionID()
    {
        return sessionID;
    }

    public int GetScore()
    {
        return score;
    }

    public float GetRaceTime()
    {
        return raceTime;
    }

    IEnumerator GetSessionCount(string uid, Action callback = null)
    {
        UnityWebRequest www = UnityWebRequest.Get(SHEET_QUERY_URL);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            string updateText = "";
            string json = www.downloadHandler.text;
            var jsonObj = JSON.Parse(json);

            this.sessionCount = 0;
            var valuesArr = jsonObj["values"];
            for (int i = 1; i < valuesArr.Count; ++i)
            {
                if (valuesArr[i][0].Equals(uid))
                {
                    ++sessionCount;
                }
            }

            if (callback != null)
            {
                callback();
            }
        }
    }

    public void SetValues(string uid, int score, float raceTime)
    {
        this.uid = uid;
        this.score = score;
        this.raceTime = raceTime;
        this.sessionID = System.Guid.NewGuid().ToString();
    }

    IEnumerator Post(string uid, string sessionID, string score, string raceTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.299766424", uid);
        form.AddField("entry.2007611108", sessionID);
        form.AddField("entry.1788419136", score);
        form.AddField("entry.450286505", raceTime);
        byte[] formData = form.data;
        WWW www = new WWW(FORM_URL, formData);
        yield return www;
    }

    private void SaveNewSessionLogCallback()
    {
        GameLogger logger = GameObject.Find("GameLogger").GetComponent<GameLogger>();
        logger.SaveLogAsCloudPlayerData("Race"+this.sessionCount);

        StartCoroutine(Post(uid, sessionID, score.ToString(), raceTime.ToString("0.000")));
    }

    public void SaveNewSession(bool doSaveLog=true)
    {
        if (doSaveLog)
        {
            GameLogger logger = GameObject.Find("GameLogger").GetComponent<GameLogger>();
            // BB - Unity cloud save's player file storage works from the editor but not from any WebGL deployed build. We're using player data now instead.
            //logger.SaveLogAsCloudPlayerFile(sessionID + ".txt");
            logger.LogEvent("SESSION_ID", sessionID);

            StartCoroutine(GetSessionCount(uid, SaveNewSessionLogCallback));
        }
        else
        {
            StartCoroutine(Post(uid, sessionID, score.ToString(), raceTime.ToString("0.000")));
        }
    }
}
