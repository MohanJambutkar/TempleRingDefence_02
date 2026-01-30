using UnityEngine;

public class TowerEnergyUsage : MonoBehaviour
{
    [Header("Energy Usage")]
    public float energyCostPerUse = 50f;

    [Header("Cooldown")]
    public float cooldown = 1f;

    [Header("Debug")]
    public bool freeUseForTesting = false;

    SolarEnergySystem energySystem;
    float lastUseTime;

    void Awake()
    {
        energySystem = FindObjectOfType<SolarEnergySystem>();
    }

    public bool CanUse()
    {
        if (freeUseForTesting)
            return true;

        if (energySystem == null)
            return false;

        if (Time.time < lastUseTime + cooldown)
            return false;

        return energySystem.CurrentEnergy >= energyCostPerUse;
    }

    public void Consume()
    {
        if (freeUseForTesting)
            return;

        if (energySystem == null)
            return;

        energySystem.AddEnergy(-energyCostPerUse);
        lastUseTime = Time.time;
    }
}