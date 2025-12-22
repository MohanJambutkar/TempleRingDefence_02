using UnityEngine;

[ExecuteAlways]
public class EnemyDirector : MonoBehaviour
{
    public EnemySpawner spawner;

    [Header("Global Control")]
    public bool enableSpawning = true;

    [Range(0.3f, 5f)]
    public float spawnInterval = 1.5f;

    [Range(1, 100)]
    public int maxAliveEnemies = 20;

    [Header("Presets")]
    public bool easy;
    public bool normal = true;
    public bool hard;

    void OnValidate()
    {
        if (!spawner) return;

        if (easy) ApplyEasy();
        if (normal) ApplyNormal();
        if (hard) ApplyHard();

        spawner.spawningEnabled = enableSpawning;
        spawner.spawnInterval = spawnInterval;
        spawner.maxAliveEnemies = maxAliveEnemies;
    }

    void ApplyEasy()
    {
        spawnInterval = 2.5f;
        maxAliveEnemies = 10;
        ResetFlags();
        easy = true;
    }

    void ApplyNormal()
    {
        spawnInterval = 1.5f;
        maxAliveEnemies = 20;
        ResetFlags();
        normal = true;
    }

    void ApplyHard()
    {
        spawnInterval = 0.7f;
        maxAliveEnemies = 40;
        ResetFlags();
        hard = true;
    }

    void ResetFlags()
    {
        easy = normal = hard = false;
    }
}