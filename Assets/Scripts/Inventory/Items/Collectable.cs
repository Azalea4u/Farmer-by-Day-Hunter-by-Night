using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    public AudioSource collect;

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
                    playerController.player.inventoryManager.Add("Inventory", item);
                    Debug.Log("Your hotbar is full!");
                }
                else
                {
                    playerController.player.inventoryManager.Add("Hotbar", item);
                }
                collect.Play();
                Destroy(this.gameObject);
            }
        }
    }
}