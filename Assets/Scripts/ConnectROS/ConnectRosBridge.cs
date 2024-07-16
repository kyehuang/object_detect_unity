using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
public class ConnectRosBridge : MonoBehaviour
{
    public string rosbridgeServerAddress = "localhost:9090";
    public WebSocket ws;

    // Start is called before the first frame update
    void Start()
    {
        string protocol = "ws://";                
        ws = new WebSocket(protocol + rosbridgeServerAddress);
        ws.Connect();
    }

    private void OnDestroy()
    {
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    public void SubscribeToTopic(string topic, string typeMsg)
    {
        // String use             typeMsg = "std_msgs/msg/String"
        // Float32MultiArray use  typeMsg = "std_msgs/Float32MultiArray"
        
        string subscribeMessage = $@"{{
            ""op"": ""subscribe"",
            ""id"": ""1"",
            ""topic"": ""{topic}"",
            ""type"": ""{typeMsg}""
        }}";
        ws.Send(subscribeMessage);
    }

    public void PublishString(string topic, string message)
    {        
        string jsonMessage = $@"{{
            ""op"": ""publish"",
            ""topic"": ""{topic}"",
            ""msg"": {{
                ""data"": ""{message.Replace("\"", "\\\"")}""
            }}
        }}";

        ws.Send(jsonMessage);
    }

    public void PublishFloat32MultiArray(string topic, float[] data)
    {
        string jsonMessage = $@"{{
            ""op"": ""publish"",
            ""topic"": ""{topic}"",
            ""msg"": {{
                ""layout"": {{
                    ""dim"": [{{""label"": ""length"", ""size"": {data.Length}, ""stride"": {data.Length} }}],
                    ""data_offset"": 0
                }},
                ""data"": [{string.Join(", ", data)}]
            }}
        }}";

        
        ws.Send(jsonMessage);  
    }


}
