using UnityEngine;
using UnityEngine.UI;

public class EnergyBar_UI : MonoBehaviour
{
    [SerializeField] private Slider energyBar;
    [SerializeField] private PlayerEnergy playerEnergy;

    public PlayerEnergy PlayerEnergy
    {
        get => playerEnergy;
        set
        {
            playerEnergy = value;
            UpdateEnergyBar(); // Automatically update the bar if set
        }
    }

    private void Update()
    {
        if (energyBar != null && playerEnergy != null)
        {
            UpdateEnergyBar();
        }
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null && playerEnergy != null)
        {
            energyBar.value = playerEnergy.GetEnergyPercentage();
        }
    }
}
