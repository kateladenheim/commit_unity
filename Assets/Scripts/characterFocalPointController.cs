using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class characterFocalPointController : MonoBehaviour
{
    public GameObject matPosition;
    public GameObject audiencePosition;
    public GameObject computerPosition;
    public GameObject head_activeLookTarget;
    public GameObject eyes_activeLookTarget;
    public GameObject rig;
    Vector3 originalHeadPos;
    Vector3 originalEyePos;
    Vector3 currentHeadPos;
    Vector3 currentEyePos;
    Vector3 matPos;
    Vector3 audPos;
    Vector3 compPos;




    public int interpolationFramesCount = 300; // Number of frames to completely interpolate between the 2 positions
    int elapsedFrames = 0;

    bool isTalking = false;
    bool isFalling = false;
    bool isControlling = false;
    bool lookAround = false;
    bool lookAway = true;

    int randomMotion = 1;

    float rigWeight = 1.0f;
    public float lookAroundSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //save the original values of the head and eye focal points so we can lerp back to them if needed
        originalHeadPos = head_activeLookTarget.transform.position;
        originalEyePos = eyes_activeLookTarget.transform.position;
        currentHeadPos = head_activeLookTarget.transform.position;
        currentEyePos = eyes_activeLookTarget.transform.position;

        matPos = matPosition.transform.position;
        audPos = audiencePosition.transform.position;
        compPos = computerPosition.transform.position;

        //variable for the multi aim constraint float
        rigWeight = rig.GetComponent<Rig>().weight;

    }

    // Update is called once per frame
    void Update()
    {
        float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

        //if falling, look at mat 
        if (isFalling && !isTalking)
        {
            currentHeadPos = Vector3.Lerp(currentHeadPos, matPos, interpolationRatio);
            currentEyePos = Vector3.Lerp(currentEyePos, matPos, interpolationRatio); //eyes faster than head

        }
        //if controlling, look at computer
        if (isControlling && !isTalking)
        {
            currentHeadPos = Vector3.Lerp(currentHeadPos, compPos, interpolationRatio);
            currentEyePos = Vector3.Lerp(currentEyePos, compPos, interpolationRatio); //eyes faster than head
        }

        //if talking, look at audience
        if (isTalking)
        {
            currentHeadPos = Vector3.Lerp(currentHeadPos, audPos, interpolationRatio);
            currentEyePos = Vector3.Lerp(currentEyePos, audPos, interpolationRatio); //eyes faster than head
        }

        //always look at audience if audio source is playing or if 1 is pressed
        if (GetComponent<AudioSource>().isPlaying || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("look at audience");
            isControlling = false;
            isFalling = false;
            isTalking = true;
        }

        //trigger looking at mat if 2 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("look at mat");
            isTalking = false;
            isControlling = false;
            isFalling = true;
        }
        //trigger looking at computer if 3 is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("look at computer");
            isTalking = false;
            isFalling = false;
            isControlling = true;
        }
        if (Time.frameCount % 1000 == 1)
        {
            lookAround = true;
        }
        if (lookAround)
        {
            if (lookAway)
            {
                while (rigWeight > 0)
                {
                    rigWeight -= lookAroundSpeed;
                }
                if (rigWeight <= 0)
                {
                    lookAway = false;
                }
            }
            else
            {
                while (rigWeight < 1)
                {
                    rigWeight += lookAroundSpeed;
                }
                if (rigWeight >= 1)
                {
                    lookAway = true;
                }
            }
        }

        head_activeLookTarget.transform.position = currentHeadPos;
        eyes_activeLookTarget.transform.position = currentEyePos;
        elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)

    }

}
