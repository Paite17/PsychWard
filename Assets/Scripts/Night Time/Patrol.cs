using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Animator anim;
    public Transform currentPoint;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = PointB.transform;
        //anim.SetBool("isRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == PointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        // allows the guard to walk towards a certain point, creating a patrol path
        if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == PointB.transform)
        {
            flip();
            currentPoint = PointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 1f && currentPoint == PointA.transform)
        {
            flip();
            currentPoint = PointB.transform;
        }
    }
    public void flip()
    {
        // allows the guard to flip it's sprite based on the direction they are going
        Vector3 localscale= transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PointA.transform.position, 1f);
        Gizmos.DrawWireSphere(PointB.transform.position, 1f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position); 
    }
}
