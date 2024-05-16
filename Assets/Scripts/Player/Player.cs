using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EventStage
{
    FIRST,
    SECOND,
    THIRD,
    FOURTH,
    FITH
}

// As opposed to PlayerMovement (which handles moving the player object) this class will contain data related to player stats
// e.g. sanity, items, quests, etc.
public class Player : MonoBehaviour
{


    // stores all items the player has currently
    public List<ItemBase> playerInventory;

    // a 0-1 float that determines the player's current sanity
    [Range(0, 1)] public float sanity = 1f;

    // a value I will probably forget about up until the transitions between scenes is done
    // but it will mostly act as a way for the game to remember which level of the building you're at
    public int currentLevel;

    // a list of all the characters the player has met, will be useful for the corkboard UI
    public List<string> charactersMet;

    // a list containing every quest the player has
    public List<QuestBase> currentQuests;

    // a list containing every quest the player has completed already
    public List<QuestBase> completedQuests;

    // the amount of money the player has, cant remember what the currency was atm so just calling it money
    public int money;

    // event tokens, I believe these will be used on an event (taking on a quest maybe?) and once they're all spent
    // night commences(?)
    // didn't even use these bruh
    public int eventTokens;
    // day counter
    public int currentDay;

    // check if its night
    public bool isNight;

    public EventStage currentStage;

    // FLAGS
    public List<PlayerFlags> playerFlags;

    // position data
    public float playerX;
    public float playerY;
    public bool didlose;

    private UnityEngine.SceneManagement.Scene scene;
    public string sceneName;

    // this should be true whenever 
    public bool changingFloor;

    private void Awake()
    {
        if (didlose)
        {
            didlose = false;
        }

        scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        sceneName = scene.name;
        // save data management here
        if (SaveSystem.DoesPlayerFileExist())
        {
            // add to this wherever we want the player to load in a specific position
            if (sceneName != "Final Level")
            {
                LoadPlayerData();
            }
            else
            {
                LoadPlayerDataWithoutPos();
            }
            
        }
        else
        {
            
            if (sceneName != "MainMenu")
            {
                sanity = 1f;
                SaveSystem.CreatePlayerFile(this);
                SavePlayerData();
            }
            
        }

        /*if (sceneName != "FirstFloor")
        {
            FindObjectOfType<AudioManager>().StopMusic("Level1_Sanity1");
            FindObjectOfType<AudioManager>().StopMusic("Level1_Sanity2");
            FindObjectOfType<AudioManager>().StopMusic("Level1_Sanity3");
        } */

    }

    // Start is called before the first frame update
    void Start()
    {
        //sanity = 3f;
        if (currentDay < 1)
        {
            currentDay = 1;
        }

        if (currentLevel < 1)
        {
            currentLevel = 1;
        }

        // making sure level value is correct
        switch (sceneName)
        {
            case "FirstFloor":
                currentLevel = 1;
                break;
            case "SecondFloor":
                currentLevel = 2;
                break;
            case "ThirdFloor":
                currentLevel = 3;
                break;
            case "FirstFloor Night":
                currentLevel = 1;
                break;
            case "SecondFloor Night":
                currentLevel = 2;
                break;
            case "ThirdFloor Night":
                currentLevel = 3;
                break;
        }

        // temp cus i'm pretty sure the nighttime bool isn't set yet
        if (sceneName == "FirstFloor Night")
        {
            FindObjectOfType<AudioManager>().Play("Night2_Sanity1");
            FindObjectOfType<AudioManager>().Play("Night2_Sanity2");
            FindObjectOfType<AudioManager>().Play("Night2_Sanity3");

            FindObjectOfType<AudioManager>().ChangeVolume("Night2_Sanity2", 0f);
            FindObjectOfType<AudioManager>().ChangeVolume("Night2_Sanity3", 0f);
        }
        else if (sceneName == "Final Level")
        {
            FindObjectOfType<AudioManager>().Play("Final_Boss");
        }

        if (GetFlagBool("FirstCutscene_WakeUp"))
        {
            if (sceneName != "MainMenu")
            {
                FindObjectOfType<AudioManager>().Play("Level" + currentDay + "_Sanity1");
                FindObjectOfType<AudioManager>().Play("Level" + currentDay + "_Sanity2");
                FindObjectOfType<AudioManager>().Play("Level" + currentDay + "_Sanity3");

                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity2", 0f);
                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity3", 0f);
            }
            

            /*switch (sceneName)
            {
                case "FirstFloor":
                    FindObjectOfType<AudioManager>().Play("Level1_Sanity1");
                    break;
            } */
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sceneName != "MainMenu")
        {
            if (sanity == 0)
            {
                if (!didlose)
                {
                    Debug.Log("YOU LOSE DINGUS");
                    didlose = true;
                }
            }
        }
    }

