/*
* Author: Loh Shau Ern Shaun
* Date: 20/07/2024
* Description: 
* Makes canvas UI face player camera at all times
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Late Update is called once per frame, AFTER Update function
    void LateUpdate()
    {
        Transform cam = GameObject.Find("SimulationCamera").transform;
        // Face canvas object towards the player
        transform.LookAt(cam);
    }
}
