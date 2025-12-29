using UnityEngine;

public class SolarTowerEnergy : MonoBehaviour
{
    [Header("Data")]
    public SolarTowerData data;

    [Header("References")]
    public SolarEnergySystem energySystem;

    [Header("Runtime (Read Only)")]
    [SerializeField] bool receivingLight;

    [SerializeField] float contributionPerSecond;

    void Awake()
    {
        if (!energySystem)
            energySystem = FindObjectOfType<SolarEnergySystem>();

        contributionPerSecond = data != null
            ? data.energyPerSecond
            : 0f;
    }

    void Update()
    {
        if (!receivingLight || data == null || energySystem == null)
            return;

        energySystem.AddEnergy(
            data.energyPerSecond * Time.deltaTime
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SolarLight"))
            receivingLight = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SolarLight"))
            receivingLight = false;
    }
}