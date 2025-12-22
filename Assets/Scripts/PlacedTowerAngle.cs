using UnityEngine;

public class PlacedTowerAngle : MonoBehaviour
{
    PlanetEdgePlacementManager manager;
    float angle;

    public void Init(PlanetEdgePlacementManager manager, float angle)
    {
        this.manager = manager;
        this.angle = angle;
    }

    void OnDestroy()
    {
        if (manager != null)
            manager.ReleaseAngle(angle);
    }
}