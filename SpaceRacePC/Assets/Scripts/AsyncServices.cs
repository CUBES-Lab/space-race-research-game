using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Leaderboards;

public class AsyncServices : MonoBehaviour
{
    public delegate void AsyncServicesReturnCallback(System.Object retObj);
    public delegate void AsyncServicesNotifyCallback();

    bool isSignedIn = false;

    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
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
     * Cloud Storage
     */
    private async void SavePlayerFileUnity(string fileName, byte[] fileBytes)
    {
        await CloudSaveService.Instance.Files.Player.SaveAsync(fileName, fileBytes);
    }

    public void SavePlayerFileToCloud(string fileName, byte[] fileBytes)
    {
        Task.Run(() => SavePlayerFileUnity(fileName, fileBytes));
    }


    /*
     * Leaderboard
     */
    private async Task<System.Object> GetLeaderboardScoresUnity(string leaderboardID, AsyncServicesReturnCallback callback = null, int initialRank = -1, int range = -1)
    {
        if (initialRank > -1 && range > 0)
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID, new GetScoresOptions { Offset = initialRank, Limit = range });
            if (callback != null)
            {
                callback(scoresResponse);
            }
            return scoresResponse;
        }
        else
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(leaderboardID);
            if (callback != null)
            {
                callback(scoresResponse);
            }
            return scoresResponse;
        }
    }

    private async Task<System.Object> GetLeaderboardPlayerScoreUnity(string leaderboardID, AsyncServicesReturnCallback callback = null)
    {
        var scoreResponse = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardID);
        if (callback != null)
        {
            callback(scoreResponse);
        }
        return scoreResponse;
    }

    private async Task<System.Object> PostLeaderboardPlayerScoreUnity(string leaderboardID, float leaderboardScore, AsyncServicesNotifyCallback callback = null)
    {
        var scoreResponse = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardID, leaderboardScore);
        if (callback != null)
        {
            callback();
        }
        return scoreResponse;
    }

    public async void GetLeaderboardScoresAsync(string leaderboardID, AsyncServicesReturnCallback callback, int initialRank = -1, int range = -1)
    {
        Task.Run(() => GetLeaderboardScoresUnity(leaderboardID, callback, initialRank, range));
    }

    public async Task<System.Object> Test()
    {
        //await UnityServices.InitializeAsync();
        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
        print("FETCHING LEADERBOARD");
        var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync("Level1Leaderboard");
        return scoresResponse;
    }

    public async Task<System.Object> GetLeaderboardScores(string leaderboardID, int initialRank = -1, int range = -1)
    {
        return await Task.Run(() => GetLeaderboardScoresUnity(leaderboardID, null, initialRank, range));
    }

    public async void GetLeaderboardPlayerScoreAsync(string leaderboardID, AsyncServicesReturnCallback callback)
    {
        Task.Run(() => GetLeaderboardPlayerScoreUnity(leaderboardID, callback));
    }

    public async Task<System.Object> GetLeaderboardPlayerScore(string leaderboardID, AsyncServicesReturnCallback callback)
    {
        return await Task.Run(() => GetLeaderboardPlayerScoreUnity(leaderboardID));
    }

    public async void PostLeaderboardPlayerScoreAsync(string leaderboardID, float leaderboardScore, AsyncServicesNotifyCallback callback)
    {
        Task.Run(() => PostLeaderboardPlayerScoreUnity(leaderboardID, leaderboardScore, callback));
    }

    public async Task<System.Object> PostLeaderboardPlayerScore(string leaderboardID, float leaderboardScore)
    {
        return await Task.Run(() => PostLeaderboardPlayerScoreUnity(leaderboardID, leaderboardScore));
    }
}
