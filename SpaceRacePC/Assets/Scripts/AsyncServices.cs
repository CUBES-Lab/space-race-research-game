using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;

public class AsyncServices : MonoBehaviour
{
    public delegate void AsyncServicesReturnCallback(System.Object retObj);
    public delegate void AsyncServicesNotifyCallback();

    bool isSignedIn = false;

    async void Awake()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("study");

            await UnityServices.InitializeAsync(options);
            await SignInAnonymously();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            // Take some action here...
            Debug.Log(s);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        isSignedIn = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }


    /*
     * Authentication
     */
    public string GetPlayerID()
    {
        return AuthenticationService.Instance.PlayerId;
    }

    public async void UpdatePlayerName(string newPlayerName)
    {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(newPlayerName);
    }

    /*
     * Cloud Storage
     */
    public async Task<Dictionary<string,string>> GetPublicDataByPlayerID(string playerID)
    {
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAllAsync(new LoadAllOptions(new PublicReadAccessClassOptions(playerID)));
        //Debug.Log($"{playerData.Count} elements loaded from player ID {playerID}!");

        Dictionary<string, string> outDict = new Dictionary<string, string>();
        foreach (var result in playerData)
        {
            outDict.Add(result.Key, result.Value.Value.GetAsString());
            //Debug.Log($"Key: {result.Key}, Value: {result.Value.Value.GetAsString()}");
        }

        return outDict;
    }

    public async Task<List<string>> GetPlayerFileNames()
    {
        List<FileItem> files = await CloudSaveService.Instance.Files.Player.ListAllAsync();
        List<string> fileNames = new List<string>();

        for (int i = 0; i < files.Count; i++)
        {
            fileNames.Add(files[i].Key);
        }
        return fileNames;
    }

    public async void SavePlayerDataToCloud(string dataKey, object data)
    {
        var dataDict = new Dictionary<string, object> { { dataKey, data } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(dataDict, new SaveOptions(new PublicWriteAccessClassOptions()));
    }

    public async void SavePlayerFileToCloud(string fileName, byte[] fileBytes)
    {
        await CloudSaveService.Instance.Files.Player.SaveAsync(fileName, fileBytes);
    }


    /*
     * Leaderboard
     */
    public async Task<LeaderboardScoresPage> GetLeaderboardScores(string leaderboardID, int initialRank = -1, int range = -1)
    {
        if (initialRank > -1 && range > 0)
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, new GetScoresOptions { Offset = initialRank, Limit = range });
            return scoresResponse;
        }
        else
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID);
            return scoresResponse;
        }
    }

    public async Task<LeaderboardEntry> GetLeaderboardPlayerScore(string leaderboardID)
    {
        var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardID);
        return scoreResponse;
    }

    public async Task<LeaderboardEntry> PostLeaderboardPlayerScore(string leaderboardID, float leaderboardScore)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, leaderboardScore);
        return scoreResponse;
    }
}
