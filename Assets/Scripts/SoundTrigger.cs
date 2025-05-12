using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip sample_YES;
    public AudioClip sample_NO;
    public AudioClip intro;

    private AudioSource source;

    public KeyCode TriggerKey = KeyCode.Space;
    public bool Append = false;

    private int haveBothClipsPlayed = 0;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = intro;
        //source.Play();
    }

    void Update()
    {
        //if you want them one after the other
        //set up a condiition where she stops talking after reading both yes and no answers
        if (Append)
        {
            if (haveBothClipsPlayed < 2 && !source.isPlaying)
            {
                if (Input.GetKeyDown(TriggerKey))
                {
                    source.clip = sample_YES;
                    source.Play();
                    haveBothClipsPlayed++;
                }
                if (!source.isPlaying && source.clip != null)
                {
                    source.clip = ChangeClip(source.clip);
                    source.Play();
                    haveBothClipsPlayed++;
                }
            }
            else
            {
                //if trigger key is pressed again, reset the counter and play both clips again
                if (Input.GetKeyDown(TriggerKey))
                {
                    haveBothClipsPlayed = 0;
                }
            }
        }

        //if you want to play them separately
        if (!Append)
        {
            if (Input.GetKeyDown(TriggerKey))
            {
                source.clip = ChangeClip(source.clip);
                source.Play();
            }
        }
    }
    AudioClip ChangeClip(AudioClip clip)
    {
        //if the yes clip is playing, change to the no clip
        if (clip == sample_YES)
        {
            clip = sample_NO;
        }
        //if the no clip is playing, change to the yes clip
        else if (clip == sample_NO)
        {
            clip = sample_YES;
        }
        else
        //if the clip is null or has been assigned another value
        //assign a default value of yes
        {
            clip = sample_YES;
        }
        //return whichever clip
        return clip;
    }
}