    // bad to have in fixed update !!!!
    private void FixedUpdate()
    {
        if (sceneName != "MainMenu")
        {
            if (!isNight)
            {
                switch (sanity)
                {
                    case 1f:
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity1", 0.75f);
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity2", 0f);
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity3", 0f);
                        break;
                    case 0.7f:
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity1", 0f);
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity2", 0.75f);
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity3", 0f);
                        break;
                    case 0.4f:
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity1", 0f);
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity2", 0f);
                        FindObjectOfType<AudioManager>().ChangeVolume("Level" + currentDay + "_Sanity3", 0.75f);
                        break;
                }
            }
            else
            {
                switch (currentDay)
                {
                    case 1:
                        switch (sanity)
                        {
                            case 1f:
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity1", 0.75f);
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity2", 0f);
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity3", 0f);
                                break;
                            case 0.7f:
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity1", 0f);
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity2", 0.75f);
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity3", 0f);
                                break;
                            case 0.4f:
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity1", 0f);
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity2", 0f);
                                FindObjectOfType<AudioManager>().ChangeVolume("Night" + currentDay + "_Sanity3", 0.75f);
                                break;
                        }
                        break;
                }
            }
        }
    }



    // add picked up item to inventory
    public void PickUpItem(Item pickup)
    {
        // item notification
        GameObject notifObj = GameObject.Find("NotificationManager");
        Notifications notif = notifObj.GetComponent<Notifications>();

        // ui script ref
        GameObject uiObj = GameObject.Find("UIManager");
        UIScript ui = uiObj.GetComponent<UIScript>();

        // convert to itembase
        ItemBase itemPickup = new ItemBase(pickup);

        playerInventory.Add(itemPickup);
        notif.ShowItemNotif(pickup);
        ui.UpdateIconPreview();
    }

    // use selected item
    public void UseItem(ItemBase consumable)
    {
        // run anything related to the item 
        // ui script ref
        GameObject uiObj = GameObject.Find("UIManager");
        UIScript ui = uiObj.GetComponent<UIScript>();

        // then discard the item (if it isn't a key item
        switch (consumable.b_ItemID)
        {
            // cipher note
            case 3:
                // open cipher ui
                ui.OpenCipherNote();
                break;
            // photo frame
            case 4:
                ui.OpenPhotoFrameUI();
                break;
            // thank you note
            case 10:
                ui.OpenThankYouNote();
                break;

        }
    }

    // basically the same as the above except it skips the whole using the item bit
    public void DiscardItem(Item discard)
    {
        // ui script ref
        GameObject uiObj = GameObject.Find("UIManager");
        UIScript ui = uiObj.GetComponent<UIScript>();

        ItemBase itemDiscard = FindMatchingItemBase(discard);

        if (itemDiscard != null)
        {
            playerInventory.Remove(itemDiscard);
            //ui.UpdateIconPreview();
            ui.DeleteItemsOnUI();

        }
        else
        {
            Debug.LogWarning("This item isn't in the player's inventory");
        }
        /*
        if (discard.ItemMode == ItemType.NORMAL)
        {
            
            // ui script ref
            GameObject uiObj = GameObject.Find("UIManager");
            UIScript ui = uiObj.GetComponent<UIScript>();

            ItemBase itemDiscard = FindMatchingItemBase(discard);

            if (itemDiscard != null)
            {
                playerInventory.Remove(itemDiscard);
                //ui.UpdateIconPreview();
                ui.DeleteItemsOnUI();

            }
            else
            {
                Debug.LogWarning("This item isn't in the player's inventory");
            } 
        }
        else
        {
            Debug.LogWarning("Can't trash item");
        } */
    }

