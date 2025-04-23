using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    [SerializeField] public string ItemName = "Item Name";
    [SerializeField] public string Description = "Description of Item";
    [SerializeField] public Sprite Icon;
    [SerializeField] public int BuyPrice;
    [SerializeField] public int SellPrice;

    [SerializeField] public bool IsFood;

    [Header("Food"), ConditionalField("IsFood", true)]
    [SerializeField] public HealingTypes healingType;  // To store whether it heals health or energy

    public enum HealingTypes
    {
        Health, Energy
    }

    [SerializeField, ConditionalField("IsFood", true)] public float HealingAmount;
    [SerializeField, ConditionalField("IsFood", true)] public bool HasStatusEffect;

    [SerializeField, ConditionalField("HasStatusEffect", true)] public StatusTypes statusType;

    public enum StatusTypes
    {
        WalkFaster, MoreAttackDamage, RegenOverTime, TakeLessDamage, MoreKnockBack
    }

    // Adding a method to check healing type
    public string GetHealingType()
    {
        if (!IsFood)
        {
            return "Not a food item";
        }

        switch (healingType)
        {
            case HealingTypes.Health:
                return $"Health Healed: {HealingAmount}";
            case HealingTypes.Energy:
                return $"Energy Restored: {HealingAmount}";
            default:
                return "No healing effect";
        }
    }
}