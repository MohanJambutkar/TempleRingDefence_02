using UnityEngine;

[CreateAssetMenu(menuName = "Tower/Shield Tower Data")]
public class ShieldTowerData : ScriptableObject
{
    [Header("Shield Stats")]
    public float maxShieldHP = 200f;

    [Tooltip("If enabled, shield will not take damage (testing mode)")]
    public bool invincibleForTest = false;

    [Header("Visual")]
    public float shieldRadius = 6.5f;
}