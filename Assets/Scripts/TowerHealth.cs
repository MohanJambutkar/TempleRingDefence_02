using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHP = 100f;
    public bool immortalTower = false;

    float currentHP;

    PairedTowerRule pairedRule;

    void Awake()
    {
        currentHP = maxHP;
        pairedRule = GetComponent<PairedTowerRule>();
    }

    public void TakeDamage(float dmg)
    {
        if (immortalTower)
            return;

        currentHP -= dmg;

        if (currentHP <= 0f)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        // ✅ Paired tower → destroy whole pair
        if (pairedRule != null)
        {
            pairedRule.DestroyPair();
            return;
        }

        // ✅ Normal single tower
        Destroy(gameObject);
    }
}