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

    private string FORM_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScdN_c6MG1zYBGxKiXkw40638VtxJXaQ-1zfj6GlUoJT1MFYQ/formResponse";
    private string SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1ev6Dj-eGm3t2aF4MsPoVpSVRACOzoi1KL7kRWmjkgPE/values/Form%20Responses%20%31?key=AIzaSyBQ9jmBrITO6k0h2aDEqLx21cyaT3boBDk";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.uid = null;
        this.sessionID = null;
        this.score = 0;
        this.raceTime = 0.0f;
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

    public void SetValues(string uid, int score, float raceTime)
    {
        this.uid = uid;
        this.score = score;
        this.raceTime = raceTime;
        this.sessionID = System.Guid.NewGuid().ToString();
    }

    //public void LoadFromLoginCallback()
    //{
    //    if (this.uid == null)
    //    {
    //        GameObject.Find("LoginMessage").GetComponent<TMP_Text>().text = "UID not found. Is it correct?";
    //    }
    //    else
    //    {
    //        GameObject gameManager = GameObject.Find("GameManager");
    //        GameModeManager gameModeManager = gameManager.GetComponent<GameModeManager>();
    //        gameModeManager.SetUseLeaderboard(this.usingLeaderboard);
    //        gameModeManager.SetUseRandomizedCoins(this.usingRandomizedCoins);

    //        GameObject.Find("Login").active = false;
    //        GameObject.Find("Canvas").transform.Find("MainMenu").gameObject.active = true; // BB - Use the parent Canvas to find the inactive MainMenu
    //    }
    //}

    //public void LoadFromLogin()
    //{
    //    string uid = GameObject.Find("UIDTextField").GetComponent<TMP_InputField>().text;

    //    StartCoroutine(Get(uid, LoadFromLoginCallback));

    //    GameObject.Find("LoginMessage").GetComponent<TMP_Text>().text = "Checking login...";
    //}

    //public void LoadFromUID(string uid)
    //{
    //    StartCoroutine(Get(uid));
    //}

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

    public void SaveNewSession(bool doSaveLog=true)
    {
        StartCoroutine(Post(uid, sessionID, score.ToString(), raceTime.ToString("0.000")));

        if (doSaveLog)
        {
            GameObject.Find("GameLogger").GetComponent<GameLogger>().SaveLogAsCloudPlayerFile(sessionID + ".txt");
        }
    }
}
