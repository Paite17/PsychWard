using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Movement script for the security guard that appears in the first cutscene
public class CutsceneSGMovement : MonoBehaviour
{
    public Vector3 pathway;
    public bool activate;

    public float speed;

    private void Update()
    {
        if (activate)
        {
            transform.position += pathway * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SecurityCutsceneDisappear")
        {
            activate = false;
            gameObject.SetActive(false);
        }
    }
}
