/*
 * Author: Nicholas Kiv
 * Last Updated: 6/11/2018
 * 
 * makes the camera smooth
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smooth_Camera : MonoBehaviour
{

    public Transform target;
    public float speed;
    public Vector3 offset;


    private void FixedUpdate()
    {

        Vector3 new_pos = target.position + offset;
        Vector3 smooth_pos = Vector3.Lerp(transform.position, new_pos, speed);
        transform.position = smooth_pos;

    }
}
