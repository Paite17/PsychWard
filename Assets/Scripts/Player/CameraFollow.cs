using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // player target for camera
    public Transform target;
    public Vector3 offset;

    public bool dontFollow;
    public bool allowYAxis;

    private Player player;

    private void Start()
    {
        // funny player moment
        if (target.name == "Player")
        {
            player = target.GetComponent<Player>();
        }
        
    }

    private void FixedUpdate()
    {

        if (!dontFollow)
        {
            //transform.position = new Vector3(target.position.x + offset.x, transform.position.y, offset.z);
            transform.position = target.position + offset;
        }
        else
        {
            // makes it look less clunky
            if (target.name == "Player")
            {
                if (player.sceneName != "Final Level")
                {
                    transform.position = new Vector3(transform.position.x, target.position.y + offset.y, transform.position.z);
                }
            }
        }

        // at one point i didn't want the camera to follow the y axis but for now that's been disabled due to issues
        /*if (allowYAxis )
        {
            transform.position = new Vector3(target.position.x + offset.x, target.position.y + offset.y, offset.z);
        } */
        
    }

    // when a transition occurs we need to correct the Y axis value
    public void CameraCorrection()
    {
        transform.position = target.position + offset;
    }


    // change to a new target gameObject
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
