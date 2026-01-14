using UnityEngine;
using System.Collections;

public class MissileTower : MonoBehaviour
{
    [Header("References")]
    public MissileTowerData data;
    public Transform muzzle;
    public GameObject missilePrefab;

    [Header("State (Runtime)")]
    [SerializeField] bool isArmed = false;
    bool firing;

    void Awake()
    {
        // 🔒 CRITICAL: Never fire by default
        isArmed = false;
        firing = false;
    }

    /// <summary>
    /// MUST be called once AFTER tower is placed on planet edge
    /// </summary>
    public void Arm()
    {
        isArmed = true;
    }

    void Update()
    {
        // ❌ ABSOLUTE BLOCK
        if (!isArmed)
            return;

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
            float t = (count == 1) ? 0.5f : (float)i / (count - 1);
            float angle = Mathf.Lerp(-spread * 0.5f, spread * 0.5f, t);

            // ✅ Always fire along tower LOCAL Y axis
            Vector3 fireDir =
                Quaternion.AngleAxis(angle, Vector3.forward) * muzzle.up;

            GameObject missile = Instantiate(
                missilePrefab,
                muzzle.position,
                Quaternion.LookRotation(Vector3.forward, fireDir)
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