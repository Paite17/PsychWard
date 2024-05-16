using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private GameObject logo1;
    [SerializeField] private GameObject logo2;
    [SerializeField] private GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartupSequence());
    }

    private IEnumerator StartupSequence()
    {
        yield return new WaitForSeconds(4f);

        text.SetActive(false);
        logo1.SetActive(true);
        logo2.SetActive(true);

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("MainMenu");
    }
}
