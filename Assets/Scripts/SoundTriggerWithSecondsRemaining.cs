using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundTriggerWithSecondsRemaining : MonoBehaviour
{
    public AudioClip feedbackAudio;

    public AudioClip youHave;
    public AudioClip minutes;
    public AudioClip remaining;

    private AudioSource adSource;
    public AudioClip errorMessage;

    public KeyCode TriggerKey = KeyCode.Space;

    public AudioClip[] audioClipSequence;
    public AudioClip[] minutesArray;

    public MinisTrigger timerValue;

    public int bufferSecondsToGetInPosition = 3;

    private int currentTime;
    private int totalTime = 60;
    private int minutesRemaining;

    private void Awake()
    {
        adSource = GetComponent<AudioSource>();
        adSource.clip = null;

        audioClipSequence = new AudioClip[] { feedbackAudio, youHave, minutes, remaining };
        minutesArray = Resources.LoadAll<AudioClip>("counterAudio");

    }

    void Update()
    {
        if(timerValue.startTimer == true)
        {
        //the current time in minutes is the integer value of the time in seconds over 60
        currentTime = (int)Time.time/60;
        minutesRemaining = totalTime - currentTime;
        Debug.Log(minutesRemaining + " minutes remaining");
        }
        else
        {
            minutesRemaining = totalTime;
        }

        if (Input.GetKeyDown(TriggerKey))
        {
            StartCoroutine(playAudioSequentially());
            Debug.Log("playing");
        }
        audioClipSequence[2] = minutesArray[minutesRemaining];

    }

    IEnumerator playAudioSequentially()
    {
        yield return null;
        yield return new WaitForSeconds(bufferSecondsToGetInPosition);

        //1.Loop through each AudioClip
        for (int i = 0; i < audioClipSequence.Length; i++)
        {
            //2.Assign current AudioClip to audiosource
            adSource.clip = audioClipSequence[i];

            //3.Play Audio
            adSource.Play();

            //4.Wait for it to finish playing
            while (adSource.isPlaying)
            {
                yield return null;
            }

            //5. Go back to #2 and play the next audio in the adClips array
        }
    }
}