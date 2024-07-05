using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float rotationSpeed = 100.0f; // Adjust rotation speed as needed

    void Update()
    {
        float xRotation = 0;
        float yRotation = 0;

        // Check for input to determine rotation direction
        if (Input.GetKey(KeyCode.W))
        {
            xRotation += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            xRotation -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            yRotation += 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            yRotation -= 1;
        }

        // Apply the rotation around the X and Y axes based on input
        transform.Rotate(Vector3.right, xRotation * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.up, yRotation * rotationSpeed * Time.deltaTime, Space.World);
    }
}