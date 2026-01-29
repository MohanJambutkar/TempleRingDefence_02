using UnityEngine;

public class TowerEnergyCost : MonoBehaviour
{
    [Header("Energy Cost")]
    public float energyCost = 100f;

    [Header("Debug")]
    public bool freeUsageForTesting = false;

    SolarEnergySystem energySystem;
    bool consumed;

    void Awake()
    {
        energySystem = FindObjectOfType<SolarEnergySystem>();
    }

    /// <summary>
    /// Called during placement validation (ghost / preview).
    /// Must NOT modify energy.
    /// </summary>
    public bool CanAfford()
    {
        if (freeUsageForTesting)
            return true;

        if (energySystem == null)
            return false;

        return energySystem.CurrentEnergy >= energyCost;
    }

    /// <summary>
    /// Called exactly once AFTER the tower is successfully placed.
    /// </summary>
    public void Consume()
    {
        if (consumed || freeUsageForTesting)
            return;

        if (energySystem == null)
            return;

        energySystem.AddEnergy(-energyCost);
        consumed = true;
    }
}