using System.Collections;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    [SerializeField] private EnergyData energyData;
    [SerializeField] private float slowEnergyDuration = 2f; // Duration to slow energy after eating

    private float currentTickInterval;
    private float energyTimer;

    private void Start()
    {
        // Use the EnergySpeed from EnergyData
        currentTickInterval = energyData.EnergySpeed;
        energyTimer = 0f;
    }

    private void Update()
    {
        if (energyData.Energy > 0 && (!GameManager.instance.IsGamePaused)) //|| !DialogueManager.instance.dialogueIsPlaying))
        {
            energyTimer += Time.deltaTime;

            if (energyTimer >= currentTickInterval)
            {
                ReduceEnergy();
                energyTimer = 0f;
            }
        }
        else
        {
            if (energyData.Energy <= 0)
            {
                Debug.Log("Player is starving!");
            }

            // Handle starvation effects here, e.g., damage or reduced movement.
        }
    }

    private void ReduceEnergy()
    {
        energyData.Energy = Mathf.Max(0, energyData.Energy - 1);
        //Debug.Log($"Energy: {energyData.Energy}");
    }

    public void Eat(float foodValue)
    {
        energyData.Energy = Mathf.Min(100, energyData.Energy + foodValue);
        Debug.Log($"Player ate! Energy: {energyData.Energy}");
        StartCoroutine(SlowEnergyRate());
    }

    private IEnumerator SlowEnergyRate()
    {
        float originalInterval = currentTickInterval;
        currentTickInterval = energyData.EnergySpeed * 2; // Slow down energy rate temporarily

        yield return new WaitForSeconds(slowEnergyDuration);

        currentTickInterval = energyData.EnergySpeed; // Restore original rate
    }

    public float GetEnergyPercentage()
    {
        return energyData.Energy / 100f; // Normalize energy for UI
    }
}
