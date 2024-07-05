using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOutput : MonoBehaviour
{
    public ConnectRosBridge connectRos;
    private Camera camera;
    private Texture2D texture2D;
    private Rect rect; 
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        texture2D = new Texture2D(camera.pixelWidth, camera.pixelHeight, 
                                            TextureFormat.RGB24, false);
        rect = new Rect(0, 0, camera.pixelWidth, camera.pixelHeight);

        StartCoroutine(PublishImage());
    }

    private IEnumerator PublishImage()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
         
            RenderTexture renderTexture = new RenderTexture(
                                          camera.pixelWidth, camera.pixelHeight, 24);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(rect, 0, 0);
            texture2D.Apply();
            camera.targetTexture = null;
            RenderTexture.active = null;

            byte[] imagebytes = texture2D.EncodeToJPG();
            connectRos.PublishImage("/camera/image/compressed", imagebytes);
        }
    }
}
