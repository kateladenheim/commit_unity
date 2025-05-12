using UnityEngine;
using TMPro;
using WebSocketSharp;

public class WebSocketReceiver : MonoBehaviour
{
    public TextMeshProUGUI textMesh1;
    public TextMeshProUGUI textMesh2;

    private WebSocket ws;
    private const string serverAddress = "ws://commit-3e48a13ebc10.herokuapp.com/"; // replace with your server address

    private float newCounter1;
    private float newCounter2;
    private bool updateCounter1 = false;
    private bool updateCounter2 = false;

    void Start()
    {
        ConnectToServer();
    }

    void ConnectToServer()
    {
        ws = new WebSocket(serverAddress);

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected to server");
        };

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Disconnected from server");
            // Implement reconnection logic
            Invoke("ConnectToServer", 5f);  // Attempt to reconnect after 5 seconds
        };

        ws.OnMessage += (sender, e) =>
        {
            var message = JsonUtility.FromJson<WebSocketMessage>(e.Data);

            if (message.type == "click_count1")
            {
                newCounter1 = message.value;
                updateCounter1 = true;
            }
            else if (message.type == "click_count2")
            {
                newCounter2 = message.value;
                updateCounter2 = true;
            }
        };

        ws.ConnectAsync();
    }

    void Update()
    {
        if (updateCounter1 && textMesh1)
        {
            textMesh1.text = "Yes: " + newCounter1.ToString("0");
            updateCounter1 = false;
        }
        if (updateCounter2 && textMesh2)
        {
            textMesh2.text = "No: " + newCounter2.ToString("0");
            updateCounter2 = false;
        }
    }

    void OnDestroy()
    {
        if (ws != null)
            ws.Close();
    }

    [System.Serializable]
    private class WebSocketMessage
    {
        public string type;
        public float value;
    }
}
