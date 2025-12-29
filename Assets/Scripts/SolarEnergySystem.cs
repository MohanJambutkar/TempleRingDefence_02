using UnityEngine;

public class SolarEnergySystem : MonoBehaviour
{
    [Header("Global Energy")]
    public float totalMaxEnergy = 1000f;

    [SerializeField] float currentEnergy;

    public float CurrentEnergy => currentEnergy;

    public void AddEnergy(float amount)
    {
        currentEnergy += amount;
        currentEnergy = Mathf.Min(currentEnergy, totalMaxEnergy);
    }

    [Range(0f, 1f)]
    [SerializeField] float charge01;

    void Update()
    {
        charge01 = totalMaxEnergy > 0
            ? currentEnergy / totalMaxEnergy
            : 0f;
    }
}