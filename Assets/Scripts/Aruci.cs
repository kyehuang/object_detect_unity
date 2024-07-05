using UnityEngine;

public class CameraMapper : MonoBehaviour
{
    public Camera targetCamera; // Assign your Unity Camera here
    public Vector3 realWorldPoint = new Vector3(0, 0, 5); // Example 3D point

    private void Start()
    {
        if (targetCamera != null)
        {
            Vector2 screenPosition = MapRealWorldPointToScreen(realWorldPoint);
            Debug.Log($"Mapped Screen Position: {screenPosition}");
        }
        else
        {
            Debug.LogError("Target camera not assigned.");
        }
    }

    Vector2 MapRealWorldPointToScreen(Vector3 point)
    {
        // Extrinsic Matrix from camera's world to camera's local space
        Matrix4x4 extrinsicMatrix = targetCamera.transform.worldToLocalMatrix;

        // Intrinsic Matrix approximation
        float focalLength = targetCamera.nearClipPlane; // Simplification for demonstration
        float aspectRatio = Screen.width / (float)Screen.height;
        float fov = targetCamera.fieldOfView * Mathf.Deg2Rad;
        float height = 2 * Mathf.Tan(fov / 2) * focalLength; // Physical height of the sensor
        float width = height * aspectRatio; // Physical width of the sensor
        Matrix4x4 intrinsicMatrix = Matrix4x4.identity;
        intrinsicMatrix[0, 0] = 2 * focalLength / width; // Scale the x coordinates
        intrinsicMatrix[1, 1] = 2 * focalLength / height; // Scale the y coordinates
        intrinsicMatrix[2, 2] = 1; // Keep z coordinates as is

        // Convert the real-world point to camera coordinates
        Vector3 cameraCoordinates = extrinsicMatrix.MultiplyPoint3x4(point);

        // Project the camera coordinates to image coordinates
        Vector3 imageCoordinates = intrinsicMatrix.MultiplyPoint3x4(cameraCoordinates);

        // Normalize and map to screen space
        Vector2 screenPosition = new Vector2((imageCoordinates.x + 1) * 0.5f * Screen.width, (imageCoordinates.y + 1) * 0.5f * Screen.height);

        return screenPosition;
    }
    
    // Vector2 Map(Vector3 point)
    // {
    //     return new Vector2;
    // }
}