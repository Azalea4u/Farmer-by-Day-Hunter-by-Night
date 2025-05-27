using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    public AudioSource collect;

    // If the Player overlaps, then the item will be destoryed and added to the Player's inventory if inventory and hotbar is not full
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController)
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {   
                if (playerController.player.inventoryManager.GetInventoryByName("Hotbar").IsFull())
                {
                    if (playerController.player.inventoryManager.GetInventoryByName("Inventroy").IsFull())
                    {
                        Debug.Log("Your hotbar is full!");

                    }
                    else
                    {
                        playerController.player.inventoryManager.Add("Inventory", item);
                        CollectItem();
                    }
                }
                else
                {
                    playerController.player.inventoryManager.Add("Hotbar", item);
                    CollectItem();
                }
            }
        }
    }

    private void CollectItem()
    {
        //collect.Play();
        Destroy(this.gameObject);
    }
}