using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;
using System;

public class ClickCountData
{
    public int button;
    public int count;
}

public class WSclient : MonoBehaviour
{
    WebSocket ws;

    public TextMeshProUGUI button1CountText;
    public TextMeshProUGUI button2CountText;

    private int button1Count;
    private int button2Count;

    private const float InitialReconnectDelay = 1.0f;
    private const float MaxReconnectDelay = 60.0f;
    private float currentReconnectDelay = InitialReconnectDelay;

    private const float PingInterval = 10.0f;
    private float timeSinceLastPing = 0.0f;

    private bool wasError = false; // Suggestion 1: Flag for error detection
    private bool attemptReconnect = true; // Suggestion 2: Flag for aggressive reconnection

    private void Start()
    {
        SetupWebSocket();

    }

    private void SetupWebSocket()
    {
        if (ws != null)
        {
            ws.Close();
            ws = null;
        }

        ws = new WebSocket("ws://commit-3e48a13ebc10.herokuapp.com/");
        //ws = new WebSocket("ws://localhost:3000");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket opened");
            currentReconnectDelay = InitialReconnectDelay;  // Reset the delay
            attemptReconnect = true; // Reset the reconnection attempt flag
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError("WebSocket Error: " + e.Message);
            wasError = true; // Set the error flag
            if (ws.ReadyState == WebSocketState.Closed || ws.ReadyState == WebSocketState.Closing)
            {
                StartCoroutine(ReconnectWithBackoff());
            }
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket closed with reason: " + e.Reason);
            if (!wasError && attemptReconnect) // Only reconnect if there wasn't an error and a reconnection attempt is allowed
            {
                StartCoroutine(ReconnectWithBackoff());
            }
            wasError = false; // Reset the error flag for the next disconnect event
            attemptReconnect = false; // Set the reconnection attempt flag to false
        };

        ws.OnMessage += WebSocket_OnMessage;

        ws.Connect();
    }

    IEnumerator ReconnectWithBackoff()
    {
        yield return new WaitForSeconds(currentReconnectDelay);
        currentReconnectDelay = Mathf.Min(currentReconnectDelay * 2, MaxReconnectDelay);
        SetupWebSocket();
        Debug.Log("reconncting with backoff");
    }

    private void Update()
    {
        if ((ws == null || ws.ReadyState == WebSocketState.Closed) && attemptReconnect) // Suggestion 2: Only attempt reconnect if allowed
        {
            SetupWebSocket();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Hello");
        }

        timeSinceLastPing += Time.deltaTime;
        if (timeSinceLastPing >= PingInterval)
        {
            timeSinceLastPing = 0.0f;
            if (ws.ReadyState == WebSocketState.Open)
            {
                ws.Ping();
            }
        }

        button1CountText.text = "Yes: " + button1Count.ToString();
        button1CountText.ForceMeshUpdate(true);
        button2CountText.text = "No: " + button2Count.ToString();
        button2CountText.ForceMeshUpdate(true);
    }

    private void WebSocket_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Received raw message: " + e.Data);
        string jsonString = e.Data;
        ClickCountData data = DeserializeJson(jsonString);
        if (data != null)
        {
            int button = data.button;
            int count = data.count;

            Debug.Log("Received click count for button " + button + ": " + count);

            if (button == 1)
            {
                button1Count = count;
            }
            else if (button == 2)
            {
                button2Count = count;
            }
        }
    }

    private ClickCountData DeserializeJson(string jsonString)
    {
        try
        {
            string[] keyValuePairs = jsonString.Trim('{', '}').Split(',');
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (string pair in keyValuePairs)
            {
                string[] keyValue = pair.Split(':');
                string key = keyValue[0].Trim('\"');
                string value = keyValue[1].Trim('\"');
                properties.Add(key, value);
            }

            if (properties.TryGetValue("button", out string buttonValue) && properties.TryGetValue("count", out string countValue))
            {
                if (int.TryParse(buttonValue, out int button) && int.TryParse(countValue, out int count))
                {
                    return new ClickCountData { button = button, count = count };
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error deserializing JSON: " + ex.Message);
            return null;
        }
    }
}


//public void HandleReload()
//{
//    if (ws != null && (ws.ReadyState == WebSocketState.Open || ws.ReadyState == WebSocketState.Connecting))
//    {
//        ws.Close();
//        Debug.Log("WebSocket closed due to page reload");
//    }
//}

//public void HandlePageVisibility()
//{
//    Debug.Log("Page is now visible. Attempting to reconnect...");
//    if (ws == null || ws.ReadyState == WebSocketState.Closed)
//    {
//        SetupWebSocket();
//    }
//}

