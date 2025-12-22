using UnityEngine;

public class EnemyDeathHook : MonoBehaviour
{
    EnemySpawner spawner;

    public void Init(EnemySpawner s)
    {
        spawner = s;
    }

    void OnDestroy()
    {
        if (spawner)
            spawner.OnEnemyDestroyed();
    }
}