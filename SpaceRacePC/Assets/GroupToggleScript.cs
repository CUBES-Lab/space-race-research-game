using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroupToggleScript : MonoBehaviour
{
    private int group = 1;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameModeManager gameModeManager = gameManager.GetComponent<GameModeManager>();
        gameModeManager.SetUseRandomizedCoins(false);
        gameModeManager.SetUseLeaderboard(false);
        group = 1;
        TextMeshProUGUI textUI = this.gameObject.GetComponent<TextMeshProUGUI>();
        textUI.text = "Group " + group.ToString();
    }

    public void IncrementGroup()
    {
        group = group % 4 + 1;
        TextMeshProUGUI textUI = this.gameObject.GetComponent<TextMeshProUGUI>();
        textUI.text = "Group " + group.ToString();

        GameObject gameManager = GameObject.Find("GameManager");
        GameModeManager gameModeManager = gameManager.GetComponent<GameModeManager>();

        // Group 1: no randomized coins, no leaderboard
        // Group 2: randomized coins, no leaderboard
        // Group 3: no randomized coins, leaderboard
        // Group 4: randomized coins, leaderboard
        switch(group)
        {
            case 1:
                gameModeManager.SetUseRandomizedCoins(false);
                gameModeManager.SetUseLeaderboard(false);
                break;
            case 2:
                gameModeManager.SetUseRandomizedCoins(true);
                gameModeManager.SetUseLeaderboard(false);
                break;
            case 3:
                gameModeManager.SetUseRandomizedCoins(false);
                gameModeManager.SetUseLeaderboard(true);
                break;
            case 4:
                gameModeManager.SetUseRandomizedCoins(true);
                gameModeManager.SetUseLeaderboard(true);
                break;
        }
    }
}
