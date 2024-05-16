using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterSpriteFlip : MonoBehaviour
{

    private Transform playerTarget;
    private SpriteRenderer spriteRenderer;
    [SerializeField] AIPath aiPath;

    private void Start()
    {
        playerTarget = GameObject.Find("Player").GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        /*if (playerTarget.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        } */

        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            spriteRenderer.flipX = true;
        }
    }
}
