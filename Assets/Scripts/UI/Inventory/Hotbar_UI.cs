using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar_UI : MonoBehaviour
{
    [SerializeField] public List<Slot_UI> hotbar_Slots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    public void LoadHotBarFromData()
    {
        GameManager.instance.player.inventoryManager.LoadInventoryData("Hotbar");

        for (int i = 0; i < hotbar_Slots.Count; i++)
        {
            if (i < GameManager.instance.player.inventoryManager.hotbarData.slots.Count)
            {
                var slotData = GameManager.instance.player.inventoryManager.hotbarData.slots[i];
                if (!string.IsNullOrEmpty(slotData.itemName))
                {
                    Inventory.Slot slot = new Inventory.Slot
                    {
                        itemName = slotData.itemName,
                        count = slotData.count,
                        icon = slotData.icon
                    };
                    hotbar_Slots[i].SetItem(slot);
                }
                else
                {
                    hotbar_Slots[i].SetEmpty();
                }
            }
        }
    }

    private void Start()
    {
        LoadHotBarFromData();
        SelectSlot(0);
    }


    private void Update()
    {
        CheckAlphaNumericKeys();
    }

    public void SelectSlot(Slot_UI slot)
    {
        SelectSlot(slot.slotID);
    }

    public void SelectSlot(int index)
    {
        if (hotbar_Slots.Count == 9)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false);
            }
            selectedSlot = hotbar_Slots[index];
            selectedSlot.SetHighlight(true);

            GameManager.instance.player.inventoryManager.hotbar.SelectSlot(index);
        }
    }

    private void CheckAlphaNumericKeys()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }

        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }

        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }
    }
}
