using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public string playerID;       // Unique identifier (e.g., player network ID)
    public string playerName;
    public int health;
    public float energy;
    public int gold;
    public HotBar_Data hotbar_data;
    public Direction lastTravelledDirection;
    public string currentScene;

    public float maxEnergy = 100f;

    public float GetEnergyPercentage()
    {
        return Mathf.Clamp01(energy / maxEnergy);
    }

    public void ReduceEnergy(float amount)
    {
        energy = Mathf.Max(0, energy - amount);
        Debug.Log($"{playerName}'s energy reduced by {amount}. Current: {energy}");
    }

    public void AddEnergy(float amount)
    {
        energy = Mathf.Min(maxEnergy, energy + amount);
        Debug.Log($"{playerName} gained {amount} energy. Current: {energy}");
    }
}
