using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    // main object references (will be heavily expanded when full menu is designed
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingProgressBar;
    [SerializeField] private GameObject defMenuSelection;
    [SerializeField] private Animator titleAnimator;
    [SerializeField] private GameObject pressStartLabel;
    [SerializeField] private Animator buttonsAnimator;
    [SerializeField] private Button continueButton;

    private bool saveExists;

    [SerializeField] private Player playerSaveData;

    private bool pressedStart;

    // Start is called before the first frame update
    void Start()
    {
        
        if (SaveSystem.DoesPlayerFileExist())
        {
            playerSaveData.LoadPlayerData();
            saveExists = true;
        }
        else
        {
            continueButton.interactable = false;
            continueButton.GetComponentInChildren<Text>().color = Color.gray;
            saveExists = false;
        }
        if (!FindObjectOfType<AudioManager>().IsPlaying("Main_Menu"))
        {
            FindObjectOfType<AudioManager>().Play("Main_Menu");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (!pressedStart)
            {
                pressedStart = true;
                OpenMainMenu();
            }
        }
    }

    // going past press enter screen
    private void OpenMainMenu()
    {
        titleAnimator.SetBool("PressStart", true);
        buttonsAnimator.SetBool("PressStart", true);

        pressStartLabel.SetActive(false);

        eventSystem.SetSelectedGameObject(defMenuSelection);
    }

    // start new game
    public void NewGame()
    {
        // delete previous save data
        if (SaveSystem.DoesPlayerFileExist())
        {
            SaveSystem.DeletePlayerFile();
            SaveSystem.DeleteStorageFile();
            saveExists = false;
        }

        StartCoroutine(LoadingManager());
    }

    // continuing
    public void ContinueGame()
    {
        if (!SaveSystem.DoesPlayerFileExist())
        {
            return;
        }
        else
        {
            StartCoroutine(LoadingManager());
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // call when loading
    private IEnumerator LoadingManager()
    {
        loadingScreen.SetActive(true);
        Debug.Log("Currently Day " + playerSaveData.currentDay);
        string sceneName;
        if (saveExists)
        {
            if (playerSaveData.isNight)
            {
                sceneName = "Floor" + playerSaveData.currentLevel + " Night";
            }
            else
            {
                sceneName = "Floor" + playerSaveData.currentLevel;
            }
              
        }
        else
        {
            sceneName = "Floor1";
        }

        FindObjectOfType<AudioManager>().StopMusic("Main_Menu");

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadOperation.isDone)
        {
            // loading bar
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingProgressBar.fillAmount = progress;
            
            yield return null;
        }
        
    }

    
}
