using UnityEngine;

[CreateAssetMenu(menuName = "Tower/Laser Tower Data")]
public class LaserTowerData : ScriptableObject
{
    [Header("Energy")]
    public float energyRequired = 200f;
    public bool freeUse = false;

    [Header("Laser")]
    public float duration = 6f;
    public float damagePerSecond = 30f;
    public float laserLength = 20f;
}