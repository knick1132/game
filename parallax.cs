/*
 * Author: Nicholas Kiv
 * Last Updated: 6/11/2018
 * 
 * parrallax script
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{


    public float smooth; //smooths the parallax
    public Transform[] backgrounds; //holds the background
    private float[] scale; // how much the camera moves

    private Transform cam;
    private Vector3 pastCam;


    void Awake()
    {
        cam = Camera.main.transform;
    }
    void Start()
    {

        pastCam = cam.position;

        scale = new float[backgrounds.Length];


        for (int i = 0; i < backgrounds.Length; i++)
        {
            scale[i] = backgrounds[i].position.z * -1;
        }

    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (pastCam.x - cam.position.x) * scale[i];

            float backgroundXpos = backgrounds[i].position.x + parallax;

            Vector3 backgroundPos = new Vector3(backgroundXpos, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundPos, smooth * Time.deltaTime);
        }

        pastCam = cam.position;
    }
}
