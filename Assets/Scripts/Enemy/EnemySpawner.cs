using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public Transform planet;
    public Transform enemyParent;
    public BoxCollider worldMovementBounds;   // REQUIRED

    [Header("Enemy Setup (ORDER MUST MATCH)")]
    public GameObject[] enemyPrefabs;
    public EnemyTypeData[] enemyTypes;

    [Header("Runtime Control (Inspector Driven)")]
    public bool spawningEnabled = true;
    public float spawnInterval = 1.5f;
    public int maxAliveEnemies = 10;

    float timer;
    int aliveCount;
    float lockedZ;

    void Awake()
    {
        if (worldMovementBounds == null)
        {
            Debug.LogError("EnemySpawner: WorldMovementBounds is NOT assigned.");
            enabled = false;
            return;
        }

        // 🔑 LOCK THE GAME PLANE
        lockedZ = worldMovementBounds.bounds.center.z;
    }

    void Update()
    {
        if (!spawningEnabled)
            return;

        if (enemyPrefabs == null || enemyTypes == null)
            return;

        if (enemyPrefabs.Length == 0 || enemyTypes.Length == 0)
            return;

        if (enemyPrefabs.Length != enemyTypes.Length)
        {
            Debug.LogError("EnemySpawner: enemyPrefabs and enemyTypes size mismatch.");
            return;
        }

        if (aliveCount >= maxAliveEnemies)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        int i = Random.Range(0, enemyPrefabs.Length);
        EnemyTypeData type = enemyTypes[i];

        Bounds b = worldMovementBounds.bounds;
        Vector3 spawnPos;

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: // Left
                spawnPos = new Vector3(b.min.x, Random.Range(b.min.y, b.max.y), lockedZ);
                break;
            case 1: // Right
                spawnPos = new Vector3(b.max.x, Random.Range(b.min.y, b.max.y), lockedZ);
                break;
            case 2: // Top
                spawnPos = new Vector3(Random.Range(b.min.x, b.max.x), b.max.y, lockedZ);
                break;
            default: // Bottom
                spawnPos = new Vector3(Random.Range(b.min.x, b.max.x), b.min.y, lockedZ);
                break;
        }

        GameObject enemy = Instantiate(
            enemyPrefabs[i],
            spawnPos,
            Quaternion.identity,
            enemyParent
        );

        // 🔒 FORCE Z LOCK (ABSOLUTE)
        Vector3 p = enemy.transform.position;
        enemy.transform.position = new Vector3(p.x, p.y, lockedZ);

        // Assign data
        EnemyBase baseComp = enemy.GetComponent<EnemyBase>();
        if (baseComp != null)
            baseComp.data = type;

        // Initialize movement
        EnemyMover mover = enemy.GetComponent<EnemyMover>();
        if (mover != null)
            mover.Init(planet, lockedZ);

        // Death hook
        EnemyDeathHook hook = enemy.GetComponent<EnemyDeathHook>();
        if (hook == null)
            hook = enemy.AddComponent<EnemyDeathHook>();

        hook.Init(this);

        aliveCount++;
    }

    public void OnEnemyDestroyed()
    {
        aliveCount = Mathf.Max(0, aliveCount - 1);
    }
}