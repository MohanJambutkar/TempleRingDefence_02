using UnityEngine;

public class EnemyPlanetDamage : MonoBehaviour
{
    EnemyBase enemy;
    bool hasHit = false;

    void Awake()
    {
        enemy = GetComponent<EnemyBase>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (enemy == null || enemy.data == null)
            return;

        bool didDamage = false;

        // 🛡️ Shield blocks everything
        ShieldHitReceiver shield = other.GetComponent<ShieldHitReceiver>();
        if (shield)
        {
            shield.TakeDamage(enemy.data.damage);
            didDamage = true;
        }
        else
        {
            // 🌍 Planet damage (only if no shield)
            PlanetHealth planet = other.GetComponent<PlanetHealth>();
            if (planet)
            {
                planet.TakeDamage(enemy.data.damage);
                didDamage = true;
            }

            // 🏗️ Tower damage
            TowerHealth tower = other.GetComponent<TowerHealth>();
            if (tower)
            {
                tower.TakeDamage(enemy.data.damage);
                didDamage = true;
            }
        }

        // 🔑 Consume enemy ONLY if something was damaged
        if (didDamage)
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}