using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used in conjunction with AdManager and/or CollectableManager to indicate which type of ads/coins to spawn.
/// </summary>
public class AdManager : MonoBehaviour
{
    public Material highInvolvementMat;
    public Material lowInvolvementMat;
    private bool useHighInvolvement = false;

    public void ToggleUseHighInvolvement()
    {
        useHighInvolvement = !useHighInvolvement;
    }

    public bool GetUseHighInvolvement()
    {
        return useHighInvolvement;
    }

    public Material GetHighInvolvementMat()
    {
        return highInvolvementMat;
    }

    public Material GetLowInvolvementMat()
    {
        return lowInvolvementMat;
    }
}
