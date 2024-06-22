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
        if (curLevel != 0 && trackManager != null && trackManager.IsRaceStopped)
        {
            if (cutsceneController.enabled)
            {
                cutsceneController.playCutscene();
                if (!cutsceneController.isPlaying())
                {
                    trackManager = null;
                    sceneController.LoadScene(GetNextLevel());
                }
            } else
            {
                trackManager = null;
                sceneController.LoadScene(GetNextLevel());
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
            cutsceneController = GameObject.Find("Timeline").GetComponent<CutsceneController>();
        }

    }

    string GetNextLevel()
    {
        ++curLevel;
        if (curLevel > numberOfLevels)
        {
            curLevel = 1;
            return "Title";
        }
        else 
        {
            return "Level" + curLevel;
        }
    }
}
