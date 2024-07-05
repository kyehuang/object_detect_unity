using System.Collections;
using UnityEngine;

public class ChessCapture : MonoBehaviour
{
    public Camera camera; // Assign the cameras you want to capture in the inspector
    public int cameraIndex = 0;
    public int captureWidth = 640; // Width of the capture
    public int captureHeight = 480; // Height of the capture
    private float captureInterval = 1f / 30f;

    private void Start()
    {        
        StartCoroutine(CaptureAtInterval());        
        // CaptureScreenshot(camera);
    }
    
    private IEnumerator CaptureAtInterval()
    {
        // while (true)
        while (true)
        {
            yield return new WaitForSeconds(captureInterval); // Wait for the interval duration

            // Capture the screenshots at the same time
            StartCoroutine(CaptureScreenshot(camera));
        }
    }

    private IEnumerator CaptureScreenshot(Camera camera)
    {
        Debug.Log("CaptureScreenshot");
        string cameraFolder = Application.dataPath + "/Captures/Camera_" + cameraIndex;
        System.IO.Directory.CreateDirectory(cameraFolder);
        
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        camera.targetTexture = rt;
        yield return new WaitForEndOfFrame(); // Ensure the frame is rendered

        Texture2D screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        // string filename = $"{cameraFolder}/CameraCapture_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.png";
        string filename =  $"{cameraFolder}/CameraCapture_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.jpg";
        System.IO.File.WriteAllBytes(filename, bytes);
        // Debug.Log($"Saved Camera {cameraIndex} Capture to {filename}");
    }
}