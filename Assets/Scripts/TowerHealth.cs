using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float hp = 100f;
    public bool immortalTower = false;

    public void TakeDamage(float dmg)
    {
        if (immortalTower)
            return;

        hp -= dmg;

        if (hp <= 0f)
            Destroy(gameObject);
    }
}