    public void RemoveAllItems()
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            playerInventory.Remove(playerInventory[i]);
        }

        // ui script ref
        GameObject uiObj = GameObject.Find("UIManager");
        UIScript ui = uiObj.GetComponent<UIScript>();
        ui.UpdateIconPreview();
    }

    // find the matching itembase to a specific item
    private ItemBase FindMatchingItemBase(Item ogItem)
    {
        for (int i = 0; i < playerInventory.Count; i++)
        {
            if (playerInventory[i].b_ItemID == ogItem.ItemID)
            {
                return playerInventory[i];
            }
        }

        return null;
    }

    // find the matching item to a specific itembase
    public Item FindMatchingItem(ItemBase itemBase)
    {
        var allItems = Resources.LoadAll("Items", typeof(Item)).Cast<Item>();

        foreach (var current in allItems)
        {
            if (current.ItemID == itemBase.b_ItemID)
            {
                return current;
            }
        }

        return null;
    }

    // sort quest list by ID order
    public void SortQuestList()
    {
        Debug.Log("Sorting quest");
        currentQuests.OrderBy(o => o.b_QuestID);
    }

    // look through list of flags to get the correct bool
    public bool GetFlagBool(string flagName)
    {
        for (int i = 0; i < playerFlags.Count; i++)
        {
            if (flagName.ToLower() == playerFlags[i].FlagName.ToLower())
            {
                //Debug.Log(playerFlags[i].FlagName + " is " + playerFlags[i].IsActive);
                return playerFlags[i].IsActive;
            }
        }

        // default to false if the flag name doesn't exist
        Debug.LogWarning(flagName + " could not be found, returning false as a fallback");
        return false;
    }

    // activate a specific flag
    public void SetFlagBool(string flagName)
    {
        for (int i = 0; i < playerFlags.Count; i++)
        {
            if (flagName.ToLower() == playerFlags[i].FlagName.ToLower())
            {
                Debug.Log("Set " + playerFlags[i].FlagName + " to true");
                playerFlags[i].IsActive = true;
            }
        }
    }

    // deactivate a specific flag
    public void DeactivateFlagBool(string flagName)
    {
        for (int i = 0; i < playerFlags.Count; i++)
        {
            if (flagName.ToLower() == playerFlags[i].FlagName.ToLower())
            {
                Debug.Log("Set " + playerFlags[i].FlagName + " to false");
                playerFlags[i].IsActive = false;
            }
        }
    }

    // check current quests to see if a specific quest is in it
    public bool IsQuestActive(int id)
    {
        for (int i = 0; i < currentQuests.Count; i++) 
        {
            if (id == currentQuests[i].b_QuestID)
            {
                return true;
            }
        }

        return false;
    }

    // check if specific quest is complete
    public bool IsQuestComplete(int id)
    {
        for (int i = 0; i < completedQuests.Count; i++)
        {
            if (id == completedQuests[i].b_QuestID)
            {
                return true;
            }
        }

        return false;
    }


    // check if a specific item is in the player's inventory
    public bool HasItem(int itemID)
    {
        for (int i = 0; i < playerInventory.Count; i++) 
        {
            if (itemID == playerInventory[i].b_ItemID)
            {
                return true;
            }
        }

        return false;
    }

    public void SavePlayerData()
    {
        // set xy values
        playerX = transform.position.x;
        playerY = transform.position.y;

        Debug.Log("Saving");
        SaveSystem.SaveGame(this);
    }

    // saving without positions
    public void SavePlayerDataWithoutPos()
    {
        Debug.Log("Saving");
        SaveSystem.SaveGame(this);
    }

    // loading all data except position data
    public void LoadPlayerDataWithoutPos()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        playerInventory = data.playerInventory;
        sanity = data.sanity;
        currentLevel = data.currentLevel;
        charactersMet = data.charactersMet;
        currentQuests = data.currentQuests;
        completedQuests = data.completedQuests;
        changingFloor = data.changingFloor;
        eventTokens = data.eventTokens;
        currentDay = data.currentDay;
        isNight = data.isNight;
        playerFlags = data.playerFlags;
        money = data.money;
    }

    public void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        playerInventory = data.playerInventory;
        sanity = data.sanity;
        currentLevel = data.currentLevel;
        charactersMet = data.charactersMet;
        currentQuests = data.currentQuests;
        completedQuests = data.completedQuests;
        changingFloor = data.changingFloor;
        eventTokens = data.eventTokens;
        if (sceneName != "MainMenu" && !changingFloor)
        {
            transform.position = new Vector2(data.playerX, data.playerY);
        }
        currentDay = data.currentDay;
        isNight = data.isNight;
        playerFlags = data.playerFlags;
        money = data.money;
        
    }
}
