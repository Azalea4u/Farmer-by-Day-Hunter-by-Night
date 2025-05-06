using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public string inventoryName;
    [SerializeField] List<Inventory_UI> inventoryUIs;
    [SerializeField] List<Slot_UI> slots = new List<Slot_UI>();
    [SerializeField] private Canvas canvas;

    private Inventory inventory;

    private void Awake()
    {
        Initialize();
        canvas = FindObjectOfType<Canvas>();
    }

    private void Start()
    {
        inventory = GameManager.instance.player.inventoryManager.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }

    // Refreshes the UI after the Hotbar_Data changes
    public void Refresh()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (inventory.slots[i].itemName != "")
            {
                slots[i].SetItem(inventory.slots[i]);
            }
            else
            {
                slots[i].SetEmpty();

            }
        }
    }

    // For when the item is dropped outside the Inventory
    public void Remove()
    {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            inventory.slots[UI_Manager.draggedSlot.slotID].itemName);

        if (itemToDrop != null)
        {
            if (UI_Manager.dragSingle)
            {
                GameManager.instance.player.DropItem(itemToDrop);
                inventory.Remove(UI_Manager.draggedSlot.slotID);
            }
            else
            {
                GameManager.instance.player.DropItem(itemToDrop, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                inventory.Remove(UI_Manager.draggedSlot.slotID, inventory.slots[UI_Manager.draggedSlot.slotID].count);
            }
        }

        Refresh();
        UI_Manager.draggedSlot = null;
    }

    // After you click on the item Slot
    public void Slot_BeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;
        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon);
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform);
        UI_Manager.draggedIcon.raycastTarget = false;
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        Refresh();
    }

    // When Mouse button is still down, you can move the item around
    public void Slot_Drag()
    {
        MoveToMousePosition(UI_Manager.draggedIcon.gameObject);
        Refresh();
    }

    // When you let go of the mouse button
    public void Slot_EndDrag()
    {
        Destroy(UI_Manager.draggedIcon.gameObject);
        UI_Manager.draggedSlot = null;

        Refresh();
    }

    // The item will be dropped onto the new slot
    public void Slot_Drop(Slot_UI slot)
    {
        if (slot.inventory != null)
        {
            if (UI_Manager.dragSingle)
            {
                UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
            }
            else
            {
                UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                    UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
            }

            // Clear the original slot
            UI_Manager.draggedSlot.SetEmpty();
        }
        //Refresh();
        GameManager.instance.uiManager.RefreshAll();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 position;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);

            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    private void SetupSlots()
    {
        int counter = 0;

        foreach (Slot_UI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory;
        }
    }

    private void Initialize()
    {
        foreach (Inventory_UI ui in inventoryUIs)
        {
            if (inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
