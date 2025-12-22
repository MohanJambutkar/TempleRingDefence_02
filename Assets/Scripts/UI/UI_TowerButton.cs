using UnityEngine;
using UnityEngine.UI;

public class UI_TowerButton : MonoBehaviour
{
    public int towerIndex;
    public PlanetEdgePlacementManager placementManager;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        placementManager.SelectTower(towerIndex);
    }
}