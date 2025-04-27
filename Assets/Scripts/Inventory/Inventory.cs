using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int count;
        public int MaxAllowed;

        public Sprite icon;

        public Slot()
        {
            itemName = "";
            count = 0;
            MaxAllowed = 99;
        }

        public bool IsEmpty
        {
            get
            {
                if (itemName == "" && count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CanAddItem(string itemName)
        {
            if (this.itemName == itemName && count < MaxAllowed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddItem(Item item)
        {
            this.itemName = item.data.ItemName;
            this.icon = item.data.Icon;
            count++;
        }

        public void AddItem(string itemName, Sprite icon, int maxAllowed)
        {
            this.itemName = itemName;
            this.icon = icon;
            count++;
            this.MaxAllowed = maxAllowed;
        }

        public void RemoveItem()
        {
            if (count > 0)
            {
                count--;

                if (count == 0)
                {
                    itemName = "";
                    icon = null;
                }
            }
        }
    }

    // ------------------------------------------------- INVENTORY BELOW --------------------------------------------------//

    public List<Slot> slots = new List<Slot>();
    public Slot selectedSlot = null;

    public Inventory(int numSlots)
    {
        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(Item item)
    {
        // Check if we already have the item in our inventoryManager
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.data.ItemName && slot.CanAddItem(item.data.ItemName))
            {
                slot.AddItem(item);
                return;
            }
        }

        // If we don't have the item in our inventoryManager/or is full
        // add it to the first empty slot
        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }

    public void Remove(int index, int numToRemove)
    {
        if (slots[index].count >= numToRemove)
        {
            for (int i = 0; i < numToRemove; i++)
            {
                Remove(index);
            }
        }
    }

    public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)
    {
        Slot fromSlot = slots[fromIndex];
        Slot toSlot = toInventory.slots[toIndex];

        if (toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemName))
        {
            for (int i = 0; i < numToMove; i++)
            {
                toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.MaxAllowed);
                fromSlot.RemoveItem(); 
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (slots != null && slots.Count > 0)
        {
            selectedSlot = slots[index];
        }
    }

    public bool IsFull()
    {
        // Check if we have any empty slots
        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty)
            {
                return false;
            }
        }
        return true;
    }
}
