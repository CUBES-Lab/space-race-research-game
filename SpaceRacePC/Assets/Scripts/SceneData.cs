using UnityEngine;

/// <summary>
/// This stores the initial position of the kart in each level.
/// </summary>
[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData", order = 1)]
public class SceneData : ScriptableObject
{
    public Vector3 level1KartPos;
    public Vector3 level2KartPos;
}
