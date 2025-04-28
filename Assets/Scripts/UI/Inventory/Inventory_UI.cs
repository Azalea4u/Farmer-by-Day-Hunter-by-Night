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
    //[SerializeField] GameObject inventoryPanel;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public bool dragSingle;

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

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(2) || Input.GetKey(KeyCode.LeftControl))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }
    }

    /*
    public void ToggleInventory()
    {
        // check if the inventory is active
        if (inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
        }
    }
    */

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)
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
        else
        {
            Debug.Log("Slots are not the same amount");
        }
    }

    public void Remove()
    {
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            inventory.slots[draggedSlot.slotID].itemName);

        if (itemToDrop != null)
        {
            if (dragSingle)
            {
                GameManager.instance.player.DropItem(itemToDrop);
                inventory.Remove(draggedSlot.slotID);
            }
            else
            {
                GameManager.instance.player.DropItem(itemToDrop, inventory.slots[draggedSlot.slotID].count);
                inventory.Remove(draggedSlot.slotID, inventory.slots[draggedSlot.slotID].count);
            }
        }

        Refresh();
        draggedSlot = null;
    }

    public void Slot_BeginDrag(Slot_UI slot)
    {
        draggedSlot = slot;
        draggedIcon = Instantiate(draggedSlot.itemIcon);
        draggedIcon.transform.SetParent(canvas.transform);
        draggedIcon.raycastTarget = false;
        draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50);

        MoveToMousePosition(draggedIcon.gameObject);
        Refresh();
    }

    public void Slot_Drag()
    {
        MoveToMousePosition(draggedIcon.gameObject);
        Refresh();
    }

    public void Slot_EndDrag()
    {
        Destroy(draggedIcon.gameObject);
        draggedSlot = null;
        Refresh();
    }

    public void Slot_Drop(Slot_UI slot)
    {
        if (slot.inventory != null)
        {
            if (dragSingle)
            {
                draggedSlot.inventory.MoveSlot(draggedSlot.slotID, slot.slotID, slot.inventory);
            }
            else
            {
                draggedSlot.inventory.MoveSlot(draggedSlot.slotID, slot.slotID, slot.inventory,
                    draggedSlot.inventory.slots[draggedSlot.slotID].count);
            }

            // Clear the original slot
            draggedSlot.SetEmpty();
        }
        Refresh();
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
