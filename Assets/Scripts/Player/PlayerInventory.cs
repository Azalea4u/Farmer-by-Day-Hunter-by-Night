using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    public Inventory_UI inventoryUI;

    [Header("Hotbar")]
    public int hotbar_SlotCount = 9;
    public Inventory hotbar;
    public HotBar_Data hotbarData;
    
    [Header("Inventory")]
    public int inventory_SlotCount = 18;
    public Inventory inventory;
    public HotBar_Data inventoryData;

    private void Awake()
    {
        // Creates the Hotbar inventory
        hotbar = new Inventory(hotbar_SlotCount);
        inventoryByName.Add("Hotbar", hotbar);
        // Creates the Inventory inventory
        inventory = new Inventory(inventory_SlotCount);
        inventoryByName.Add("Inventory", inventory);

        // Loads up the Data
        LoadInventoryData("Hotbar");
        LoadInventoryData("Inventory");
    }

    #region Inventory Data Management

    public void LoadInventoryData(string inventoryName)
    {
        if (!inventoryByName.ContainsKey(inventoryName)) return;

        var targetInventory = inventoryByName[inventoryName];
        var data = inventoryName == "Hotbar" ? hotbarData : inventoryData;

        for (int i = 0; i < data.slots.Count; i++)
        {
            if (i < targetInventory.slots.Count)
            {
                targetInventory.slots[i].itemName = data.slots[i].itemName;
                targetInventory.slots[i].count = data.slots[i].count;
                targetInventory.slots[i].icon = data.slots[i].icon;
            }
        }
    }

    public void SaveInventoryData(string inventoryName)
    {
        if (!inventoryByName.ContainsKey(inventoryName)) return;

        var targetInventory = inventoryByName[inventoryName];
        var data = inventoryName == "Hotbar" ? hotbarData : inventoryData;

        data.UpdateData(targetInventory.slots.Select(slot => new HotBar_Data.SlotData
        {
            itemName = slot.itemName,
            count = slot.count,
            icon = slot.icon
        }).ToList());
    }

    public void RefreshInventoryData(string inventoryName)
    {
        if (!inventoryByName.ContainsKey(inventoryName)) return;

        var targetInventory = inventoryByName[inventoryName];
        var data = inventoryName == "Hotbar" ? hotbarData : inventoryData;

        data.slots.Clear();

        foreach (var slot in targetInventory.slots)
        {
            data.slots.Add(new HotBar_Data.SlotData
            {
                itemName = slot.itemName,
                count = slot.count,
                icon = slot.icon
            });
        }
    }

    public void ClearInventoryData(string inventoryName)
    {
        if (!inventoryByName.ContainsKey(inventoryName)) return;

        var targetInventory = inventoryByName[inventoryName];
        var data = inventoryName == "Hotbar" ? hotbarData : inventoryData;

        if (data != null)
        {
            data.slots.Clear();
            Debug.Log($"{inventoryName} data has been cleared.");
        }
        else
        {
            Debug.LogWarning($"{inventoryName} data is not loaded and cannot be cleared.");
        }

        foreach (var slot in targetInventory.slots)
        {
            slot.itemName = null;
            slot.count = 0;
            slot.icon = null;
        }
    }

    #endregion

    // Call this in your Add and Remove methods
    public void Add(string inventoryName, Item item)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
            inventoryUI.Refresh();
            RefreshInventoryData(inventoryName);
        }
    }

    public void Remove(string inventoryName, int slotID, int quantity)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Remove(slotID, quantity);
            inventoryUI.Refresh();
            RefreshInventoryData(inventoryName);
        }
    }

    public Inventory GetInventoryByName(string inventoryName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }
}

