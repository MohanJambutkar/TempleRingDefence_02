using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public EnemyTypeData data;
    float health;

    void Awake()
    {
        health = data.maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}