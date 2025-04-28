using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    public AudioSource collect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();

        if (player)
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {   
                // WILL FIX ONCE INVENTORY IS FULLY ADDED
                /*
                if (player.inventoryManager.GetInventoryByName("Hotbar").IsFull())
                {
                    //player.inventoryManager.Add("Inventory", item);
                    Debug.Log("Your hotbar is full!");
                }
                else
                {
                    player.inventoryManager.Add("Hotbar", item);
                }
                */
                collect.Play();
                Destroy(this.gameObject);
            }
        }
    }
}