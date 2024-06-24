using UnityEngine;
using UnityEngine.UI;

public class EndGamePlayAgain : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(PlayAgain);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAgain()
    {
        GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Play Again", "Player is playing again");
        GameObject.Find("GameManager").GetComponent<LevelManager>().LoadLevel(1);
    }
}
