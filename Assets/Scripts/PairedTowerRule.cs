using UnityEngine;
using System.Collections.Generic;

public class PairedTowerRule : MonoBehaviour
{
    [Header("Pair Rules")]
    public bool requiresOppositePair = true;
    public int maxPairs = 2;

    // ================= STATIC STATE =================
    // One counter PER PREFAB (Laser independent from Shield)
    static Dictionary<GameObject, int> activePairs = new();

    // ================= INSTANCE STATE =================
    PairedTowerRule partner;
    bool isOwner;
    bool isDestroying;

    GameObject prefabKey;

    void Awake()
    {
        prefabKey = ResolvePrefabKey();
        EnsureEntry();
    }

    // ================= PUBLIC API =================
    public bool CanPlacePair()
    {
        prefabKey = ResolvePrefabKey();
        EnsureEntry();
        return activePairs[prefabKey] < maxPairs;
    }

    /// <summary>
    /// Call ONCE on the PRIMARY tower after BOTH are spawned
    /// </summary>
    public void RegisterPair(PairedTowerRule other)
    {
        if (!other || partner != null)
            return;

        prefabKey = ResolvePrefabKey();
        EnsureEntry();

        if (activePairs[prefabKey] >= maxPairs)
            return;

        partner = other;
        other.partner = this;

        isOwner = true;
        activePairs[prefabKey]++;
    }

    // ================= DESTRUCTION =================
    public void DestroyPair()
    {
        if (isDestroying)
            return;

        isDestroying = true;

        prefabKey = ResolvePrefabKey();
        EnsureEntry();

        if (isOwner)
        {
            activePairs[prefabKey] =
                Mathf.Max(0, activePairs[prefabKey] - 1);
        }

        if (partner && !partner.isDestroying)
        {
            partner.isDestroying = true;
            Destroy(partner.gameObject);
        }

        Destroy(gameObject);
    }

    // ================= INTERNAL =================
    void EnsureEntry()
    {
        if (prefabKey == null)
            return;

        if (!activePairs.ContainsKey(prefabKey))
            activePairs[prefabKey] = 0;
    }

    GameObject ResolvePrefabKey()
    {
#if UNITY_EDITOR
        var prefab =
            UnityEditor.PrefabUtility.GetCorrespondingObjectFromSource(gameObject);

        return prefab != null ? prefab : gameObject;
#else
        return gameObject;
#endif
    }

    // ================= DEBUG =================
    public static void ResetAll()
    {
        activePairs.Clear();
    }
}