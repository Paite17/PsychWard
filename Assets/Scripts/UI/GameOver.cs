using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    private bool done;

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Retry();
        }
    }

    public void Retry()
    {
        if (!done)
        {
            FindObjectOfType<AudioManager>().StopMusic("Game_Over");
            FindObjectOfType<LoadingManager>().StartLoadingArea("Final Level");
            done = true;
        }
        
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Game_Over");
    }
}
