using UnityEngine;
using System.Collections.Generic;

public class PlanetEdgePlacementManager : MonoBehaviour
{
    [Header("References")]
    public Transform planet;
    public CircleCollider2D placementRing;
    public Camera mainCamera;
    public Transform towerParent;

    [Header("Placement Settings")]
    public float planetRadius = 3f;
    public float minAngleSpacing = 15f;

    [Header("Ghost Materials")]
    public Material ghostValidMat;
    public Material ghostInvalidMat;

    [Header("Towers")]
    public GameObject[] towerPrefabs;

    int selectedIndex = -1;

    GameObject ghostPrimary;
    GameObject ghostOpposite;
    float ghostBottomOffset;

    // 🔑 STORE ANGLES IN PLANET-LOCAL SPACE
    readonly List<float> occupiedAngles = new();

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
            return;
        }

        if (ghostPrimary == null || selectedIndex < 0)
            return;

        UpdateGhostPosition2D();

        if (Input.GetMouseButtonDown(0))
            TryPlaceTower();
    }

    // ================= UI =================
    public void SelectTower(int index)
    {
        if (index < 0 || index >= towerPrefabs.Length)
            return;

        CancelPlacement();
        selectedIndex = index;

        ghostPrimary = Instantiate(towerPrefabs[index]);
        ghostBottomOffset = CalculateBottomOffset(ghostPrimary);
        ApplyGhostMaterial(ghostPrimary, ghostValidMat);

        PairedTowerRule pairRule =
            ghostPrimary.GetComponent<PairedTowerRule>();

        if (pairRule && pairRule.requiresOppositePair)
        {
            if (!pairRule.CanPlacePair())
            {
                CancelPlacement();
                return;
            }

            ghostOpposite = Instantiate(towerPrefabs[index]);
            ApplyGhostMaterial(ghostOpposite, ghostValidMat);
        }
    }

    // ================= CORE =================
    void UpdateGhostPosition2D()
    {
        Vector2 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);

        if (!hit || hit.collider != placementRing)
            return;

        Vector2 dir = (hit.point - (Vector2)planet.position).normalized;

        float angle = GetPlanetLocalAngle(dir);
        bool valid = IsAngleFree(angle);

        // PRIMARY
        ghostPrimary.transform.position =
            planet.position + (Vector3)(dir * (planetRadius + ghostBottomOffset));

        ghostPrimary.transform.rotation =
            Quaternion.LookRotation(Vector3.forward, dir);

        // OPPOSITE
        if (ghostOpposite)
        {
            Vector2 oppDir = -dir;
            float oppAngle = angle + 180f;

            valid &= IsAngleFree(oppAngle);

            ghostOpposite.transform.position =
                planet.position + (Vector3)(oppDir * (planetRadius + ghostBottomOffset));

            ghostOpposite.transform.rotation =
                Quaternion.LookRotation(Vector3.forward, oppDir);
        }

        ApplyGhostMaterial(ghostPrimary, valid ? ghostValidMat : ghostInvalidMat);
        if (ghostOpposite)
            ApplyGhostMaterial(ghostOpposite, valid ? ghostValidMat : ghostInvalidMat);
    }

    void TryPlaceTower()
    {
        Vector2 dir =
            (ghostPrimary.transform.position - planet.position).normalized;

        float angle = GetPlanetLocalAngle(dir);

        if (!IsAngleFree(angle))
            return;

        PairedTowerRule pairRule =
            towerPrefabs[selectedIndex].GetComponent<PairedTowerRule>();

        if (pairRule && !pairRule.CanPlacePair())
            return;

        // ---- PRIMARY ----
        GameObject tower = Instantiate(
            towerPrefabs[selectedIndex],
            ghostPrimary.transform.position,
            ghostPrimary.transform.rotation,
            towerParent
        );

        occupiedAngles.Add(angle);
        tower.AddComponent<PlacedTowerAngle>()
             .Init(this, angle);

        // ---- OPPOSITE ----
        if (ghostOpposite && pairRule)
        {
            float oppAngle = angle + 180f;

            GameObject oppTower = Instantiate(
                towerPrefabs[selectedIndex],
                ghostOpposite.transform.position,
                ghostOpposite.transform.rotation,
                towerParent
            );

            occupiedAngles.Add(oppAngle);
            oppTower.AddComponent<PlacedTowerAngle>()
                    .Init(this, oppAngle);

            pairRule.RegisterPair(angle);
        }

        CancelPlacement();
    }

    // ================= ANGLE SYSTEM =================
    float GetPlanetLocalAngle(Vector2 dir)
    {
        float worldAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float planetRot = planet.eulerAngles.z;
        return Mathf.DeltaAngle(planetRot, worldAngle);
    }

    bool IsAngleFree(float angle)
    {
        foreach (float a in occupiedAngles)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(a, angle)) < minAngleSpacing)
                return false;
        }
        return true;
    }

    public void ReleaseAngle(float angle)
    {
        occupiedAngles.RemoveAll(
            a => Mathf.Abs(Mathf.DeltaAngle(a, angle)) < 0.1f
        );
    }

    // ================= HELPERS =================
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
        if (!obj) return;

        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
            r.material = mat;
    }
}