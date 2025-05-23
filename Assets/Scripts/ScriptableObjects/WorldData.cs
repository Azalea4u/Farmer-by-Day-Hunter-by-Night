using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WorldData")]
public class WorldData : ScriptableObject
{
    public string worldID;
    public string worldName;

    public List<Player> players;
}
