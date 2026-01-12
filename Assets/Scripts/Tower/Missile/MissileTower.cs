using UnityEngine;
using System.Collections;

public class MissileTower : MonoBehaviour
{
    [Header("References")]
    public MissileTowerData data;
    public Transform muzzle;
    public GameObject missilePrefab;

    bool firing;

    void Update()
    {
        if (!firing)
            StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        firing = true;

        FireRound();

        yield return new WaitForSeconds(data.timeBetweenRounds);

        firing = false;
    }

    void FireRound()
    {
        if (!data || !missilePrefab || !muzzle)
            return;

        int count = data.missilesPerRound;
        float spread = data.spreadAngle;

        for (int i = 0; i < count; i++)
        {
            float t = (count == 1) ? 0f : (float)i / (count - 1);
            float angle = Mathf.Lerp(-spread * 0.5f, spread * 0.5f, t);

            // 🔒 ALWAYS fire along tower local Y axis
            Vector3 fireDir =
                Quaternion.AngleAxis(angle, Vector3.forward) * muzzle.up;

            GameObject missile = Instantiate(
                missilePrefab,
                muzzle.position,
                Quaternion.identity
            );

            StraightMissile sm = missile.GetComponent<StraightMissile>();
            if (sm != null)
            {
                sm.Init(
                    fireDir,
                    data.missileSpeed,
                    data.missileDamage
                );
            }
        }
    }
}