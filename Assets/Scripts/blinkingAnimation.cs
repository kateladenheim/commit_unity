using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blinkingAnimation : MonoBehaviour
{
    SkinnedMeshRenderer _smr;
    [SerializeField]
    int framesBetweenBlinks = 1000;
    // Start is called before the first frame update
    void Start()
    {
        _smr = GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        int blinkInterval = Time.frameCount % framesBetweenBlinks;
        if (blinkInterval <= 5 || (blinkInterval >= 10 && blinkInterval < 15))
        {
            Blink(50);
        }
        else if (blinkInterval > 5 && blinkInterval < 10)
        {
            Blink(100);
        } else { 
            Blink(0); 
        }
    }

    void Blink(int shapeVal)
    {
        //left eye
        _smr.SetBlendShapeWeight(16, shapeVal);
        //right eye
        _smr.SetBlendShapeWeight(17, shapeVal);
    }
}
