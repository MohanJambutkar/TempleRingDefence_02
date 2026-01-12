using UnityEngine;

[CreateAssetMenu(menuName = "Tower/Missile Tower Data")]
public class MissileTowerData : ScriptableObject
{
    [Header("Round Settings")]
    public int missilesPerRound = 5;
    public float timeBetweenRounds = 0.5f;

    [Header("Missile")]
    public float missileSpeed = 12f;
    public float missileDamage = 10f;

    [Header("Spread")]
    [Tooltip("Total spread angle in degrees")]
    public float spreadAngle = 40f;
}