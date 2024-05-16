using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// This class controls when the cutscenes at the end of the levels should be played.
/// </summary>
public class CutsceneController : MonoBehaviour
{
    //Boolean to check if the cutsecene for the level has been played yet or not.
    bool played = false;
    PlayableDirector timeline;
    // Start is called before the first frame update
    void Start()
    {
        //Get the scene timeline gameobject that has the playable director component that controls the cutscenes.
        timeline = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCutscene()
    {
        //Only play if the cutscene is not currenlty playing and hasn't been played yet for the current scene
        if (!isPlaying() && !played)
        {
            timeline.Play();
            played = true;
        }
    }



    //Check if the cutscene is currenlty playing or not.
    public bool isPlaying()
    {
        if (timeline.state == PlayState.Playing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
