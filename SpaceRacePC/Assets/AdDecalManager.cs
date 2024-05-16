using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdDecalManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        GameObject adDecals = GameObject.Find("AdDecals");
        if (gameManager != null && adDecals != null)
        {
            AdManager adManager = gameManager.GetComponent<AdManager>();
            if (adManager != null)
            {
                Material decalMat = adManager.GetUseHighInvolvement() ? adManager.GetHighInvolvementMat() : adManager.GetLowInvolvementMat();
                foreach (Transform child in adDecals.transform)
                {
                    MeshRenderer meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material = decalMat;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
