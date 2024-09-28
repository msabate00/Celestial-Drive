using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirdLayerVisualController : MonoBehaviour
{
    public Transform floorGird, roofGrid;
    public Camera cameraMain;

    public float speedDisolve;

    private Material floorMaterial, roofMaterial;

    private float maxAlphaF = 0.3f, maxAlphaR = 0.3f;
    private float contadorF, contadorR;

    private void Start()
    {
        if (cameraMain == null) {
            cameraMain = Camera.main;
        }
        floorMaterial = floorGird.GetComponent<MeshRenderer>().sharedMaterial;
        roofMaterial = roofGrid.GetComponent<MeshRenderer>().sharedMaterial;
        //maxAlphaF = floorMaterial.GetFloat("_Alpha");
        //maxAlphaR = roofMaterial.GetFloat("_Alpha");
        floorMaterial.SetFloat("_Alpha", 0);
        roofMaterial.SetFloat("_Alpha", 0);
    }

    // Update is called once per frame
    void Update()
    {

        float floorGrid_Y = floorGird.position.y + (floorGird.localScale.y/2);
        float roofGrid_Y = roofGrid.position.y - (roofGrid.localScale.y/2);

        if (cameraMain.transform.position.y > floorGrid_Y)
        {
            //floorGird.GetComponent<MeshRenderer>().enabled = true;
            //roofGrid.GetComponent<MeshRenderer>().enabled = false;

            if (contadorF < maxAlphaF) {
                contadorF += Time.deltaTime * speedDisolve;
                floorMaterial.SetFloat("_Alpha", contadorF);
            }
            if (contadorR > 0)
            {
                contadorR -= Time.deltaTime * speedDisolve;
                roofMaterial.SetFloat("_Alpha", contadorR);
            }






        }
        else {
            //floorGird.GetComponent<MeshRenderer>().enabled = false;
            //roofGrid.GetComponent<MeshRenderer>().enabled = true;


            if (contadorF > 0)
            {
                contadorF -= Time.deltaTime * speedDisolve;
                floorMaterial.SetFloat("_Alpha", contadorF);
            }
            if (contadorR < maxAlphaR)
            {
                contadorR += Time.deltaTime * speedDisolve;
                roofMaterial.SetFloat("_Alpha", contadorR);
            }


        }
    }
}
