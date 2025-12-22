using UnityEngine;

public class WorldMovementBounds : MonoBehaviour
{
    public static WorldMovementBounds Instance;

    Bounds worldBounds;

    void Awake()
    {
        Instance = this;

        // Use renderer bounds if available, otherwise collider
        if (TryGetComponent(out Renderer r))
            worldBounds = r.bounds;
        else if (TryGetComponent(out Collider c))
            worldBounds = c.bounds;
    }

    public Vector3 Clamp(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, worldBounds.min.x, worldBounds.max.x),
            Mathf.Clamp(position.y, worldBounds.min.y, worldBounds.max.y),
            position.z // keep Z untouched (2.5D)
        );
    }
}