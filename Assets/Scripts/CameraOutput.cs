using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraOutput : MonoBehaviour
{
    public ConnectRosBridge connectRos;
    public string CameraTopic = "/camera/image/compressed";
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

        AdvertiseTopic();
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
            PublishImage(CameraTopic, imagebytes, "camera");
        }
    }
    void AdvertiseTopic()
    {
        // Prepare the advertise message JSON
        string advertiseMessage = $@"{{
            ""op"": ""advertise"",
            ""topic"": ""{CameraTopic}"",
            ""type"": ""sensor_msgs/msg/CompressedImage""
        }}";

        // Send the advertise message
        connectRos.ws.Send(advertiseMessage);
    }
    public void PublishImage(string topic, byte[] imagebytes, string frame_id = "camera")
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        string jsonMessage = $@"{{
            ""op"": ""publish"",
            ""topic"": ""{topic}"",
            ""msg"": {{
                ""header"": {{
                    ""stamp"": {{
                        ""secs"": {timestamp / 1000},
                        ""nsecs"": {(timestamp % 1000) * 1000000}
                    }},
                    ""frame_id"": ""{frame_id}""
                }},
                ""format"": ""jpeg"",
                ""data"": ""{System.Convert.ToBase64String(imagebytes)}""
            }}
        }}";

        connectRos.ws.Send(jsonMessage);
    }
}
