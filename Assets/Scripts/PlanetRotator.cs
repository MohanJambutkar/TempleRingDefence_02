using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public PlanetTypeData data;

    void Update()
    {
        float input = Input.GetKey(KeyCode.Space) ? 1f : 0f;

        if (data)
        {
            transform.Rotate(
                Vector3.forward,
                input * data.rotationSpeed * Time.deltaTime
            );
        }
    }
}