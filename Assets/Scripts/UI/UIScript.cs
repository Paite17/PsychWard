using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuState
{
    NONE,
    DEFAULT,
    QUEST,
    CHARACTER,
    INVENTORY,
    SETTINGS,
    KEYPAD,
    PHOTO_FRAME,
    QUIT_PROMPT,
    CIPHER_NOTE,
    THANKS_NOTE,
    ITEM_STORAGE
}
public class UIScript : MonoBehaviour
{
    // holds the icons for items on the preview
    [SerializeField] private List<Image> invPreviewSprites;

    // submenu gameobjects
    [SerializeField] private GameObject questSubMenu;
    [SerializeField] private GameObject charSubMenu;
    [SerializeField] private GameObject invSubMenu;
    [SerializeField] private GameObject settingsSubMenu;

    // The default gameobjects that the game should highlight upon activating the menu
    [SerializeField] private GameObject defMenuObj;
    [SerializeField] private GameObject defQuestObj;
    [SerializeField] private GameObject defStickyObj;
    [SerializeField] private GameObject defInvObj;
    [SerializeField] private GameObject defSettingsObj;

    // event system
    [SerializeField] private EventSystem eventSystem;

    // keypad puzzle related UI elements
    [SerializeField] GameObject keyPadTextField;
    [SerializeField] private Text keyPadInput;
    [SerializeField] private GameObject keypadUI;
    [SerializeField] private GameObject photoFrameUI;
    [SerializeField] private GameObject cipherNote;
    [SerializeField] private GameObject thanksNote;

    // Quest UI elements
    [SerializeField] private Transform questContentContainer;
    [SerializeField] private GameObject questTemplate;


    // Inventory UI elements
    [SerializeField] private List<Image> itemPreviews;
    [SerializeField] private Sprite missingIcon;
    [SerializeField] private Sprite noItem;
    [SerializeField] private Text pageNumberText;
    [SerializeField] private List<Image> invItemIcons;
    [SerializeField] private List<GameObject> invItemButtons; // the icons for the items themselves are a child of the button objects, so i gotta do this

    // Inventory UI item info popup box
    [SerializeField] private GameObject itemPopup;
    [SerializeField] private Text popup_ItemName;
    [SerializeField] private Text popup_ItemDescription;

    

    // inventory values
    private int amountOfPages = 1;
    [SerializeField] private List<int> savedIndex;
    private int maxIndex;
    private const int NUM_OF_ELEMENTS_PER_PAGE = 8;
    private int currentPage;

    
    [SerializeField] private string keypadPassword;
    [SerializeField] private Player player;

    // quit prompt
    [SerializeField] private GameObject quitPrompt;
    [SerializeField] private GameObject defPromptObj;

    // all main overlay buttons
    [SerializeField] private List<Button> overlayButtons;

    // settings
    [SerializeField] private AudioMixer mixer;

    // the sanity bar
    [SerializeField] private Image sanityBar;

    // item storage
    [SerializeField] private GameObject storageUI;
    //[SerializeField] private List<Image> storedItemIcons;
    [SerializeField] private List<Button> storedItemButtons;
    [SerializeField] private StorageBox itemStorageBox;
    [SerializeField] private GameObject storagePrompt;
    [SerializeField] private GameObject storagePromptDefObj;
    [SerializeField] private GameObject storageAddItemWindow;
    [SerializeField] private Transform storageContentContainer;
    [SerializeField] private GameObject itemTemplate;
    
    public MenuState currentState;

    // bool for checking if the player is allowed to open the menu (will probably be false during scripted events)
    public bool canUseMenu = true;

    private bool hasLoadedBefore = false;
    private bool storage_hasLoadedBefore = false;

    // for a specific UI thing
    [SerializeField] private int indexOfCurrentItem;

    [SerializeField] private GameObject useMouseIndicator;

    // an object that signifies that the menu is currently active and selected
    [SerializeField] private GameObject menuActiveObj;

