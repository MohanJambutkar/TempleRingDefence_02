using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Planet")]
    public PlanetHealth planetHealth;
    public PlanetRotator planetRotator;

    [Header("Global Debug")]
    public bool immortalPlanet;
    public bool immortalTowers;

    [Header("DDA Scaling")]
    [Range(0.5f, 2f)] public float rotationSpeedMultiplier = 1f;
    [Range(0.5f, 2f)] public float enemyDamageMultiplier = 1f;

    void Update()
    {
        if (planetHealth)
            planetHealth.immortalPlanet = immortalPlanet;

        if (planetRotator && planetRotator.data)
            planetRotator.data.rotationSpeed *= rotationSpeedMultiplier;
    }

    public void ApplyTowerImmortality()
    {
        foreach (TowerHealth t in FindObjectsOfType<TowerHealth>())
            t.immortalTower = immortalTowers;
    }
}