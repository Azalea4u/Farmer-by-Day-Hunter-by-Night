using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Seed Data", menuName = "ScriptableObjects/Seeds")]
public class SeedData : ItemData
{
    [Header("Seed")]

    [SerializeField] public int daysToGrow;
    [SerializeField] public Item cropToYield;
    [SerializeField] public Tile seedlingSprite;
    [SerializeField] public int yieldAmount;
    [SerializeField] public List<Tile> growthStageTiles = new List<Tile>();
    [SerializeField] public bool IsHarvestable;
    [SerializeField]
    public enum GrowthStage
    {
        SEEDLING,
        SPROUT,
        GROWTH,
        HARVESTABLE
    }
    [SerializeField] public GrowthStage eState;

    public SeedData() { }

    public void PlantSeed(Vector3Int tilePosition, Tilemap interactableMap)
    {
        interactableMap.SetTile(tilePosition, seedlingSprite);
        IsHarvestable = false;
    }

    public void Grow()
    {
        if (daysToGrow > 0)
        {
            daysToGrow--;
            Debug.Log("Days left: " + daysToGrow);
        }
    }
}