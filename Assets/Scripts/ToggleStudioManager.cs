using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStudioManager : MonoBehaviour
{
    private MonoBehaviour studioManagerScript; // Reference to the StudioManager script
    private bool isStudioManagerActive = true; // Flag to keep track of the script's state

    private void Start()
    {
        // Find the StudioManager script on this GameObject
        studioManagerScript = GetComponent<StudioManager>();

        if (studioManagerScript == null)
        {
            Debug.LogError("StudioManager script not found on this GameObject.");
            enabled = false; // Disable this script to prevent errors
        }
    }

    private void Update()
    {
        // Check for a key press (e.g., "T") to toggle the StudioManager script
        if (Input.GetKeyDown(KeyCode.T))
        {
            isStudioManagerActive = !isStudioManagerActive; // Toggle the state

            // Enable or disable the StudioManager script based on the state
            studioManagerScript.enabled = isStudioManagerActive;

            if (isStudioManagerActive)
            {
                Debug.Log("StudioManager script is now active.");
            }
            else
            {
                Debug.Log("StudioManager script is now inactive.");
            }
        }
    }
}
