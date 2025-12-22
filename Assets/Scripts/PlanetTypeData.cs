using UnityEngine;

[CreateAssetMenu(menuName = "Planet/Planet Type Data")]
public class PlanetTypeData : ScriptableObject
{
    [Header("Lives")]
    [Range(1, 20)]
    public int maxLives = 10;

    [Header("Per-Life Health")]
    public float minLifeHP = 500f;
    public float maxLifeHP = 800f;

    [Header("Rotation")]
    public float rotationSpeed = 120f;
}