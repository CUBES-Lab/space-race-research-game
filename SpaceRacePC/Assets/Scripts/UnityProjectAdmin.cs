using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.Core;

public class UnityProjectAdmin : MonoBehaviour
{
    private string UNITY_CLOUD_SAVE_API_URL = "https://services.api.unity.com/cloud-save/v1/data/projects/ac2f7ffa-9123-4ef4-a8d0-184f20dfb589/environments/0f50aec4-88b5-4bf3-a515-571c0f4ac2d1/";
    private Dictionary<string, int> playerDataCounts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        this.playerDataCounts = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async Task<int> GetNumPlayerDataIDs()
    {
        if (this.playerDataCounts.Count == 0)
        {
            await FetchPlayerDataMetadata();
        }
        return this.playerDataCounts.Count;
    }

    public async Task<List<string>> GetPlayerDataIDs()
    {
        if (this.playerDataCounts.Count == 0)
        {
            await FetchPlayerDataMetadata();
        }
        return this.playerDataCounts.Keys.ToList();
    }

    public async Task FetchPlayerDataMetadata(Action callback = null)
    {
        UnityWebRequest www = UnityWebRequest.Get(UNITY_CLOUD_SAVE_API_URL+"players");
        www.SetRequestHeader("Authorization", "Basic N2ViYTU1YjctMzZjMS00MjU0LTk1ZDUtYjA3ZWI5MGM5MzJjOi0wY3R2bTRoTWZ6OHZ2NFZ3Z0xDaXhwVVF3WXVHUzFv");
        www.SendWebRequest();
        while (!www.isDone)
        {
            await Task.Yield();
        }
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("ERROR: " + www.error);
        }
        else
        {
            string updateText = "";
            string json = www.downloadHandler.text;
            var jsonObj = JSON.Parse(json);
            var resultsArry = jsonObj["results"];

            this.playerDataCounts.Clear();
            for (int i = 0; i < resultsArry.Count; ++i)
            {
                this.playerDataCounts.Add(resultsArry[i]["id"], resultsArry[i]["accessClasses"]["public"]["numKeys"]);
            }

            if (callback != null)
            {
                callback();
            }
        }
    }
}
