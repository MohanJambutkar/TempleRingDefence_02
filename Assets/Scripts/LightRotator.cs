using UnityEngine;

public class LightRotator : MonoBehaviour
{
    [Header("References")]
    public Transform planet;
    public Transform lightWedge;

    [Header("Rotation")]
    [Tooltip("Degrees per second")]
    public float rotationSpeed = 30f;

    void Update()
    {
        // 1️⃣ Rotate endlessly around planet center
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // 2️⃣ Always face planet center
        Vector3 dirToCenter = planet.position - lightWedge.position;

        float angle =
            Mathf.Atan2(dirToCenter.y, dirToCenter.x) * Mathf.Rad2Deg;

        lightWedge.rotation =
            Quaternion.Euler(0f, 0f, angle - 90f);
    }
}