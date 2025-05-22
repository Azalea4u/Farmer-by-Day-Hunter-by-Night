using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] public Tilemap interactableMap;
    [SerializeField] public Tilemap backgroundMap;

    [Header("Tiles")]
    [SerializeField] public Tile interactableTile;
    [SerializeField] public Tile dirtTile;
    [SerializeField] public Tile plowedTile;
    [SerializeField] public Tile wateredTile;
    [SerializeField] public Tile seededTile;

    public Dictionary<Vector3Int, SeedData> seededTiles = new Dictionary<Vector3Int, SeedData>();

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null && tile.name == "Interactable")
            {
                interactableMap.SetTile(position, interactableTile);
            }
        }
    }

    public void SetPlowed(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);
    }

    public void SetWatered(Vector3Int position)
    {
        backgroundMap.SetTile(position, wateredTile);
    }

    public void SetSeeded(Vector3Int position)
    {
        interactableMap.SetTile(position, seededTile);

    }
    public void PlantSeed(Vector3Int position, SeedData seedData)
    {
        interactableMap.SetTile(position, seedData.seedlingSprite);
        seededTiles.Add(position, seedData);

        seedData.daysToGrow = 4;
        seedData.eState = SeedData.GrowthStage.SEEDLING;
        seedData.IsHarvestable = false;

        //seededTiles[position] = seedData;
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return "";
    }

    public void ResetWateredTiles()
    {
        foreach (var position in backgroundMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = backgroundMap.GetTile(position);
            if (tile != null && tile == wateredTile)
            {
                backgroundMap.SetTile(position, dirtTile);
            }
        }
    }

    public void UpdateSeededTiles()
    {
        List<Vector3Int> tilesToRemove = new List<Vector3Int>();

        foreach (var entry in seededTiles)
        {
            Vector3Int position = entry.Key;
            SeedData seedData = entry.Value;

            // Check if the tile has been watered
            if (backgroundMap.GetTile(position) == wateredTile)
            {
                // If watered, increment the growth stage
                if (seedData.eState < SeedData.GrowthStage.HARVESTABLE)
                {
                    seedData.eState++;
                    interactableMap.SetTile(position, seedData.growthStageTiles[(int)seedData.eState]);
                    if (seedData.eState == SeedData.GrowthStage.HARVESTABLE)
                    {
                        seedData.IsHarvestable = true;
                    }
                }
            }
            else
            {
                // If not watered, add one day to the days to grow
                seedData.daysToGrow = Mathf.Min(seedData.daysToGrow + 1, seedData.daysToGrow);
            }
        }

        foreach (var position in tilesToRemove)
        {
            seededTiles.Remove(position);
        }
    }
}
