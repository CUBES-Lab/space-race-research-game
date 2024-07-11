using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDataMonitor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void UpdatePlayerDataCount()
    {
        UnityProjectAdmin uAdmin = GameObject.Find("UnityAdmin").GetComponent<UnityProjectAdmin>();
        int playerDataCount = await uAdmin.GetNumPlayerDataIDs();
        GameObject playerDataCountObj = GameObject.Find("Canvas").transform.Find("PlayerDataPlayerCount").gameObject;
        GameObject playerDataCountText = playerDataCountObj.transform.Find("PlayerDataCountText").gameObject;
        playerDataCountText.GetComponent<TMP_Text>().text = ""+playerDataCount;

        List<string> playerDataIDs = await uAdmin.GetPlayerDataIDs();
        GameObject playerDataIDsText = playerDataCountObj.transform.Find("PlayerDataIDsText").gameObject;
        playerDataIDsText.GetComponent<TMP_Text>().text = string.Join(",", playerDataIDs);
    }

    public async void DownloadPlayerData()
    {
        AsyncServices asyncServices = GameObject.Find("AsyncServices").GetComponent<AsyncServices>();
        UnityProjectAdmin uAdmin = GameObject.Find("UnityAdmin").GetComponent<UnityProjectAdmin>();
        string pathPrefix = GameObject.Find("DownloadPlayerData").transform.Find("DownloadPathTextInput").gameObject.GetComponent<TMP_InputField>().text;

        List<string> playerDataIDs = await uAdmin.GetPlayerDataIDs();
        for (int i = 0; i < playerDataIDs.Count; ++i)
        {
            Dictionary<string, string> playerDataDict = await asyncServices.GetPublicDataByPlayerID(playerDataIDs[i]);

            foreach (KeyValuePair<string, string> entry in playerDataDict)
            {
                string path = Path.Join(pathPrefix, playerDataIDs[i] + "_" +entry.Key+".log");

                StreamWriter writer = new StreamWriter(path, true);
                writer.Write(entry.Value);
                writer.Close();
            }
        }
    }
}
