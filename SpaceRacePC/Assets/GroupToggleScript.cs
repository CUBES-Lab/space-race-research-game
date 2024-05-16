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
        AdManager adManager = gameManager.GetComponent<AdManager>();
        group = adManager.GetUseHighInvolvement() ? 2 : 1;
        TextMeshProUGUI textUI = this.gameObject.GetComponent<TextMeshProUGUI>();
        textUI.text = "Group " + group.ToString();
    }

    public void ToggleGroupText()
    {
        group = 2 + (1 - group);
        TextMeshProUGUI textUI = this.gameObject.GetComponent<TextMeshProUGUI>();
        textUI.text = "Group " + group.ToString();

        GameObject gameManager = GameObject.Find("GameManager");
        AdManager adManager = gameManager.GetComponent<AdManager>();
        adManager.ToggleUseHighInvolvement();
    }
}
