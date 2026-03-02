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
                currentLives = 0;
                currentHP = 0f;

                Debug.Log("PLANET DESTROYED");

                if (ScoreManager.Instance != null)
                    ScoreManager.Instance.StopScoring();

                if (GameStateManager.Instance != null)
                    GameStateManager.Instance.GameOver();
            }
            else
            {
                ResetLifeHP();
            }
        }
    }
}