using UnityEngine;

public class PlanetEdgePlacementManager : MonoBehaviour
{
    [Header("References")]
    public Transform planet;
    public CircleCollider2D placementRing;
    public Camera mainCamera;
    public Transform towerParent;

    [Header("Optional Spawn Limiter")]
    public TowerSpawnLimiter spawnLimiter;

    [Header("Placement Settings")]
    public float planetRadius = 3f;
    public float minEdgeSpacing = 0.6f;

    [Header("Ghost Materials")]
    public Material ghostValidMat;
    public Material ghostInvalidMat;

    [Header("Towers")]
    public GameObject[] towerPrefabs;

    int selectedIndex = -1;
    GameObject ghostPrimary;
    GameObject ghostOpposite;
    float ghostBottomOffset;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
            return;
        }

        if (ghostPrimary == null || selectedIndex < 0)
            return;

        UpdateGhost();

        if (Input.GetMouseButtonDown(0))
            TryPlaceTower();
    }

    // ================= UI =================
    public void SelectTower(int index)
    {
        if (index < 0 || index >= towerPrefabs.Length)
            return;

        if (spawnLimiter != null && !spawnLimiter.CanSpawn(towerPrefabs[index]))
        {
            spawnLimiter.ShowLimitMessage();
            return;
        }


        CancelPlacement();
        selectedIndex = index;

        ghostPrimary = Instantiate(towerPrefabs[index]);
        ghostBottomOffset = CalculateBottomOffset(ghostPrimary);
        ApplyGhostMaterial(ghostPrimary, ghostValidMat);

        PairedTowerRule rule =
            towerPrefabs[index].GetComponent<PairedTowerRule>();

        if (rule && rule.requiresOppositePair)
        {
            if (!rule.CanPlacePair())
            {
                CancelPlacement();
                return;
            }

            ghostOpposite = Instantiate(towerPrefabs[index]);
            ApplyGhostMaterial(ghostOpposite, ghostValidMat);
        }
    }

    // ================= CORE =================
    void UpdateGhost()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (!hit || hit.collider != placementRing)
            return;

        Vector2 dir = (hit.point - (Vector2)planet.position).normalized;

        ghostPrimary.transform.position =
            planet.position + (Vector3)(dir * (planetRadius + ghostBottomOffset));
        ghostPrimary.transform.rotation =
            Quaternion.FromToRotation(Vector3.up, dir);

        bool valid = IsPositionFree(ghostPrimary.transform.position);

        if (ghostOpposite)
        {
            Vector2 oppDir = -dir;

            ghostOpposite.transform.position =
                planet.position + (Vector3)(oppDir * (planetRadius + ghostBottomOffset));
            ghostOpposite.transform.rotation =
                Quaternion.FromToRotation(Vector3.up, oppDir);

            valid &= IsPositionFree(ghostOpposite.transform.position);
        }

        ApplyGhostMaterial(ghostPrimary, valid ? ghostValidMat : ghostInvalidMat);
        if (ghostOpposite)
            ApplyGhostMaterial(ghostOpposite, valid ? ghostValidMat : ghostInvalidMat);
    }

    void TryPlaceTower()
    {
        if (spawnLimiter != null && !spawnLimiter.CanSpawn(towerPrefabs[selectedIndex]))
        {
            spawnLimiter.ShowLimitMessage();
            return;
        }

        if (!IsPositionFree(ghostPrimary.transform.position))
            return;

        GameObject primary = Instantiate(
            towerPrefabs[selectedIndex],
            ghostPrimary.transform.position,
            ghostPrimary.transform.rotation,
            towerParent
        );

        TowerEnergyCost primaryCost = primary.GetComponent<TowerEnergyCost>();
        if (primaryCost != null)
        {
            primaryCost.Consume();
        }

        if (spawnLimiter != null)
            spawnLimiter.RegisterSpawn(primary);

        MissileTower primaryMissile = primary.GetComponent<MissileTower>();
        if (primaryMissile != null)
            primaryMissile.Arm();

        PairedTowerRule primaryPair =
            primary.GetComponent<PairedTowerRule>();

        if (ghostOpposite && primaryPair)
        {
            GameObject opposite = Instantiate(
                towerPrefabs[selectedIndex],
                ghostOpposite.transform.position,
                ghostOpposite.transform.rotation,
                towerParent
            );

            if (spawnLimiter != null)
                spawnLimiter.RegisterSpawn(opposite);

            MissileTower oppositeMissile = opposite.GetComponent<MissileTower>();
            if (oppositeMissile != null)
                oppositeMissile.Arm();

            PairedTowerRule oppositePair =
                opposite.GetComponent<PairedTowerRule>();

            primaryPair.RegisterPair(oppositePair);
        }

        CancelPlacement();
    }

    // ================= HELPERS =================
    bool IsPositionFree(Vector3 pos)
    {
        foreach (Transform t in towerParent)
            if (Vector3.Distance(t.position, pos) < minEdgeSpacing)
                return false;
        return true;
    }

    float CalculateBottomOffset(GameObject obj)
    {
        Renderer r = obj.GetComponentInChildren<Renderer>();
        return r ? r.bounds.extents.y : 0f;
    }

    void CancelPlacement()
    {
        if (ghostPrimary) Destroy(ghostPrimary);
        if (ghostOpposite) Destroy(ghostOpposite);

        ghostPrimary = null;
        ghostOpposite = null;
        selectedIndex = -1;
    }

    void ApplyGhostMaterial(GameObject obj, Material mat)
    {
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
            r.material = mat;
    }
}