using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Unity.Services.Leaderboards;

public class GameLogger : MonoBehaviour
{
    private MemoryStream memoryStream;
    private TextWriter writer;

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

    public void SaveLogAsCloudPlayerFile(string fileName)
    {
        writer.Flush();
        memoryStream.Position = 0;
        byte[] logBytes = memoryStream.ToArray();
        GameObject.Find("AsyncServices").GetComponent<AsyncServices>().SavePlayerFileToCloud(fileName, logBytes);
    }

    public void SaveLogAsCloudPlayerData(string key)
    {
        writer.Flush();
        memoryStream.Position = 0;
        byte[] logBytes = memoryStream.ToArray();
        string logString = System.Text.Encoding.UTF8.GetString(logBytes);
        GameObject.Find("AsyncServices").GetComponent<AsyncServices>().SavePlayerDataToCloud(key, logString);
    }
}
