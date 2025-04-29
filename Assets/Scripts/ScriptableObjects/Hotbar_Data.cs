using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HotBar_Data", menuName = "ScriptableObjects/HotBar_Data")]
public class HotBar_Data : ScriptableObject
{
    [System.Serializable]
    public class SlotData
    {
        public string itemName;
        public int count;
        public Sprite icon;
    }

    [SerializeField] public List<SlotData> slots = new List<SlotData>();

    public void UpdateData(List<SlotData> newData)
    {
        slots.Clear();
        slots.AddRange(newData);
    }
}
