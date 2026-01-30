using UnityEngine;

public class SolarTowerEnergy : MonoBehaviour
{
    [Header("Data")]
    public SolarTowerData data;

    [Header("References")]
    public SolarEnergySystem energySystem;

    [Header("Production Control")]
    [Tooltip("Energy produced per second by tower")]
    [SerializeField] float energyProductionPerSecond = 10f;

    [Tooltip("Additional bonus added PER active solar tower (stacking)")]
    [SerializeField] float bonusPerTower = 0f;

    [Header("Runtime (Read Only)")]
    [SerializeField] bool receivingLight;

    [SerializeField] float finalEnergyPerSecond;

    static int activeSolarTowers;

    void Awake()
    {
        if (!energySystem)
            energySystem = FindObjectOfType<SolarEnergySystem>();

        activeSolarTowers++;
        RecalculateProduction();
    }

    void OnDestroy()
    {
        activeSolarTowers = Mathf.Max(0, activeSolarTowers - 1);
    }

    void Update()
    {
        if (!receivingLight || data == null || energySystem == null)
            return;

        energySystem.AddEnergy(
            finalEnergyPerSecond * Time.deltaTime
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SolarLight"))
        {
            receivingLight = true;
            RecalculateProduction();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SolarLight"))
            receivingLight = false;
    }

    void RecalculateProduction()
    {
        if (data == null)
        {
            finalEnergyPerSecond = 0f;
            return;
        }

        finalEnergyPerSecond =
            energyProductionPerSecond +
            (activeSolarTowers * bonusPerTower);
    }
}