    private void Start()
    {
        DisableOverlayButtons();
        //eventSystem.SetSelectedGameObject(null);
        currentState = MenuState.NONE;
        UpdateIconPreview();
        currentPage = 1;
        savedIndex.Add(0);

        // yuck hard coded 
        if (player.sceneName == "Floor1")
        {
            storedItemButtons[0].GetComponent<InvItemAssign>().currentItem = null;
            storedItemButtons[1].GetComponent<InvItemAssign>().currentItem = null;
            storedItemButtons[2].GetComponent<InvItemAssign>().currentItem = null;
        }
        
    }

    public void OpenQuestMenu()
    {
        currentState = MenuState.QUEST;
        questSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defQuestObj);
        LoadQuestMenuContent();
    }

    public void OpenCharProfileMenu()
    {
        currentState = MenuState.CHARACTER;
        charSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defStickyObj);
    }

    public void OpenInventoryMenu()
    {
        if (player.playerInventory.Count > 0)
        {
            itemPopup.SetActive(true);
        }
        
        currentState = MenuState.INVENTORY;
        invSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defInvObj);
        LoadInventoryIcons();
    }

    // finds how many pages are needed depending on how many items you have
    private int CalculateNumberOfPages(int totalItems, int itemsPerPage)
    {
        int numberOfPages = (int)Mathf.Ceil((float)totalItems / itemsPerPage);
        return numberOfPages;
    }

    // Set icons for inventory submenu
    private void LoadInventoryIcons()
    {
        if (player.playerInventory.Count <= 8)
        {
            amountOfPages = 1;
        }
        else
        {
            amountOfPages = CalculateNumberOfPages(player.playerInventory.Count, NUM_OF_ELEMENTS_PER_PAGE);

        }

        // set inital page text
        pageNumberText.text = "Page " + currentPage + " of " + amountOfPages;

        int killMe = 0;

        // maxIndex will be used in the loop as if the player inventory
        if (player.playerInventory.Count <= NUM_OF_ELEMENTS_PER_PAGE)
        {
            maxIndex = player.playerInventory.Count - 1;
            killMe = player.playerInventory.Count;
        }
        else
        {
            //maxIndex = NUM_OF_ELEMENTS_PER_PAGE * currentPage;
            maxIndex = player.playerInventory.Count;
            killMe = NUM_OF_ELEMENTS_PER_PAGE;
        }

        
        int saving = NUM_OF_ELEMENTS_PER_PAGE * (amountOfPages - 1);
        //int g = savedIndex[currentPage - 1];
        int iconPointer = 0;
        // CODE REWRITE TIME !!!!
        if (player.playerInventory.Count > NUM_OF_ELEMENTS_PER_PAGE)
        {
            // loop for when the player has more than 8 items
            for (int i = savedIndex[currentPage - 1]; i < maxIndex; i++)
            {
                // i need a better way of checking this, as the loop will try to go further than the amount of items in the list
                if (player.playerInventory[i] != null)
                {
                    
                    invItemButtons[iconPointer].GetComponent<InvItemAssign>().currentItem = player.playerInventory[i];

                    // check if icon exists before applying it
                    if (player.playerInventory[i].GetIconByID() != null)
                    {
                        invItemButtons[iconPointer].GetComponent<InvItemAssign>().SetImageIconAsItem();
                    }
                    else
                    {
                        invItemIcons[i].sprite = noItem;
                    }

                    if (iconPointer < 7)
                    {
                        iconPointer++;
                    }
                }
                else
                {
                    invItemButtons[i].GetComponent<InvItemAssign>().currentItem = null;
                }
                Debug.Log(i);
                Debug.Log("icon index = " + iconPointer);
            }
            iconPointer = 0;
        }
        else
        {
            // slightly adjusted loop for when the player has 8 or less items
            for (int i = 0; i < NUM_OF_ELEMENTS_PER_PAGE; i++)
            {
                if (player.playerInventory[i] != null)
                {
                    invItemButtons[i].GetComponent<InvItemAssign>().currentItem = player.playerInventory[i];

                    // check if icon exists before applying it
                    if (player.playerInventory[i].GetIconByID() != null)
                    {
                        invItemButtons[i].GetComponent<InvItemAssign>().SetImageIconAsItem();
                    }
                    else
                    {
                        invItemIcons[i].sprite = noItem;
                    }
                }
                else
                {
                    invItemButtons[i].GetComponent<InvItemAssign>().currentItem = null;
                }
            }
        }
        
        
        
        // apply item icons to each space on the menu
        // i feel bad for anyone that has to look at this mess, my brain is in pain
        /*for (int i = savedIndex[currentPage - 1]; i < maxIndex; i++)
        {
            Debug.Log("i = " + i);
            Debug.Log("killMe = " + killMe);
            Debug.Log("maxIndex = " + maxIndex);

            for (int j = 0; j < killMe; j++)
            {

                Debug.Log("j = " + j);
                Debug.Log("g = " + g);
                // so much nesting oh god
                if (player.playerInventory.Count > 0)
                {
                    if (g < player.playerInventory.Count)
                    {
                        if (player.playerInventory[g] != null)
                        {
                            if (player.playerInventory[g].GetIconByID() != null)
                            {
                                invItemIcons[j].sprite = player.playerInventory[g].GetIconByID();
                            }
                            else
                            {
                                invItemIcons[j].sprite = noItem;
                            }

                        }
                    } 

                    if (player.playerInventory.Count > 0)
                    {
                        if (g < player.playerInventory.Count)
                        {
                            if (player.playerInventory[g] != null)
                            {
                                invItemButtons[j].GetComponent<InvItemAssign>().currentItem = player.playerInventory[g];
                                invItemButtons[j].GetComponent<InvItemAssign>().SetImageIconAsItem();
                            }
                            else
                            {
                                invItemButtons[j].GetComponent<InvItemAssign>().currentItem = null;
                            }
                        }
                    }
                    g++;
                    //saving = i;
                } 

            }
            

            Debug.Log("savedIndex = " + savedIndex);

        }*/

        savedIndex.Add(saving);
    }

    // using an item
    public void UseItem()
    {
        if (eventSystem.currentSelectedGameObject.GetComponent<InvItemAssign>().currentItem != null)
        {
            player.UseItem(eventSystem.currentSelectedGameObject.GetComponent<InvItemAssign>().currentItem);
        }
        else
        {
            Debug.Log("No item on this slot");
        }
        
    }

    // upon loading the quest sub menu
    public void LoadQuestMenuContent()
    {
        if (hasLoadedBefore)
        {
            // get all quest objs
            FunnyMoment[] allObjs = FindObjectsOfType<FunnyMoment>();

            foreach (var obj in allObjs)
            {
                Destroy(obj.gameObject);
            }

            hasLoadedBefore = false;
        }

        for (int i = 0; i < player.currentQuests.Count; i++)
        {
            if (player.currentQuests.Count > 0)
            {
                var quest_obj = Instantiate(questTemplate);
                // did not know GetComponentInChildren existed before this one forum post i found that would've been nice to know earlier
                quest_obj.GetComponentInChildren<Text>().text = player.currentQuests[i].b_QuestName;

                // set parent
                quest_obj.transform.SetParent(questContentContainer);

                // reset scale
                quest_obj.transform.localScale = Vector2.one;
            }
            else
            {
                return;
            }

        }

        if (player.currentQuests.Count > 0)
        {
            hasLoadedBefore = true;
        }
    }

    // called when pressing the arrow buttons
    // if bool is true then we're changing to the next page
    public void ChangePage(bool nextPage)
    {
        if (amountOfPages > 1)
        {
            if (nextPage)
            {
                if (currentPage < amountOfPages)
                {
                    currentPage++;
                    LoadInventoryIcons();
                }
            }
            else
            {
                if (currentPage > 1)
                {
                    currentPage--;
                    LoadInventoryIcons();
                }
            }

            if (currentPage == 0)
            {
                currentPage = 1;
            }

        }
        
    }

    // open the item box menu
    public void OpenStorageUI()
    {
        storageUI.SetActive(true);
        storedItemButtons[0].interactable = true;
        storedItemButtons[1].interactable = true;
        storedItemButtons[2].interactable = true;
        currentState = MenuState.ITEM_STORAGE;
        eventSystem.SetSelectedGameObject(storedItemButtons[0].gameObject);

        // grab icons
        for (int i = 0; i < itemStorageBox.storedItems.Count; i++)
        {
            if (itemStorageBox.storedItems.Count > 0)
            {
                storedItemButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = itemStorageBox.storedItems[i].GetIconByID();
                storedItemButtons[i].GetComponent<InvItemAssign>().currentItem = itemStorageBox.storedItems[i];
            }
        }
    }

    

    // when you press enter on one of the item slots on the storage menu
    public void SelectItemSlot()
    {
        if (eventSystem.currentSelectedGameObject.GetComponent<InvItemAssign>().currentItem != null)
        {
            StorageItemReturnPrompt();
        }
        else
        {
            StorageNoItemPrompt();
        }
    }

    // the little window that lets you put an item into the storage box
    public void StorageNoItemPrompt()
    {
        storageAddItemWindow.SetActive(true);
        storedItemButtons[0].interactable = false;
        storedItemButtons[1].interactable = false;
        storedItemButtons[2].interactable = false;

        if (storage_hasLoadedBefore)
        {
            FunnyMoment2[] allObjs_ = FindObjectsOfType<FunnyMoment2>();

            foreach (var obj in allObjs_)
            {
                Destroy(obj.gameObject);
            }

            storage_hasLoadedBefore = false;
        }

        for (int i = 0; i < player.playerInventory.Count; i++)
        {
            if (player.playerInventory.Count > 0)
            {
                var item_obj = Instantiate(itemTemplate);

                // hope this works lolol
                item_obj.GetComponent<Button>().onClick.AddListener(delegate { AddItemToStorage(); });

                // set name
                item_obj.GetComponentInChildren<Text>().text = player.playerInventory[i].b_ItemName;

                // item icon
                //item_obj.GetComponentInChildren<Image>().sprite = player.playerInventory[i].GetIconByID();
                item_obj.transform.GetChild(1).GetComponent<Image>().sprite = player.playerInventory[i].GetIconByID();

                // set parent
                item_obj.transform.SetParent(storageContentContainer);

                // reset scale
                item_obj.transform.localScale = Vector2.one;
            }
            else
            {
                return;
            }


        }
        FunnyMoment2[] allObjs = FindObjectsOfType<FunnyMoment2>();
        if (player.playerInventory.Count > 0)
        {
            eventSystem.SetSelectedGameObject(allObjs[0].gameObject);
        }

        storage_hasLoadedBefore = true;

    }

    // the window prompt that lets you get back your item from storage
    public void StorageItemReturnPrompt()
    {
        storagePrompt.SetActive(true);
        
        // wow
        indexOfCurrentItem = storedItemButtons.IndexOf(eventSystem.currentSelectedGameObject.GetComponent<Button>());
        eventSystem.SetSelectedGameObject(storagePromptDefObj);
    }

    // upon selecting an item to put in storage (on the sub-submenu)
    public void AddItemToStorage()
    {
        if (itemStorageBox.storedItems.Count < itemStorageBox.storageLimit)
        {
            ItemBase currentItem = null;
            // get item
            // not great way of doing it
            for (int i = 0; i < player.playerInventory.Count; i++)
            {
                if (eventSystem.currentSelectedGameObject.GetComponentInChildren<Text>().text == player.playerInventory[i].b_ItemName)
                {
                    currentItem = player.playerInventory[i];
                }
            }

            itemStorageBox.AddItemToStorage(currentItem, player);
            // what the hell is this
            player.DiscardItem(player.FindMatchingItem(currentItem));

            // close menu
            itemStorageBox.SaveStorage();
            eventSystem.SetSelectedGameObject(null);
            currentState = MenuState.NONE;
            storagePrompt.SetActive(false);
            storageAddItemWindow.SetActive(false);
            storageUI.SetActive(false);
            DeleteItemsOnUI();

            player.GetComponent<PlayerMovement>().canMove = true;

            player.SavePlayerData();
            itemStorageBox.SaveStorage();
        }
        else
        {
            // idk play a sound or something
            return;
        }
        
    }

    // pressing no on the prompt
    public void ReturnPromptNo()
    {
        storagePrompt.SetActive(false);
        eventSystem.SetSelectedGameObject(storedItemButtons[0].gameObject);
    }

    // pressing yes on the prompt
    public void ReturnPromptYes()
    {
        itemStorageBox.RemoveItemFromStorage(storedItemButtons[indexOfCurrentItem].gameObject.GetComponent<InvItemAssign>().currentItem, player);
        Debug.Log(storedItemButtons[indexOfCurrentItem].transform.GetChild(0).name);
        player.SavePlayerData();
        itemStorageBox.SaveStorage();
        storagePrompt.SetActive(false);
        UpdateIconPreview();
        OpenStorageUI();
        storedItemButtons[indexOfCurrentItem].transform.GetChild(0).GetComponent<Image>().sprite = noItem;
        storedItemButtons[indexOfCurrentItem].GetComponent<InvItemAssign>().currentItem = null;
    }

    // fixes a thing where items, when deleted from the inventory, would still show their icon and stats
    public void DeleteItemsOnUI()
    {
        // item preview
        for (int i = 0; i < itemPreviews.Count; i++)
        {
            itemPreviews[i].sprite = noItem;
        }

        UpdateIconPreview();

        // inv icons
        for (int i = 0; i < invItemIcons.Count; i++)
        {
            invItemIcons[i].sprite = noItem;
            invItemButtons[i].GetComponent<InvItemAssign>().currentItem = null;
        }

        LoadInventoryIcons();
    }

    // item popup stuff
    private void SetItemPopup()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            // position
            itemPopup.transform.position = new Vector2(eventSystem.currentSelectedGameObject.transform.position.x + 280, eventSystem.currentSelectedGameObject.transform.position.y + 100);

            // check if the current object isn't the arrows for changing page (cus apparently thats a problem)
            if (eventSystem.currentSelectedGameObject.name != "NextPageButton" && eventSystem.currentSelectedGameObject.name != "LastPageButton")
            {
                itemPopup.SetActive(true);
                // this is gonna look messy as hell but at least it should work
                if (eventSystem.currentSelectedGameObject.GetComponent<InvItemAssign>().currentItem != null)
                {
                    popup_ItemName.text = eventSystem.currentSelectedGameObject.GetComponent<InvItemAssign>().currentItem.b_ItemName;
                    popup_ItemDescription.text = eventSystem.currentSelectedGameObject.GetComponent<InvItemAssign>().currentItem.b_ItemDescription;
                }
            }
            else
            {
                itemPopup.SetActive(false);
            }
            
            
        }
    }

    public void OpenCipherNote()
    {
        cipherNote.SetActive(true);
        currentState = MenuState.CIPHER_NOTE;
    }

    public void OpenThankYouNote()
    {
        thanksNote.SetActive(true);
        currentState = MenuState.THANKS_NOTE;
    }


    private void FixedUpdate()
    {
        if (currentState == MenuState.INVENTORY)
        {
            if (player.playerInventory.Count > 0)
            {
                SetItemPopup();
            }
        }

        if (currentState != MenuState.NONE)
        {
            menuActiveObj.SetActive(true);
        }
        else
        {
            menuActiveObj.SetActive(false);
        }

        sanityBar.fillAmount = player.sanity;
    }

    public void QuitGame()
    {
        // set selected to one of the promps
        quitPrompt.SetActive(true);
        eventSystem.SetSelectedGameObject(defPromptObj);
        currentState = MenuState.QUIT_PROMPT;
        //Application.Quit();
    }

    // actual quit button (on prompt)
    public void SaveAndQuit()
    {
        player.SavePlayerData();
        FindObjectOfType<AudioManager>().StopMusic("Level" + player.currentDay + "_Sanity1");
        FindObjectOfType<AudioManager>().StopMusic("Level" + player.currentDay + "_Sanity2");
        FindObjectOfType<AudioManager>().StopMusic("Level" + player.currentDay + "_Sanity3");
        FindObjectOfType<AudioManager>().StopMusic("Level" + player.currentDay + "_Sanity1");
        FindObjectOfType<AudioManager>().StopMusic("Level" + player.currentDay + "_Sanity2");
        FindObjectOfType<AudioManager>().StopMusic("Level" + player.currentDay + "_Sanity3");
        FindObjectOfType<AudioManager>().Play("Main_Menu");
        SceneManager.LoadScene("MainMenu");
        //Application.Quit();
    }

    // pressing no on the save&quit prompt
    public void GoBackPrompt()
    {
        // close menu
        quitPrompt.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        currentState = MenuState.NONE;
        DisableOverlayButtons();

        player.GetComponent<PlayerMovement>().canMove = true;

    }

    public void OpenSettingsMenu()
    {
        currentState = MenuState.SETTINGS;
        settingsSubMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(defSettingsObj);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        switch (currentState)
        {
            case MenuState.NONE:
                // activating menu
                if (canUseMenu)
                {
                    EnableOverlayButtons();
                    // set the menu object as active
                    eventSystem.SetSelectedGameObject(defMenuObj);


                    // stop player from moving
                    player.GetComponent<PlayerMovement>().canMove = false;

                    currentState = MenuState.DEFAULT;

                    // play a sfx when it exists
                }
                else
                {
                    // when the menu isn't openable just do nothing
                    return;
                }

                break;
            case MenuState.QUEST:
                ResetMenu(questSubMenu);
                break;
            case MenuState.CHARACTER:
                ResetMenu(charSubMenu);
                break;
            case MenuState.INVENTORY:
                ResetMenu(invSubMenu);
                break;
            case MenuState.SETTINGS:
                ResetMenu(settingsSubMenu);
                break;
            case MenuState.DEFAULT:
                // close menu
                eventSystem.SetSelectedGameObject(null);
                currentState = MenuState.NONE;
                DisableOverlayButtons();

                player.GetComponent<PlayerMovement>().canMove = true;

                break;
            case MenuState.KEYPAD:
                // close menu (again)
                eventSystem.SetSelectedGameObject(null);
                currentState = MenuState.NONE;
                keypadUI.SetActive(false);

                player.GetComponent<PlayerMovement>().canMove = true;
                break;
            case MenuState.PHOTO_FRAME:
                // close menu (again again)
                useMouseIndicator.SetActive(false);
                ResetMenu(invSubMenu);
                eventSystem.SetSelectedGameObject(null);
                currentState = MenuState.NONE;
                photoFrameUI.gameObject.SetActive(false);

                player.GetComponent<PlayerMovement>().canMove = true;
                break;
            case MenuState.QUIT_PROMPT:
                ResetMenu(quitPrompt);
                break;
            case MenuState.CIPHER_NOTE:
                // close menu (again again again)
                // plus get rid of inventory
                ResetMenu(invSubMenu);
                eventSystem.SetSelectedGameObject(null);
                currentState = MenuState.NONE;
                cipherNote.SetActive(false);

                player.GetComponent<PlayerMovement>().canMove = true;
                break;
            case MenuState.THANKS_NOTE:
                // close menu (again again again again)
                // might need to make a generic 'piece of paper UI' state instead
                // plus get rid of inventory
                ResetMenu(invSubMenu);
                eventSystem.SetSelectedGameObject(null);
                currentState = MenuState.NONE;
                thanksNote.SetActive(false);

                player.GetComponent<PlayerMovement>().canMove = true;
                break;
            case MenuState.ITEM_STORAGE:
                // close menu (again again again again again)
                // god it never ends
                // plus get rid of inventory
                eventSystem.SetSelectedGameObject(null);
                currentState = MenuState.NONE;
                storagePrompt.SetActive(false);
                storageAddItemWindow.SetActive(false);
                storageUI.SetActive(false);
                
                player.GetComponent<PlayerMovement>().canMove = true;
                break;

        }
    }


    // for exiting submenus
    private void ResetMenu(GameObject submenu)
    {
        // return to default
        eventSystem.SetSelectedGameObject(defMenuObj);
        submenu.SetActive(false);
        currentState = MenuState.DEFAULT;
    }

    public void KeypadSubmit(string keypadPass)
    {
        if (keypadPass.ToLower() == keypadPassword.ToLower())
        {
            Debug.Log("Keypad correct");
            // maybe make a check later for different keypad events
            GameObject questManager = GameObject.Find("QuestManager");
            QuestManager qm = questManager.GetComponent<QuestManager>();
            keyPadInput.color = Color.green;
            qm.gameObject.GetComponent<GogglesQuest>().GiveQuest();
            qm.gameObject.GetComponent<GogglesQuest>().GiveBattery();
            qm.gameObject.GetComponent<GogglesQuest>().SetRollUnderTrigger();

            player.SetFlagBool("KeypadUsed");
            CloseKeypad();
        }
        else
        {
            keyPadInput.color = Color.red;
            eventSystem.SetSelectedGameObject(keyPadTextField);
            Debug.Log("Incorrect pass");
        }
    }

    // call this whenever the keypad needs to be focused on
    public void OpenKeypad()
    {
        if (!player.GetFlagBool("KeypadUsed"))
        {
            keypadUI.SetActive(true);
            eventSystem.SetSelectedGameObject(keyPadTextField);
            currentState = MenuState.KEYPAD;
        }
        
    }

    public void CloseKeypad()
    {
        player.GetComponent<PlayerMovement>().used = false;
        keypadUI.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
        currentState = MenuState.NONE;
    }

    // call whenever item exists
    public void UpdateIconPreview()
    {
        // update item previews
        for (int i = 0; i < player.playerInventory.Count; i++)
        {
            if (player.playerInventory[i] != null)
            {
                Debug.Log(player.playerInventory[i].b_ItemName);
                Sprite itemIcon = player.playerInventory[i].GetIconByID();
                if (itemIcon!= null)
                {
                    if (i < 4)
                    {
                        itemPreviews[i].sprite = itemIcon;
                    }
                    
                }
                else
                {
                    // missing icon
                    itemPreviews[i].sprite = missingIcon;
                }
            }
            else
            {
                itemPreviews[i].sprite = noItem;
            }
            
        }
    }

    public void OpenPhotoFrameUI()
    {
        useMouseIndicator.SetActive(true);
        photoFrameUI.SetActive(true);
        currentState = MenuState.PHOTO_FRAME;
    }

    // when in dialogue or something 
    private void DisableOverlayButtons()
    {
        for (int i = 0; i < overlayButtons.Count; i++)
        {
            overlayButtons[i].interactable = false;
        }
    }

    // same but in reverse
    private void EnableOverlayButtons()
    {
        for (int i = 0; i < overlayButtons.Count; i++)
        {
            overlayButtons[i].interactable = true;
        }
    }

    // temp fullscreen toggle
    public void FullscreenToggle(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    // pretty much just for the indicator
    public void CutleryPuzzleActivate()
    {
        useMouseIndicator.SetActive(true);
    }

    public void SetAudioLevel(float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
}
