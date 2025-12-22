using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Type Data")]
public class EnemyTypeData : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;

    [Header("Stats")]
    public float maxHealth = 10f;
    public float moveSpeed = 2f;
    public float damage = 1f;

    [Header("Spawning")]
    public float spawnRadius = 6f;

    [Header("Behavior Flags")]
    public bool isFlying;
    public bool placesCorruption;
}