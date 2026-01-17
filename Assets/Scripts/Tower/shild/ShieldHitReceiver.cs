using UnityEngine;

public class ShieldHitReceiver : MonoBehaviour
{
    ShieldController controller;

    // ✅ Called by ShieldController
    public void Init(ShieldController controller)
    {
        this.controller = controller;
    }

    public void TakeDamage(float damage)
    {
        if (controller != null)
        {
            controller.TakeDamage(damage);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Enemy touches shield → enemy dies, shield takes damage
        if (!other.CompareTag("Enemy"))
            return;

        if (controller != null)
        {
            // You can later replace 10f with enemy damage
            controller.TakeDamage(10f);
        }

        Destroy(other.gameObject);
    }
}