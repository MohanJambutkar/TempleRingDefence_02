using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TowerSpawnLimiter : MonoBehaviour
{
    [System.Serializable]
    public class Limit
    {
        public GameObject towerPrefab;
        public int maxCount;
    }

    [Header("Tower Limits")]
    public List<Limit> limits = new List<Limit>();

    [Header("UI Feedback")]
    [Tooltip("Text shown when selected tower limit is full")]
    public GameObject limitReachedText;
    public float limitTextDuration = 2f;

    // limit → active count
    Dictionary<Limit, int> counts = new Dictionary<Limit, int>();

    Coroutine textRoutine;

    void Awake()
    {
        if (limitReachedText)
            limitReachedText.SetActive(false);
    }

    // ---------- PUBLIC API ----------

    public bool CanSpawn(GameObject prefab)
    {
        if (!prefab) return true;

        Limit limit = GetLimit(prefab);
        if (limit == null) return true;

        if (limit.maxCount <= 0) return true; // unlimited unless explicitly set

        counts.TryGetValue(limit, out int current);
        return current < limit.maxCount;
    }

    public void RegisterSpawn(GameObject instance)
    {
        if (!instance) return;

        Limit limit = GetLimitFromInstance(instance);
        if (limit == null) return;

        if (limit.maxCount <= 0) return; // unlimited, do not track

        if (!counts.ContainsKey(limit))
            counts[limit] = 0;

        counts[limit]++;

        TowerSpawnLimiterHook hook = instance.AddComponent<TowerSpawnLimiterHook>();
        hook.Init(this, limit);
    }


    // Query if a prefab has reached its spawn limit.
    public bool IsLimitFull(GameObject prefab)
    {
        if (!prefab) return false;

        Limit limit = GetLimit(prefab);
        if (limit == null) return false;

        if (limit.maxCount <= 0) return false; // unlimited

        counts.TryGetValue(limit, out int current);
        return current >= limit.maxCount;
    }

    public void UpdateLimitText(GameObject prefab)
    {
        if (!limitReachedText) return;

        if (IsLimitFull(prefab))
        {
            if (textRoutine != null)
                StopCoroutine(textRoutine);

            textRoutine = StartCoroutine(ShowLimitText());
        }
    }

    IEnumerator ShowLimitText()
    {
        limitReachedText.SetActive(true);
        yield return new WaitForSeconds(limitTextDuration);
        limitReachedText.SetActive(false);
        textRoutine = null;
    }

    // ---------- INTERNAL ----------

    public void Unregister(Limit limit)
    {
        if (limit == null) return;

        if (counts.ContainsKey(limit))
            counts[limit] = Mathf.Max(0, counts[limit] - 1);
    }

    Limit GetLimit(GameObject prefab)
    {
        foreach (var l in limits)
            if (l.towerPrefab == prefab)
                return l;
        return null;
    }

    Limit GetLimitFromInstance(GameObject instance)
    {
        foreach (var l in limits)
        {
            if (!l.towerPrefab) continue;

            // clone-safe match
            if (instance.name.StartsWith(l.towerPrefab.name))
                return l;
        }
        return null;
    }
}

public class TowerSpawnLimiterHook : MonoBehaviour
{
    TowerSpawnLimiter limiter;
    TowerSpawnLimiter.Limit limit;

    public void Init(TowerSpawnLimiter limiter, TowerSpawnLimiter.Limit limit)
    {
        this.limiter = limiter;
        this.limit = limit;
    }

    void OnDestroy()
    {
        if (limiter != null && limit != null)
            limiter.Unregister(limit);
    }
}