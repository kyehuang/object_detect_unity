using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Use the intrinsic matrix and rotation matrix to convert the screen point to the world point
/// </summary>
public class ScreenToWorldConversion : MonoBehaviour
{
    public Camera camera;

    public ConnectRosBridge connectRos;
    public string CameraDataTopic = "/camera_data";

        
    /// <summary>
    /// Function to get the world point of the object
    /// </summary>
    /// <param name="ScreenPoint">The center of the object and distance 
    ///                            Vector3(center.x, center.y, distance)
    /// </param>
    /// 
    public void ScreenPointToWorldPoint(Vector3 ScreenPoint, string ObjectName)
    {                   
        // use the marco of unity to get the world point of the object
        Vector3 worldPoint = camera.ScreenToWorldPoint(ScreenPoint);
        Debug.Log("World point using Unity's method: " + worldPoint);

        // unity's Screen point is different from the opencv's screen point
        // so we need to convert the screen point to the opencv's screen point                
        ScreenPoint.y = Screen.height - ScreenPoint.y;
        Debug.Log("Screen point: " + ScreenPoint);
    
        CameraData cameraData = new CameraData
        {
            u = ScreenPoint.x,
            v = ScreenPoint.y,
            depth = ScreenPoint.z,
            objectName = ObjectName
        };
        
        string jsonData = JsonUtility.ToJson(cameraData);

        connectRos.PublishString(CameraDataTopic, jsonData);
    }    

    [System.Serializable]
    public class CameraData
    {
        public float u;
        public float v;
        public float depth;
        public string objectName;
    }
}
