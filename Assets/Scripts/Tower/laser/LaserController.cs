using UnityEngine;
using System.Collections.Generic;

public class LaserController : MonoBehaviour
{
    public LaserTowerData data;
    public SolarEnergySystem energy;

    List<LaserTower> lasers = new();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RefreshLasers();
            UseLaser();
        }
    }

    void RefreshLasers()
    {
        lasers.Clear();
        lasers.AddRange(FindObjectsOfType<LaserTower>());
    }

    void UseLaser()
    {
        if (!data.freeUse)
        {
            if (energy.CurrentEnergy < data.energyRequired)
                return;

            energy.AddEnergy(-data.energyRequired);
        }

        foreach (var laser in lasers)
        {
            laser.Activate();
        }
    }
}