using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Material[] myMaterials = new Material[5];
    public GameObject bodyToChange;
    public GameObject turtleneck;
    public GameObject hair;
    public GameObject bangs;
    public GameObject eyes;
    void Start()
    {
        Material[] oldMaterials = bodyToChange.GetComponent<Renderer>().materials;
        oldMaterials = myMaterials;
        bodyToChange.GetComponent<Renderer>().materials = oldMaterials;
        turtleneck.GetComponent<Renderer>().material = oldMaterials[4];
        hair.GetComponent<Renderer>().material = oldMaterials[2];
        bangs.GetComponent<Renderer>().material = oldMaterials[4];
        Material[] eyeMats = eyes.GetComponent<Renderer>().materials;
        eyeMats[1] = myMaterials[3];
        eyeMats[3] = myMaterials[3];
        eyes.GetComponent<Renderer>().materials = eyeMats;

        /*        for(int i = 0; i < myMaterials.Length; i++)
                {
                    bodyToChange.GetComponent<Renderer>().materials[i] = myMaterials[i];
                }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
