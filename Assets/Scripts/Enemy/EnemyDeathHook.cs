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
        // ================= EXISTING FEATURE (UNCHANGED) =================
        if (spawner)
            spawner.OnEnemyDestroyed();

        // ================= ADDITIVE FEATURE (SAFE) =================
        EnemyScoreValue scoreValue =
            GetComponent<EnemyScoreValue>();

        if (scoreValue != null && ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddEnemyKillScore(
                scoreValue.scoreOnKill
            );
        }
    }
}