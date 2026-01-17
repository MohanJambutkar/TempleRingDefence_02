using UnityEngine;

public class ShieldTower : MonoBehaviour
{
    public Transform shieldAnchor;

    void OnEnable()
    {
        ShieldController.RegisterTower(this);
    }

    void OnDisable()
    {
        ShieldController.UnregisterTower(this);
    }
}