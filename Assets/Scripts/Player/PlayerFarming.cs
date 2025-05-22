using UnityEngine;

public class PlayerFarming : MonoBehaviour
{
    [SerializeField] private Player player;

    public PlayerInventory playerInventory;
    private TileManager tileManager;

    [Header("Audio")]
    public AudioSource collect;
    public AudioSource water;
    public AudioSource plow;
    public AudioSource plant;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        tileManager = GameManager.instance.tileManager;
    }

    private void Update()
    {
        Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y - 1, 0);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (tileManager != null)
            {
                string tileName = tileManager.GetTileName(position);

                if (!string.IsNullOrWhiteSpace(tileName))
                {
                    if (tileManager.seededTiles.ContainsKey(position))
                    {
                        SeedData seedData = tileManager.seededTiles[position];
                        if (seedData.IsHarvestable)
                        {
                            // Harvest the crop
                            Item cropToYield = seedData.cropToYield;
                            int yieldAmount = seedData.yieldAmount;

                            // Add the harvested crop to the inventory
                            for (int i = 0; i < yieldAmount; i++)
                            {
                                playerInventory.Add("Hotbar", cropToYield);
                                collect.Play();
                            }

                            // Remove the seeded tile from the dictionary and set the tile back to dirt
                            tileManager.seededTiles.Remove(position);
                            tileManager.interactableMap.SetTile(position, tileManager.interactableTile);

                            Debug.Log("Harvested " + yieldAmount + " " + cropToYield.name);
                        }
                    }

                    if (tileName.Contains("Interactable"))
                    {


                        if (playerInventory.hotbar.selectedSlot.itemName == "Hoe")
                        {
                            if (tileName.Contains("Seed"))
                            {
                                Debug.Log("This tile has already been plowed up");
                            }
                            else
                            {
                                tileManager.SetPlowed(position);
                                plow.Play();
                            }
                        }
                        else if (playerInventory.hotbar.selectedSlot.itemName == "WateringCan")
                        {
                            tileManager.SetWatered(position);
                            water.Play();
                        }
                        else if (playerInventory.hotbar.selectedSlot.itemName.Contains("Seed"))
                        {
                            // get seed data from selected slot
                            if (playerInventory.hotbar.selectedSlot.count > 0)
                            {
                                if (tileName.Contains("Plow"))
                                {
                                    //tileManager.SetSeeded(position);
                                    tileManager.PlantSeed(position, playerInventory.hotbar.selectedSlot.seedData);

                                    //cropBehavior.PlantSeed(position, playerInventory.hotbar.selectedSlot.seedData);
                                    plant.Play();
                                    playerInventory.hotbar.selectedSlot.count--;

                                    Debug.Log("Planted Seed");
                                }
                                else
                                    Debug.Log("This tile has not been plowed up");

                            }
                            else
                            {
                                Debug.Log("You don't have any seeds to plant!");
                            }

                            playerInventory.inventoryUI.Refresh();
                        }
                    }
                }
            }
        }
    }

    public void DropItem(Item item)
    {
        Vector2 spawmLocation = transform.position;
        Vector2 spawnOffset = Random.insideUnitCircle * 1.5f;

        Item droppedItem = Instantiate(item, spawmLocation + spawnOffset, Quaternion.identity);

        // Makes the dropped item slide
        droppedItem.rb2D.AddForce(spawnOffset * 0.2f, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
