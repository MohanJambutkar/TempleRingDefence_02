using UnityEngine;
using System.Collections.Generic;

public class LaserController : MonoBehaviour
{
    [Header("Laser Control")]
    public LaserTowerData data;

    List<LaserTower> lasers = new();
    TowerEnergyUsage energyUsage;

    void Awake()
    {
        // Keep existing behavior

        // NEW: runtime energy usage handler (per use, with cooldown)
        energyUsage = GetComponent<TowerEnergyUsage>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUseLaser();
        }
    }

    void TryUseLaser()
    {
        RefreshLasers();

        if (lasers.Count == 0)
            return;

        // 🔋 ENERGY CHECK (does NOT modify energy)
        if (energyUsage != null)
        {
            if (!energyUsage.CanUse())
                return;
        }

        // 🔥 ACTIVATE ALL LASERS
        foreach (var laser in lasers)
        {
            laser.Activate();
        }

        // 🔋 ENERGY CONSUME (exactly once per Q press)
        if (energyUsage != null)
        {
            energyUsage.Consume();
        }
    }

    void RefreshLasers()
    {
        lasers.Clear();
        lasers.AddRange(FindObjectsOfType<LaserTower>());
    }
}