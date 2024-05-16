using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class DemoEnd : MonoBehaviour
{
    [SerializeField] private Animator fadeAnimator;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Main_Menu");
    }

    // Update is called once per frame
    void Update()
    {
        // ew input
        if (Keyboard.current.enterKey.isPressed)
        {
            StartCoroutine(ReturnToMenu());
        }
    }

    private IEnumerator ReturnToMenu()
    {
        fadeAnimator.SetBool("Pressed", true);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("MainMenu");
    }
}
