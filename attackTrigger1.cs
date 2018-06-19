/*
 * Author: Nicholas Kiv
 * Last Updated: 6/11/2018
 * 
 * Trigger for the player attack range to activate/message enemy script
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger1 : MonoBehaviour {
    
    
    void OnTriggerEnter2D(Collider2D col) //sends message to hit enemy to execute IsHit function
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("hit");
            col.gameObject.SendMessage("IsHit", 5.0F);
        }
    }

}
