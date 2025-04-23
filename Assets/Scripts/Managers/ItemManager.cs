using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Item[] items;

    private Dictionary<string, Item> nameToItemDict =
        new Dictionary<string, Item>();

    private void Awake()
    {
        foreach (Item item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        if (!nameToItemDict.ContainsKey(item.data.ItemName))
        {
            nameToItemDict.Add(item.data.ItemName, item);
        }
    }

    public Item GetItemByName(string type)
    {
        if (nameToItemDict.ContainsKey(type))
        {
            return nameToItemDict[type];
        }
        else
        {
            return null;
        }
    }
}
