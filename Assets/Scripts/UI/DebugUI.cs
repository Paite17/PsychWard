using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.EventSystems;
using System.IO;
using System;

public enum DebugMenuState
{
    NONE,
    DEFAULT,
    ITEMS,
    SCENES,
    PLAYERFLAGS,
    MISC
}

public class DebugUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Dropdown itemDropdown;
    [SerializeField] private Dropdown sceneDropdown;
    [SerializeField] private GameObject itemSubMenu;
    [SerializeField] private GameObject sceneSubMenu;
    [SerializeField] private GameObject prefabSubMenu;
    [SerializeField] private GameObject flagSubMenu;
    [SerializeField] private GameObject miscSubMenu;
    [SerializeField] private GameObject defItemObj;
    [SerializeField] private GameObject defSceneObj;
    [SerializeField] private GameObject defPrefabObj;
    [SerializeField] private GameObject defFlagObj;
    [SerializeField] private GameObject defMiscObj;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject defMenuObj;
    [SerializeField] private GameObject debugMenu;
    [SerializeField] private Dropdown prefabDropdown;
    [SerializeField] private InputField prefabX;
    [SerializeField] private InputField prefabY;
    [SerializeField] private Dropdown flagDropdown;
    [SerializeField] private Toggle flagToggle;


    public DebugMenuState menuState;

    // lists pertaining to the item submenu
    private List<string> allItemNames;
    private List<Item> allItems;

    // lists pertaining to the scenes submenu
    private List<string> allSceneNames;

    // lists pertaining to the prefab submenu
    private List<string> allPrefabNames;
    private List<GameObject> allPrefabs;

    // flag list
    private List<string> allFlagNames;

    // Start is called before the first frame update
    void Start()
    {
        menuState = DebugMenuState.NONE;
        allItemNames = new List<string>();
        allItems = new List<Item>();
        allSceneNames = new List<string>();
        allPrefabNames = new List<string>();
        allPrefabs = new List<GameObject>();
        allFlagNames = new List<string>();
    }

    public void OpenItemSubmenu()
    {
        // get all items
        var item = Resources.LoadAll("Items", typeof(Item)).Cast<Item>();

        // look through them
        foreach (var current in item)
        {
            Debug.Log(current.ItemName);
            allItemNames.Add(current.ItemName);
            allItems.Add(current);
        }

        itemDropdown.AddOptions(allItemNames);
        menuState = DebugMenuState.ITEMS;
        itemSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defItemObj);
    }

    public void OpenSceneSubmenu()
    {
        // get scene count
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        // get scene names
        for (int i = 0; i < sceneCount; i++)
        {
            allSceneNames.Add(Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }

        sceneDropdown.AddOptions(allSceneNames);
        menuState = DebugMenuState.SCENES;
        sceneSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defSceneObj);
    }

    public void OpenPrefabSubmenu()
    {
        // get all objects
        var prefab = Resources.LoadAll("Objects", typeof(GameObject)).Cast<GameObject>();

        // look through em
        foreach (var current in prefab)
        {
            allPrefabNames.Add(current.name);
            allPrefabs.Add(current);
        }

        menuState = DebugMenuState.SCENES;
        prefabSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defPrefabObj);
        prefabDropdown.AddOptions(allPrefabNames);
    }

    public void OpenFlagSubmenu()
    {
        // add flag names
        for (int i = 0; i < player.playerFlags.Count; i++)
        {
            allFlagNames.Add(player.playerFlags[i].FlagName);
        }
        flagDropdown.AddOptions(allFlagNames);
        menuState = DebugMenuState.PLAYERFLAGS;
        flagSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defFlagObj);
    }

    public void OpenMiscSubmenu()
    {
        menuState = DebugMenuState.MISC;
        miscSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defMiscObj);
    }



    // when pressing the add button on the item submenu
    public void OnItemAddButton()
    {
        Item thisItem = null ;
        // find matching item from both lists
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].ItemName == itemDropdown.options[itemDropdown.value].text)
            {
                thisItem = allItems[i];
            }
        }


        // Add item
        if (thisItem != null)
        {
            player.PickUpItem(thisItem);
        }
        else
        {
            Debug.Log("Somehow this item doesn't exist");
        }
        
    }

    // load into scene
    public void LoadSelectedScene()
    {
        string sceneName = sceneDropdown.options[sceneDropdown.value].text;

        FindObjectOfType<AudioManager>().StopAllMusic();
        GameObject loadObj = GameObject.Find("LoadManager");
        loadObj.GetComponent<LoadingManager>().StartLoadingArea(sceneName);
    }

    // create prefab real!!!
    public void SpawnPrefab()
    {
        string prefabName = prefabDropdown.options[prefabDropdown.value].text;
        GameObject objToSpawn = null;

        Vector3 spawnLoc = new Vector3(Convert.ToInt32(prefabX.text), Convert.ToInt32(prefabY.text), 0);

        // find obj
        for (int i = 0; i < allPrefabs.Count; i++)
        {
            if (allPrefabs[i].name == prefabName)
            {
               objToSpawn = allPrefabs[i];
            }
        }

        // if its the item pickup put a random item in it for funsies
        if (objToSpawn.name == "ItemPickup")
        {
            // get all items
            var item = Resources.LoadAll("Items", typeof(Item)).Cast<Item>();

            // look through them
            foreach (var current in item)
            {
                allItems.Add(current);
            }

            int r = UnityEngine.Random.Range(0, allItems.Count);
            objToSpawn.GetComponent<ItemPickup>().itemToGive = allItems[r];
            // deload the list after for memory saving !!!!!
            allItems.Clear();
        }

        GameObject temp = Instantiate(objToSpawn, spawnLoc, objToSpawn.transform.rotation);
    }

    public void SetFlag()
    {
        // get toggle value
        if (flagToggle.isOn)
        {
            // set flag to true
            player.SetFlagBool(flagDropdown.options[flagDropdown.value].text);
        }
        else
        {
            player.DeactivateFlagBool(flagDropdown.options[flagDropdown.value].text);
        }
    }

    // when pressing the button to open/close the menu
    public void ToggleMenu()
    {
        Debug.Log("ToggleMenu() called");
        switch (menuState)
        {
            case DebugMenuState.NONE:
                // if the menu isn't open, open it
                player.gameObject.GetComponent<PlayerMovement>().canMove = false;
                menuState = DebugMenuState.DEFAULT;
                debugMenu.SetActive(true);
                eventSystem.SetSelectedGameObject(defMenuObj);
                break;
            case DebugMenuState.DEFAULT:
                // close the menu
                menuState = DebugMenuState.NONE;
                player.gameObject.GetComponent<PlayerMovement>().canMove = false;
                debugMenu.SetActive(false);
                eventSystem.SetSelectedGameObject(null);

                // de-load unneeded data
                allItemNames = new List<string>();
                allItems = new List<Item>();
                allSceneNames = new List<string>();
                itemDropdown.ClearOptions();
                sceneDropdown.ClearOptions();
                prefabDropdown.ClearOptions();
                break;
            default:
                // return to default state
                menuState = DebugMenuState.DEFAULT;
                debugMenu.SetActive(true);
                itemSubMenu.SetActive(false);
                sceneSubMenu.SetActive(false);
                prefabSubMenu.SetActive(false);
                flagSubMenu.SetActive(false);
                miscSubMenu.SetActive(false);
                eventSystem.SetSelectedGameObject(defMenuObj);
                break;
        }
    }

    

}
