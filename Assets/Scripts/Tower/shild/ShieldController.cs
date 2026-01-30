using UnityEngine;
using System.Collections.Generic;

public class ShieldController : MonoBehaviour
{
    [Header("Data")]
    public ShieldTowerData data;

    [Header("References")]
    public GameObject shieldVisualPrefab;
    public Transform planetCenter;

    [Header("Runtime (Inspector Only)")]
    [SerializeField] float currentHP;

    [Range(0f, 1f)]
    [SerializeField] float hp01;   // Inspector bar

    static List<ShieldTower> towers = new();

    GameObject activeShield;
    bool shieldActive;

    TowerEnergyUsage energyUsage;

    /* ---------------- STATIC REGISTRATION ---------------- */

    public static void RegisterTower(ShieldTower tower)
    {
        if (!towers.Contains(tower))
            towers.Add(tower);
    }

    public static void UnregisterTower(ShieldTower tower)
    {
        towers.Remove(tower);
    }

    void Awake()
    {
        // Auto‑resolve planet center if not assigned
        if (!planetCenter)
        {
            GameObject planet = GameObject.FindGameObjectWithTag("Planet");
            if (planet)
                planetCenter = planet.transform;
        }

        energyUsage = GetComponent<TowerEnergyUsage>();
        if (!energyUsage)
            energyUsage = GetComponentInParent<TowerEnergyUsage>();
    }

    /* ---------------- INPUT ---------------- */

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryActivateShield();
        }

        // Update inspector HP bar
        hp01 = (data && data.maxShieldHP > 0f)
            ? currentHP / data.maxShieldHP
            : 0f;
    }

    /* ---------------- SHIELD LOGIC ---------------- */

    void TryActivateShield()
    {
        if (shieldActive)
            return;

        if (towers.Count == 0)
            return;

        if (!data || !shieldVisualPrefab || !planetCenter)
            return;

        if (energyUsage != null && !energyUsage.CanUse())
            return;

        ActivateShield();
    }

    void ActivateShield()
    {
        shieldActive = true;

        if (energyUsage != null)
            energyUsage.Consume();

        currentHP = data.maxShieldHP;

        activeShield = Instantiate(
            shieldVisualPrefab,
            planetCenter.position,
            Quaternion.identity
        );

        activeShield.transform.localScale =
            Vector3.one * data.shieldRadius;

        // Ensure ShieldHitReceiver exists and is wired
        ShieldHitReceiver receiver =
            activeShield.GetComponent<ShieldHitReceiver>();

        if (!receiver)
            receiver = activeShield.AddComponent<ShieldHitReceiver>();

        receiver.Init(this);
    }

    public void TakeDamage(float amount)
    {
        if (!shieldActive)
            return;

        if (data && data.invincibleForTest)
            return;

        currentHP -= amount;
        currentHP = Mathf.Max(0f, currentHP);

        if (currentHP <= 0f)
        {
            DeactivateShield();
        }
    }

    void DeactivateShield()
    {
        shieldActive = false;
        currentHP = 0f;

        if (activeShield)
            Destroy(activeShield);
    }
}