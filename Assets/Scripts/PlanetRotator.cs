using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public PlanetTypeData data;

    void Update()
    {
        float input =
            Input.GetAxis("Horizontal") +
            (Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f) -
            (Input.GetKey(KeyCode.RightArrow) ? 1f : 0f);

        if (data)
        {
            transform.Rotate(
                Vector3.forward,
                -input * data.rotationSpeed * Time.deltaTime
            );
        }
    }
}