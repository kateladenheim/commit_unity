using UnityEngine;
using UnityEngine.InputSystem;

public class MinisTrigger : MonoBehaviour
{
    public InputActionAsset MIDI_InputActions;  // Reference to your Input Actions Asset
    private InputAction noteTriggeredAction;
    public GameObject inputType_Sample;
    public AudioClip intro_AudioClip;

    private AudioSource audioSource;  // Reference to the AudioSource component

    public bool startTimer = false;

    private void Awake()
    {
        // Cache the reference to the AudioSource
        audioSource = inputType_Sample.GetComponent<AudioSource>();

        // Find the MIDI action map and specific action
        var actionMap = MIDI_InputActions.FindActionMap("Midi");
        noteTriggeredAction = actionMap.FindAction("NewTrigger");

        // Subscribe to the noteTriggered event
        if (noteTriggeredAction != null)
        {
            noteTriggeredAction.performed += OnNoteTriggered;
        }
    }

    private void OnEnable()
    {
        noteTriggeredAction.Enable();
    }

    private void OnNoteTriggered(InputAction.CallbackContext context)
    {
        if(startTimer == false)
        {
        startTimer = true;
        }
        Debug.Log("MIDI Note Triggered!");

        // Check if the audio source is already playing
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Set the clip and play it
        audioSource.clip = intro_AudioClip;
        audioSource.Play();
    }

    private void OnDisable()
    {
        // Stop the audio source if it's playing
        if (audioSource && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.clip = null;  // Optional: Clear the clip
        }

        // Disable the MIDI action
        noteTriggeredAction.Disable();
    }
}