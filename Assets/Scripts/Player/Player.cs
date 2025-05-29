using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;
using static ItemData;
using System.Globalization;
using UnityEngine.SceneManagement;

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

[RequireComponent(typeof(PlayerController))]
public class Player : NetworkBehaviour
{
    [HideInInspector] public PlayerInventory inventoryManager;
    
    [SerializeField] private PlayerController controller;

    [SerializeField] private HotBar_Data hotBar_Data;
    [SerializeField] private HotBar_Data inventory_Data;
    [SerializeField] public PlayerData playerData;

    [SerializeField] public CameraFollow playerCamera;

    public Direction lastTravelledDirection;

    public bool inFarm = false;
    public string currentScene;


    private void Start()
    {
        if (inventoryManager == null) inventoryManager = GetComponent<PlayerInventory>();
        if (controller == null) controller = GetComponent<PlayerController>();

        if (IsOwner) GameManager.instance.player = this;

        currentScene = playerData.currentScene;
    }

    public override void OnNetworkSpawn()
    {
        print("Player spawned");
    }

    private void Update()
    {
        // !! REMINDER !!
        /// Add designated functions/calls for pausing & consuming an item in PlayerController

        if (false && !GameManager.instance.IsGamePaused)
            //&& !
            //Manager.instance.dialogueIsPlaying)
        {
            ConsumeItem();

            // Refresh both UI
            inventoryManager.SaveInventoryData("Hotbar");
            inventoryManager.SaveInventoryData("Inventory");
            inventoryManager.inventoryUI.Refresh();
        }
    }

    // New method to handle both food and potion consumption based on HealingType
    private void ConsumeItem()
    {
        var selectedSlot = inventoryManager.hotbar.selectedSlot;

        if (selectedSlot != null && !string.IsNullOrEmpty(selectedSlot.itemName))
        {
            var item = GameManager.instance.itemManager.GetItemByName(selectedSlot.itemName);

            if (item != null && item.IsFood)
            {
                // Check the HealingType and call the appropriate method
                if (item.HealingType == HealingTypes.Energy)
                {
                    // If the item heals hunger, consume it as food
                    //ConsumeFood(item, selectedSlot);
                }
                else if (item.HealingType == HealingTypes.Health)
                {
                    // If the item heals health, consume it as a potion
                    //ConsumePotion(item, selectedSlot);
                }
            }
        }
    }

    /* Consume Items WIP
    // Consume food and heal hunger
    private void ConsumeFood(Item item, Inventory.Slot selectedSlot)
    {
        if (GameManager.instance.playerUI.Hunger < 100)
        {
            HungerData hungerData = GameManager.instance.playerUI.hungerData;
            hungerData.Hunger = Mathf.Min(100, hungerData.Hunger + item.HealingAmount);

            // Remove the food item
            if (selectedSlot.count > 0)
            {
                selectedSlot.count--;

                // Remove the slot if count drops to 0
                if (selectedSlot.count <= 0)
                {
                    selectedSlot.itemName = null;
                    selectedSlot.icon = null;
                }
            }

            Debug.Log($"Consumed {item.ItemName}, Hunger: {hungerData.Hunger}");
        }
        else
        {
            Debug.Log("Hunger is already full.");
        }
    }

    // Consume potion and heal health
    private void ConsumePotion(Item itemData, Inventory.Slot selectedSlot)
    {
        //if (playerHealth.Health < playerHealth.MaxHealth)
        {
            playerHealth.Health = Mathf.Min(99, playerHealth.Health + (int)itemData.HealingAmount);

            // Remove the potion item
            if (selectedSlot.count > 0)
            {
                selectedSlot.count--;

                // Remove the slot if count drops to 0
                if (selectedSlot.count <= 0)
                {
                    selectedSlot.itemName = null;
                    selectedSlot.icon = null;
                }
            }

            Debug.Log($"Consumed {itemData.ItemName}, Health: {playerHealth.Health}");
        }
    }
    */

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
