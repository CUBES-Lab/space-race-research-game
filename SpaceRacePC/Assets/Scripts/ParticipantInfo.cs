using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ParticipantInfo : MonoBehaviour
{
    private string uid;
    private string email;
    private bool usingLeaderboard;
    private bool usingRandomizedCoins;

    private string FORM_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSep4ld6T5ZEjDP-GzR2NAX6N14PO-1PeIf7g3eL3Fx_P6tAjA/formResponse";
    private string SHEET_URL = "https://sheets.googleapis.com/v4/spreadsheets/1e6kHgvgaoK1xcukUo4owkOGVNN15gyWs-hPPgCITKOU/values/Form%20Responses%20%31?key=AIzaSyBQ9jmBrITO6k0h2aDEqLx21cyaT3boBDk";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.uid = null;
        this.email = null;
        this.usingLeaderboard = false;
        this.usingRandomizedCoins = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetUID()
    {
        return uid;
    }

    public string GetEmail()
    {
        return email;
    }

    public bool GetUsingLeaderboard()
    {
        return usingLeaderboard;
    }

    public bool GetUsingRandomizedCoins()
    {
        return usingRandomizedCoins;
    }

    IEnumerator Get(string uid, Action callback = null)
    {
        UnityWebRequest www = UnityWebRequest.Get(SHEET_URL);
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

            var valuesArr = jsonObj["values"];
            var header = valuesArr[0].AsArray;
            int uidIndex = -1;
            int emailIndex = -1;
            int leaderboardIndex = -1;
            int randomizedCoinsIndex = -1;
            for (int i = 0; i < header.Count; ++i)
            {
                if (header[i].Equals("UID"))
                {
                    uidIndex = i;
                }
                else if (header[i].Equals("Email"))
                {
                    emailIndex = i;
                }
                else if (header[i].Equals("Leaderboard?"))
                {
                    leaderboardIndex = i;
                }
                else if (header[i].Equals("Randomized Coins?"))
                {
                    randomizedCoinsIndex = i;
                }
            }
            if (uidIndex < 0 || emailIndex < 0 || leaderboardIndex < 0 || randomizedCoinsIndex < 0)
            {
                Debug.Log("ERROR: Unable to parse JSON header");
            }

            for (int i = 1; i < valuesArr.Count; ++i)
            {
                if (valuesArr[i][uidIndex].Equals(uid))
                {
                    this.uid = uid;
                    this.email = valuesArr[i][emailIndex];
                    this.usingLeaderboard = valuesArr[i][leaderboardIndex].Equals("Yes");
                    this.usingRandomizedCoins = valuesArr[i][randomizedCoinsIndex].Equals("Yes");
                    break;
                }
            }

            if (callback != null)
            {
                callback();
            }
        }
    }

    public void LoadFromLoginCallback()
    {
        if (this.uid == null)
        {
            GameObject.Find("LoginMessage").GetComponent<TMP_Text>().text = "UID not found. Is it correct?";
        }
        else
        {
            GameObject gameManager = GameObject.Find("GameManager");
            GameModeManager gameModeManager = gameManager.GetComponent<GameModeManager>();
            gameModeManager.SetUseLeaderboard(this.usingLeaderboard);
            gameModeManager.SetUseRandomizedCoins(this.usingRandomizedCoins);

            GameObject.Find("Login").active = false;
            GameObject.Find("Canvas").transform.Find("MainMenu").gameObject.active = true; // BB - Use the parent Canvas to find the inactive MainMenu

            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Login", "Participant " + this.uid + " logged in successfully");
        }
    }

    public void LoadFromLogin()
    {
        string uid = GameObject.Find("UIDTextField").GetComponent<TMP_InputField>().text;

        StartCoroutine(Get(uid, LoadFromLoginCallback));

        GameObject.Find("LoginMessage").GetComponent<TMP_Text>().text = "Checking login...";
    }

    public void LoadFromUID(string uid)
    {
        StartCoroutine(Get(uid));
    }

    //IEnumerator Post(string uid, string email, string usingLeaderboard, string usingRandomizedCoins)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("entry.299766424", uid);
    //    form.AddField("entry.2007611108", email);
    //    form.AddField("entry.2128522933", usingLeaderboard);
    //    form.AddField("entry.1028502878", usingRandomizedCoins);
    //    byte[] formData = form.data;
    //    WWW www = new WWW(FORM_URL, formData);
    //    yield return www;
    //}

    //public void Save()
    //{
    //    StartCoroutine(Post(uid, email, usingLeaderboard, usingRandomizedCoins));
    //}
}
