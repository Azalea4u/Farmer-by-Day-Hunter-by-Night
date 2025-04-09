using UnityEngine;
using UnityEngine.UI;

public class EnergyBar_UI : MonoBehaviour
{
    [SerializeField] private PlayerEnergy playerEnergy; // Reference to PlayerEnergy script
    [SerializeField] private Slider energyBar; // Reference to UI Slider

    private void Update()
    {
        if (energyBar != null && playerEnergy != null)
            //&&(!DialogueManager.instance.dialogueIsPlaying || !GameManager.instance.isGamePaused))
        {
            energyBar.value = playerEnergy.GetEnergyPercentage(); // Set slider value
        }
    }
}
