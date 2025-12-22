using UnityEngine;
using System.Collections.Generic;

public class PairedTowerRule : MonoBehaviour
{
    [Header("Pair Rules")]
    public bool requiresOppositePair = true;
    public int maxPairs = 2;

    // Per tower-type tracking
    static Dictionary<string, int> pairCounts = new();

    string key;

    // 🔑 Ensure key & dictionary entry always exist
    void EnsureInitialized()
    {
        if (string.IsNullOrEmpty(key))
            key = gameObject.name.Replace("(Clone)", "").Trim();

        if (!pairCounts.ContainsKey(key))
            pairCounts[key] = 0;
    }

    public bool CanPlacePair()
    {
        EnsureInitialized();
        return pairCounts[key] < maxPairs;
    }

    public void RegisterPair(float angle)
    {
        EnsureInitialized();
        pairCounts[key]++;
    }

    public float GetOppositeAngle(float angle)
    {
        angle += 180f;
        if (angle >= 360f) angle -= 360f;
        return angle;
    }

    // Optional: call on level reset
    public static void ResetAll()
    {
        pairCounts.Clear();
    }
}