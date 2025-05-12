using UnityEngine;
using MidiJack;

public class MidiToggle : MonoBehaviour
{
    public GameObject targetObject; // The GameObject you want to activate/deactivate
    public int midiNote = 60;       // The MIDI note number (e.g., 60 is Middle C)

    private bool isNoteOn = false;
    private bool[] noteStates = new bool[128]; // To track which notes are currently pressed

    private void Update()
    {
        // Check and log all note states
        for (int i = 0; i <= 127; i++)
        {
            float currentVelocity = MidiMaster.GetKey(i);
            if (currentVelocity > 0 && !noteStates[i]) // Note is newly pressed
            {
                Debug.Log($"Note {i} is pressed");
                noteStates[i] = true;
            }
            else if (currentVelocity == 0 && noteStates[i]) // Note is released
            {
                noteStates[i] = false;
            }
        }

        // Your original functionality for toggling the targetObject
        float targetNoteVelocity = MidiMaster.GetKey(midiNote);
        if (targetNoteVelocity > 0 && !isNoteOn)
        {
            // Note is pressed
            targetObject.SetActive(!targetObject.activeSelf);
            isNoteOn = true;
        }
        else if (targetNoteVelocity == 0 && isNoteOn)
        {
            // Note is released
            isNoteOn = false;
        }
    }
}