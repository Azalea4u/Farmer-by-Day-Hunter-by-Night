using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    [SerializeField] public string ItemName = "Item Name";
    [SerializeField] public Sprite Icon;
    [SerializeField] public int BuyPrice;
    [SerializeField] public int SellPrice;
    [SerializeField] public bool IsFood;

    [SerializeField, ConditionalField("IsFood", true)] // Custom attribute to show based on IsFood
    public HealingType healingType;  // To store whether it heals health or hunger

    public enum HealingType
    {
        Health, Hunger
    }

    [SerializeField, ConditionalField("IsFood", true)] // Custom attribute to show based on IsFood
    public float HealingAmount;

    [SerializeField] public string Description = "Description of Item";

    // Adding a method to check healing type
    public string GetHealingType()
    {
        if (!IsFood)
        {
            return "Not a food item";
        }

        switch (healingType)
        {
            case HealingType.Health:
                return $"Health Healed: {HealingAmount}";  // Use HealingAmount to store healing currentGold for health
            case HealingType.Hunger:
                return $"Hunger Restored: {HealingAmount}";
            default:
                return "No healing effect";
        }
    }
}
