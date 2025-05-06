using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public GameObject inventoryPanel;

    public List<Inventory_UI> inventroyUIs;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;

    public static bool dragSingle;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        foreach (Inventory_UI ui in inventroyUIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }

    private void Update()
    {
        // Tab to Oopen/Close Inventory UI
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory_UI();
        }

        // LeftShift if you want to move only 1 item of a stack
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftControl))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }
    }

    public void ToggleInventory_UI()
    {
        // check if the inventory is active
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
            RefreshInventoryUI("Inventory");
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }

    // Refreshes all of the inventories
    public void RefreshAll()
    {
        foreach (KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        Debug.LogWarning("There is no inventory ui for " +  inventoryName);
        return null;
    }
}
