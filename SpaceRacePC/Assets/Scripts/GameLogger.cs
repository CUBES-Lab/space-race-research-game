using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

public class GameLogger : MonoBehaviour
{
    private MemoryStream memoryStream;
    private TextWriter writer;
    bool hasInitialized = false;

    async void Awake()
    {
        if (!hasInitialized)
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                hasInitialized = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        memoryStream = new MemoryStream();
        writer = new StreamWriter(memoryStream);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LogEvent(string eventName, string message)
    {
        writer.Write(Time.timeSinceLevelLoad.ToString("0.000") + " - " + eventName.ToUpper() + ": " + message + "\n");
    }

    IEnumerator PostToCloudSave(string fileName, byte[] logBytes)
    {
        yield return CloudSaveService.Instance.Files.Player.SaveAsync(fileName, logBytes);
    }

    public void SaveLogCloudSave(string fileName)
    {
        writer.Flush();
        memoryStream.Position = 0;
        byte[] logBytes = memoryStream.ToArray();
        StartCoroutine(PostToCloudSave(fileName, logBytes));
    }
}
