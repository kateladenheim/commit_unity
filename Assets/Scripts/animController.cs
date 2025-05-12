using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animController : MonoBehaviour
{

    public Animator anim;
    private bool isFlashing;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isFlashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !isFlashing)
        {
            anim.Play("flashflash");
            isFlashing = true;
        }

        if (Input.GetKeyDown("space") && isFlashing)
        {
            anim.Play("New State");
            isFlashing = false;
        }
    }
}
