using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.Track;

/// <summary>
/// This class handles what level is suppose to be transitioned to next.
/// </summary>
public class LevelManager : MonoBehaviour
{
    // The number of levels in the game
    public int numberOfLevels = 1; // BB - used to be 2

    // The current level.
    // This is set to 1, because this is not incremented in the title screen.
    public int curLevel = 1;

    public string endGameLevel = "EndGameScene";

    TrackManager trackManager;

    SceneController sceneController;

    CutsceneController cutsceneController;

    void Start()
    {
        sceneController = GetComponent<SceneController>();
        sceneController.AddLoadCallback(SetTrackManager);
        sceneController.AddLoadCallback(SetTimeline);
    }

    void Update()
    {
        if (curLevel != 0 && trackManager != null && trackManager.IsRaceStopped && GetLevel() != endGameLevel)
        {
            if (cutsceneController.enabled)
            {
                cutsceneController.playCutscene();
                if (!cutsceneController.isPlaying())
                {
                    trackManager = null;
                    IncrementLevel();
                    string nextLevel = GetLevel();
                    GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Level Load", "Loading level "+nextLevel);
                    sceneController.LoadScene(nextLevel);
                }
            } else
            {
                trackManager = null;
                IncrementLevel();
                string nextLevel = GetLevel();
                GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Level Load", "Loading level " + nextLevel);
                sceneController.LoadScene(nextLevel);
            }
        }
    }

    void SetTrackManager(string sceneName)
    {
        if (curLevel != 0)
        {
            trackManager = GameObject.Find("TrackManager").GetComponent<TrackManager>();
        }
    }

    void SetTimeline(string sceneName)
    {
        if (curLevel != 0)
        {
            cutsceneController = GameObject.Find("EndGameTimeline").GetComponent<CutsceneController>();
        }
    }

    public void LoadLevel(int levelNumber)
    {
        if (levelNumber > 0 && levelNumber <= numberOfLevels)
        {
            curLevel = levelNumber;
            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Level Load", "Loading level " + GetLevel());
            sceneController.LoadScene(GetLevel());
        }
        else
        {
            Debug.Log("ERROR: level number is outside of range");
        }
    }

    private void IncrementLevel()
    {
        ++curLevel;
    }

    string GetLevel()
    {
        if (curLevel > numberOfLevels)
        {
            return endGameLevel;
        }
        else 
        {
            return "Level" + curLevel;
        }
    }
}
