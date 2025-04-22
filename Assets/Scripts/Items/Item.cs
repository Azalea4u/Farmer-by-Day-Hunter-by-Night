using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    [SerializeField] public ItemData data;
    [HideInInspector] public Rigidbody2D rb2D;

    public string ItemName => data.ItemName;
    public string Description => data.Description;
    public int SellPrice => data.SellPrice;
    public int BuyPrice => data.BuyPrice;
    public bool IsFood => data.IsFood;
    public float HealingAmount => data.HealingAmount;
    public HealingType HealingType => data.healingType;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Method to display healing type based on IsFood
    public void DisplayHealingEffect()
    {
        if (IsFood)
        {
            Debug.Log(data.GetHealingType());
        }
        else
        {
            Debug.Log("This item does not heal.");
        }
    }
}
