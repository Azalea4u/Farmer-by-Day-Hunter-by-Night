using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class Hotbar_UI : MonoBehaviour
{
    [SerializeField] public List<Slot_UI> hotbar_Slots = new List<Slot_UI>();

    private Slot_UI selectedSlot;

    private void Start()
    {
        StartCoroutine(WaitForLocalPlayerAndInitialize());
    }

    private IEnumerator WaitForLocalPlayerAndInitialize()
    {
        // Wait until the GameManager and local player are assigned
        while (GameManager.instance == null || GameManager.instance.player == null)
        {
            yield return null;
        }

        // Extra safety wait
        yield return new WaitForEndOfFrame();

        // Only initialize if *this* is the local player's hotbar
        if (GameManager.instance.player.IsOwner)
        {
            InitializeHotbar();
        }
    }


    // Loads the data from the Hotbar
    public void LoadHotBarFromData()
    {
        GameManager.instance.player.playerInventory.LoadInventoryData("Hotbar");

        for (int i = 0; i < hotbar_Slots.Count; i++)
        {
            if (i < GameManager.instance.player.playerInventory.hotbarData.slots.Count)
            {
                var slotData = GameManager.instance.player.playerInventory.hotbarData.slots[i];
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

    public void InitializeHotbar()
    {
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            LoadHotBarFromData();
            SelectSlot(0);
        }
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

            GameManager.instance.player.playerInventory.hotbar.SelectSlot(index);
        }
    }

    // Press keys 1-9 and it will select that slot
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
