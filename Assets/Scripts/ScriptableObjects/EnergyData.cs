using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnergyData", menuName = "ScriptableObjects/EnergyData")]
public class EnergyData : ScriptableObject
{
    [SerializeField, Range(0, 100)] public float Energy;
    [SerializeField] public float EnergySpeed;
    [SerializeField] public bool JustAte;
}
