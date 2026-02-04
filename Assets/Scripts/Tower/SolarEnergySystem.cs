using UnityEngine;

public class SolarEnergySystem : MonoBehaviour
{
    [Header("Global Energy")]
    [Min(0f)]
    public float totalMaxEnergy = 1000f;

    [SerializeField, Min(0f)]
    float currentEnergy;

    public float CurrentEnergy => currentEnergy;

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, totalMaxEnergy);
    }

    [Range(0f, 1f)]
    [SerializeField] float charge01;

    void Update()
    {
        // Safety clamp in case values are changed externally
        totalMaxEnergy = Mathf.Max(0f, totalMaxEnergy);
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, totalMaxEnergy);

        charge01 = totalMaxEnergy > 0f
            ? currentEnergy / totalMaxEnergy
            : 0f;
    }
}