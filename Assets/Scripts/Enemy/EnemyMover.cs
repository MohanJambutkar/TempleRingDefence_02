using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    Transform planet;
    float speed;
    float lockedZ;

    public void Init(Transform planetTarget, float z)
    {
        planet = planetTarget;
        lockedZ = z;
        speed = GetComponent<EnemyBase>().data.moveSpeed;
    }

    void Update()
    {
        if (!planet) return;

        Vector3 target = planet.position;
        target.z = lockedZ;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        // 🔒 HARD Z LOCK
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, p.y, lockedZ);
    }
}