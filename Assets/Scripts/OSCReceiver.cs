using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;
using TMPro;

public class OSCReceiver : MonoBehaviour
{
    public TextMeshProUGUI textMesh1; // Drag your TextMeshPro object for counter 1 here in the inspector
    public TextMeshProUGUI textMesh2; // Drag your TextMeshPro object for counter 2 here in the inspector

    private OscServer oscServer;
    private const int PORT = 8000; // Listening port.

    private void Start()
    {
        // Initialize the OSC server.
        oscServer = new OscServer(PORT);
        Debug.Log($"OSC Server Listening on Port {PORT}");

        // Register callbacks.
        oscServer.MessageDispatcher.AddCallback("/clickCount1", OnClickCount1Received);
        oscServer.MessageDispatcher.AddCallback("/clickCount2", OnClickCount2Received);
    }

    void OnClickCount1Received(string address, OscDataHandle data)
    {
        float count = data.GetElementAsFloat(0);
        Debug.Log($"Received OSC at address {address} with value {count}");

        // Update the TextMeshPro object.
        if (textMesh1 != null)
        {
            textMesh1.text = "Yes: " + count.ToString("0"); // Using "0" to format it as a whole number
        }
    }

    void OnClickCount2Received(string address, OscDataHandle data)
    {
        float count = data.GetElementAsFloat(0);
        Debug.Log($"Received OSC at address {address} with value {count}");

        // Update the other TextMeshPro object.
        if (textMesh2 != null)
        {
            textMesh2.text = "No: " + count.ToString("0"); // Using "0" to format it as a whole number
        }
    }

    private void OnDestroy()
    {
        if (oscServer != null)
        {
            oscServer.Dispose();
        }
    }
}
