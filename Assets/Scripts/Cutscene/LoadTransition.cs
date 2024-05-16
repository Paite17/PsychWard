using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadTransition : MonoBehaviour
{
    // Attach this script to an object to make it a level transition trigger
    // it should have additional parameters such as the target transform location, maybe the type of transition animation
    // It should run a coroutine 
    [SerializeField] private Transform targetDestination;
    [SerializeField] private Animator transitionAnimator;

    // this is for the timing between the entrance animation starting and the exit animation starting
    [SerializeField] private float transitionTimer;
    [SerializeField] private bool startOnContact;

    // scene management
    [SerializeField] private bool changeSceneOnActivate;
    [SerializeField] private Scene sceneDestination;

    private CameraFollow cam;

    private Player playerRef;

    // a quick check to make sure the button can't be spammed
   


    private void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");
        cam = camera.GetComponent<CameraFollow>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerRef == null)
        {
            playerRef = collision.gameObject.GetComponent<Player>();
        }
        
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "CutscenePlayer")
        {
            if (startOnContact)
            {
                StartCoroutine(StartTransition());
            }
        }
    }

    // actual transition period
    // maybe it should use animation events if i can figure them out
    private IEnumerator StartTransition()
    {
        // do normal procedure if false
        if (!changeSceneOnActivate)
        {
            // set transition
            transitionAnimator.SetBool("active", true);
            

            yield return new WaitForSeconds(transitionTimer);

            // get player andd set position
            GameObject plr = GameObject.Find("Player");
            plr.transform.position = new Vector3(targetDestination.position.x, targetDestination.position.y, plr.transform.position.z);
            cam.CameraCorrection();

            transitionAnimator.SetBool("active", false);
            
        }
        else
        {
            // set transition
            transitionAnimator.SetBool("active", true);

            yield return new WaitForSeconds(transitionTimer);

            // make sure sceneDestination isn't null
            // also make sure the scene is in the build settings too thats an easy thing to miss
            // also also make sure a loading manager and loading screen are on the scene
            // (drag in the prefabs, the loading screen MUST be a child of the canvas)
            FindObjectOfType<LoadingManager>().StartLoadingArea(sceneDestination);
        }

    }

    // public call for outside of this component
    public void BeginTransition()
    {
        StartCoroutine(StartTransition());
    }
}
