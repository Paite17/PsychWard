using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{

    // actual parameters
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingProgressBar;


    public void StartLoadingArea(Scene sceneToLoad)
    {
        StartCoroutine(LoadNewScene(sceneToLoad));
    }

    public void StartLoadingArea(string sceneToLoad)
    {
        StartCoroutine(LoadNewScene(sceneToLoad));
    }

    // load a scene in the background using async 
    private IEnumerator LoadNewScene(Scene sceneToLoad)
    {
        loadingScreen.SetActive(true);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad.name);

        while (!loadOperation.isDone)
        {
            // loading bar
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingProgressBar.fillAmount = progress;
            yield return null;
        }
    }

    // load a scene in the background using async using a string 
    private IEnumerator LoadNewScene(string sceneToLoad)
    {
        loadingScreen.SetActive(true);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!loadOperation.isDone)
        {
            // loading bar
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingProgressBar.fillAmount = progress;
            yield return null;
        }
    }
}
