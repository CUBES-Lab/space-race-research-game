using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used in conjunction with AdManager and/or CollectableManager to indicate which type of ads/coins to spawn.
/// </summary>
public class GameModeManager : MonoBehaviour
{
    private bool useLeaderboard = false;
    private bool useRandomizedCoins = false;

    public void SetUseLeaderboard(bool useLeaderboard)
    {
        this.useLeaderboard = useLeaderboard;
    }

    public bool GetUseLeaderboard()
    {
        return useLeaderboard;
    }

    public void SetUseRandomizedCoins(bool useRandomizedCoins)
    {
        this.useRandomizedCoins = useRandomizedCoins;
    }

    public bool GetUseRandomizedCoins()
    {
        return useRandomizedCoins;
    }
    
    //public Material highInvolvementMat;
    //public Material lowInvolvementMat;
    //private bool useHighInvolvement = false;

    //public void ToggleUseHighInvolvement()
    //{
    //    useHighInvolvement = !useHighInvolvement;
    //}

    //public bool GetUseHighInvolvement()
    //{
    //    return useHighInvolvement;
    //}

    //public Material GetHighInvolvementMat()
    //{
    //    return highInvolvementMat;
    //}

    //public Material GetLowInvolvementMat()
    //{
    //    return lowInvolvementMat;
    //}
}
