using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class rvalue : MonoBehaviour

{

    public TextMeshProUGUI textX;
    public TextMeshProUGUI textY;
    public TextMeshProUGUI textZ;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        textX.SetText("X : " + transform.position.x.ToString());
        textY.SetText("Y : " + transform.position.y.ToString());
        textZ.SetText("Z : " + transform.position.z.ToString());

    }
}