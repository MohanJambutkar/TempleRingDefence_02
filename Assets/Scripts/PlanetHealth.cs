using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    public PlanetTypeData data;

    [Header("Runtime")]
    public int currentLives;
    public float currentHP;

    [Header("Debug")]
    public bool immortalPlanet = false;

    void Awake()
    {
        if (!data)
        {
            Debug.LogError("PlanetHealth: No PlanetTypeData assigned.");
            enabled = false;
            return;
        }

        currentLives = data.maxLives;
        ResetLifeHP();
    }

    void ResetLifeHP()
    {
        currentHP = Random.Range(data.minLifeHP, data.maxLifeHP);
    }

    public void TakeDamage(float dmg)
    {
        if (immortalPlanet)
            return;

        currentHP -= dmg;

        if (currentHP <= 0f)
        {
            currentLives--;

            if (currentLives <= 0)
            {
                Debug.Log("PLANET DESTROYED");
                // later: GameOver
            }
            else
            {
                ResetLifeHP();
            }
        }
    }
}