using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class Velocity : MonoBehaviour
{

    public TextMeshProUGUI textX;
    public TextMeshProUGUI textY;
    public TextMeshProUGUI textZ;

    private float startPosX;
    private float endPosX;
    private float velocityX;

    private float startPosY;
    private float endPosY;
    private float velocityY;

    private float startPosZ;
    private float endPosZ;
    private float velocityZ;

    private float delay = 1.0f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        
        startPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            CalculateVelocity();
            timer = 0.0f;
        }

    }

    void CalculateVelocity ()
    {

        endPosX = transform.position.x;
        endPosY = transform.position.y;
        endPosZ = transform.position.z;

        velocityX = (endPosX - startPosX) / Time.deltaTime;
        velocityY = (endPosY - startPosY) / Time.deltaTime;
        velocityZ = (endPosZ - startPosZ) / Time.deltaTime;

        textX.SetText("X Axis Velocity : " + velocityX.ToString());
        textY.SetText("Y Axis Velocity : "+velocityY.ToString());
        textZ.SetText("Z Axis Velocity : " + velocityZ.ToString());

        startPosX = endPosX;
        startPosY = endPosY;
        startPosZ = endPosZ;
    }

}
