using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator guardAnimator; 
    [SerializeField] LayerMask Layermask;
    public GameObject pllayer;
    public GameObject Guard;
    public Transform Respawn;
    public float speed;
    public Player Player;
    public Patrol Patrol;
    public PlayerMovement playerMovement;


    private void Start()
    {
       
    }


    private void FixedUpdate()
    {
        if (playerMovement.isHiding == false)
        {


            if (Patrol.currentPoint == Patrol.PointB.transform)
            {
                // Draws a ray that detects the player
                if (Physics2D.Raycast(transform.position, transform.right, 15f, Layermask))
                {
                    // shows the raycast line
                    //Debug.DrawLine(transform.position, new Vector2(10,0), Color.yellow);

                    Debug.Log("Get back to your cell!");
                    // Teleports the player back to their cell
                    pllayer.transform.position = Respawn.position;
                    if (Player.sanity > 0)
                    {
                        Player.sanity -= 0.3f;
                    }

                }
            }

            else if (Patrol.currentPoint == Patrol.PointA.transform)
            {
                if (Physics2D.Raycast(transform.position, -transform.right, 15f, Layermask))
                {
                    //  shows the raycast line
                    // Debug.DrawLine(transform.position, new Vector2(10,0), Color.yellow);

                    Debug.Log("Get back to your cell!");
                    // Teleports the player back to their cell
                    pllayer.transform.position = Respawn.position;
                    if (Player.sanity > 0)
                    {
                        Player.sanity -= 0.3f;
                    }
                }
            }
        }
       else if (playerMovement.isHiding == true)
        {
            Debug.Log("I sneak");
        }

        guardAnimator.SetFloat("Sanity", Player.sanity);
    }
}
