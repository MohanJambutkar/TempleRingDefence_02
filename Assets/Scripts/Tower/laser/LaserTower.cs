using UnityEngine;
using System.Collections;

public class LaserTower : MonoBehaviour
{
    public LaserTowerData data;
    public LineRenderer beam;
    public Transform muzzle;

    bool active;

    void Awake()
    {
        beam.enabled = false;
    }

    public void Activate()
    {
        if (!active)
            StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        active = true;
        beam.enabled = true;

        float timer = 0f;

        while (timer < data.duration)
        {
            UpdateBeam();
            DamageEnemies();
            timer += Time.deltaTime;
            yield return null;
        }

        beam.enabled = false;
        active = false;
    }

    void UpdateBeam()
    {
        Vector3 start = muzzle.position;
        Vector3 dir = muzzle.up; // 🔒 ALWAYS outward from tower
        Vector3 end = start + dir * data.laserLength;

        beam.SetPosition(0, start);
        beam.SetPosition(1, end);
    }

    void DamageEnemies()
    {
        Ray ray = new Ray(muzzle.position, muzzle.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, data.laserLength);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.SendMessage(
                    "TakeDamage",
                    data.damagePerSecond * Time.deltaTime,
                    SendMessageOptions.DontRequireReceiver
                );
            }
        }
    }
}