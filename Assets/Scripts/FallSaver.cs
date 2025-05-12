using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FallSaver : MonoBehaviour
{
    public float activeTime = 2f;
    public Material mat; // This is the new material you want to assign to all sub-meshes.

    public int numberOfRows;
    public int objectsPerRow;
    public float spacing;
    private int currentGridPosition = 0;
    Vector3 gridStartPosition = Vector3.zero;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public Transform positionToSpawn;

    private bool isFallen;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private List<GameObject> generatedMeshes = new List<GameObject>();

    public PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput.actions["AdjustTransparency"].performed += OnAdjustTransparency;
        playerInput.actions["AdjustTransparency"].Enable();
    }

    private void OnDisable()
    {
        playerInput.actions["AdjustTransparency"].performed -= OnAdjustTransparency;
        playerInput.actions["AdjustTransparency"].Disable();
    }

    public void OnAdjustTransparency(InputAction.CallbackContext context)
    {
        AdjustMeshTransparency();
        Debug.Log("midigo");
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A)) // Change KeyCode.A to any key you want.
    //    {
    //        AdjustMeshTransparency();
    //    }
    //}

    void AdjustMeshTransparency()
    {
        // Adjusting transparency of the SkinnedMeshRenderers
        if (skinnedMeshRenderers == null)
            skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            AdjustMaterialTransparency(skinnedMeshRenderer.materials);
        }

        // Adjusting transparency of the generated meshes
        foreach (GameObject gObj in generatedMeshes)
        {
            MeshRenderer mr = gObj.GetComponent<MeshRenderer>();
            if (mr)
            {
                AdjustMaterialTransparency(mr.materials);
            }
        }
    }

    void AdjustMaterialTransparency(Material[] materials)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            Color color = materials[i].color;
            color.a = Mathf.Clamp01(color.a - 0.04f); // Reduce alpha by 4/100 or 0.04.
            materials[i].color = color;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!isFallen)
        {
            isFallen = true;

            int gridPosCount = 0;
            for (int row = 0; row < numberOfRows; row++)
            {
                for (int col = 0; col < objectsPerRow; col++)
                {
                    if (gridPosCount == currentGridPosition)
                    {
                        Vector3 startingPos = new Vector3(gridStartPosition.x + (col * spacing), gridStartPosition.y - (row * spacing), gridStartPosition.z);
                        positionToSpawn.position = startingPos;
                    }

                    gridPosCount++;
                }
            }

            StartCoroutine(ActivateBody(activeTime));

            currentGridPosition++;
            if (currentGridPosition > numberOfRows * objectsPerRow)
            {
                currentGridPosition = 0;
            }
        }
    }

    IEnumerator ActivateBody(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                generatedMeshes.Add(gObj);
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;

                // Create an array of new materials based on the public `mat` variable
                Material[] newMaterials = new Material[skinnedMeshRenderers[i].materials.Length];
                for (int j = 0; j < skinnedMeshRenderers[i].materials.Length; j++)
                {
                    newMaterials[j] = mat; // Assigning the public mat variable to all sub-meshes.
                }

                // Assign the new materials to the new MeshRenderer
                mr.materials = newMaterials;
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        isFallen = false;
    }
}
