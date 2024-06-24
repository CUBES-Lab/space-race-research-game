using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// The SceneController is handles transitions between scenes and firing a delegate to perform operations required at the  beginning of a level loaded.
/// </summary>
public class SceneController : MonoBehaviour
{
    public delegate void LevelLoaded(string sceneName);

    LevelLoaded load;

    private static SceneController instance = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        instance = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void RunOnStart()
    {
        instance = null;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(GameObject.Find("ParticipantInfo"));
            DontDestroyOnLoad(GameObject.Find("GameLogger"));
            DontDestroyOnLoad(GameObject.Find("AsyncServices"));
            load = null;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            // Have to reset this to the old gameManager as the currently reference the one in scene
            GameObject startVRObj = GameObject.Find("StartVRButton");
            if (startVRObj != null)
            {
                GameObject.Find("StartVRButton").GetComponent<Button>().onClick.AddListener(
                () => { instance.gameObject.GetComponent<KartManager>().SetIsVR(true); });
            }

            GameObject startMobileObj = GameObject.Find("StartMobileButton");
            if (startMobileObj != null)
            {
                GameObject.Find("StartMobileButton").GetComponent<Button>().onClick.AddListener(
                () => { instance.gameObject.GetComponent<KartManager>().SetIsVR(false); });
                GameObject.Find("StartMobileButton").GetComponent<Button>().onClick.AddListener(
                    () => { instance.LoadScene("Level1"); });
            }

            // Destroy instance that is in this scene as you don't want duplciates
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Fires subscribed methods if a racetrack is loaded.
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //print("OnSceneLoaded " + instance);
        //print(gameObject);
        // Check if you loaded a playable level
        if (scene.name.Contains("Level"))
        {
            load(scene.name);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);                
    }

    public void AddLoadCallback(LevelLoaded callback)
    {
        load += callback;
    }
}
