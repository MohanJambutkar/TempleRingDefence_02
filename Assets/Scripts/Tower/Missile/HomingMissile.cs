using UnityEngine;

public class StraightMissile : MonoBehaviour
{
    Vector3 direction;
    float speed;
    float damage;

    public void Init(Vector3 dir, float spd, float dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
    }

    void Update()
    {
        // Move straight forward
        transform.position += direction * speed * Time.deltaTime;

        // Keep missile facing movement direction (2.5D correct)
        transform.up = direction;
    }

    void OnTriggerEnter(Collider other)
    {
        // Only affect enemies
        if (!other.CompareTag("Enemy"))
            return;

        // Use EnemyBase (your existing system)
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}