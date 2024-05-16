using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator guardAnim;
    [SerializeField] private PlayerMovement plrMove;
    [SerializeField] private Animator fadeOut;
    [SerializeField] private GameObject cutleryQuestTrigger;
    [SerializeField] private GameObject getFoodTrigger;
    [SerializeField] private GameObject securityVisitorDialogueTrigger;
    [SerializeField] private GameObject teleportToVisitors;
    [SerializeField] private GameObject MickeyVNoQuest;
    [SerializeField] private GameObject secondGuard;
    [SerializeField] private GameObject visitorsEntrance;
    [SerializeField] private Animator guard2Anim;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private GameObject day1EndofDayDialogue;
    [SerializeField] private GameObject roomInspectionDialogue;
    [SerializeField] private GameObject roomInspectionItemsDialogue;
    [SerializeField] private GameObject roomInspectionNoItemsDialogue;
    [SerializeField] private GameObject RoomInspectionFinishingDialogue;
    [SerializeField] private Transform gogglesRoomPos;

    // items
    [SerializeField] private GameObject grapeItem;
    [SerializeField] private GameObject photoItem;

    private Scene currentScene;
    private string sceneName;


    public void StartFirstHallwayCutscene()
    {
        GameObject target = GameObject.Find("AllowInteraction");
        float x = target.transform.position.x - guardAnim.transform.position.x;
        float y = target.transform.position.y - guardAnim.transform.position.y;

        Vector3 pathway = new Vector3(x, 0, 0);

        pathway = pathway.normalized;

        guardAnim.SetBool("Start", true);
        playerAnim.GetComponent<Player>().SetFlagBool("HallwayGuardCutsceneActive");
        guardAnim.gameObject.GetComponent<CutsceneSGMovement>().pathway = pathway;
        guardAnim.gameObject.GetComponent<CutsceneSGMovement>().activate = true;
    }


    // level 1 - doing an action in the kitchen
    // call this when either: do cutlery quest, talk to goggles or talk to dorothy
    
    public void SetVisitorsCutsceneReady()
    {
        // set flag based on which was done
        if (playerAnim.gameObject.GetComponent<Player>().HasItem(3))
        {
            playerAnim.gameObject.GetComponent<Player>().SetFlagBool("Level1_OnGogglesRoute");
        }

        // cutlery quest
        if (playerAnim.gameObject.GetComponent<Player>().IsQuestComplete(0))
        {
            playerAnim.gameObject.GetComponent<Player>().SetFlagBool("Level1_MickyVQuestRoute");
        }

        // dorothy quest
        if (playerAnim.gameObject.GetComponent<Player>().IsQuestActive(1))
        {
            playerAnim.gameObject.GetComponent<Player>().SetFlagBool("Level1_OnDorothyRoute");
            // temp
            GameObject gogglesObj = GameObject.Find("GogglesKitchen");
            gogglesObj.GetComponent<BoxCollider2D>().enabled = false;
        }

        // if the myles dialogue has already happened then we can just have the next event run instead
        if (!playerAnim.gameObject.GetComponent<Player>().GetFlagBool("TalkedToMyles1"))
        {
            securityVisitorDialogueTrigger.transform.position = playerAnim.transform.position;
        }
        

        
    }

    // another very specific thing
    public void MoveRoomInspTrigger()
    {
        // move the intercom dialogue to goggles' room
        day1EndofDayDialogue.transform.position = gogglesRoomPos.position;
    }

    // very specific function to solve a very specific problem
    public void ReEnableDororthyNPC()
    {
        if (!playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnDorothyRoute"))
        {
            if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_MickyVQuestRoute"))
            {
                GameObject dorothyObj = GameObject.Find("DorothyQuestStart");
                dorothyObj.GetComponent<BoxCollider2D>().enabled = true;
            }
            
        }
    }

    public void DisableNPCS()
    {
        // TODO: check for current level (as with a lot of this stuff)
        // disable box colliders so that you can't interact with the npcs
        // level 1
        // this prevents an error if you talk to dorothty
        if (!playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnDorothyRoute"))
        {
            GameObject dorothyObj = GameObject.Find("DorothyQuestStart");
            if (dorothyObj != null)
            {
                dorothyObj.GetComponent<BoxCollider2D>().enabled = false;
            }
            
        }
        
        GameObject gogglesObj = GameObject.Find("GogglesKitchen");
        if (gogglesObj != null)
        {
            gogglesObj.GetComponent<BoxCollider2D>().enabled = false;
        }
        

        if (!playerAnim.gameObject.GetComponent<Player>().GetFlagBool("TalkedToCutleryQuest"))
        {
            GameObject mickeyObj = GameObject.Find("MickyCutleryQuest");
            if (mickeyObj != null)
            {
                mickeyObj.GetComponent<BoxCollider2D>().enabled = false;
            }
            
        }
        
    }

    // After finishing one of the quests after visiting myles
    // starts dialogue for room inspection announcement
    public void RoomInspectionInitialise()
    {
        // this is for the cutlery quest
        if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("TalkedToMyles1"))
        {
            // check to make sure the cutlery quest is done
            // this is bad
            if (playerAnim.gameObject.GetComponent<Player>().IsQuestComplete(0))
            {
                //day1EndofDayDialogue.transform.position = playerAnim.transform.position;
                day1EndofDayDialogue.SetActive(true);
                playerAnim.GetComponent<Player>().SetFlagBool("RoomInspectionTrigger");

                // place dialogue trigger in room (Make sure the game removes it when this is false)
                roomInspectionDialogue.gameObject.SetActive(true);
            }
            else if (playerAnim.gameObject.GetComponent<Player>().IsQuestComplete(1))
            {
                //day1EndofDayDialogue.transform.position = playerAnim.transform.position;
                day1EndofDayDialogue.SetActive(true);
                playerAnim.GetComponent<Player>().SetFlagBool("RoomInspectionTrigger");

                // place dialogue trigger in room (Make sure the game removes it when this is false)
                roomInspectionDialogue.gameObject.SetActive(true);
            }
            else if (playerAnim.gameObject.GetComponent<Player>().IsQuestComplete(2))
            {
                //day1EndofDayDialogue.transform.position = playerAnim.transform.position;
                day1EndofDayDialogue.SetActive(true);
                playerAnim.GetComponent<Player>().SetFlagBool("RoomInspectionTrigger");

                // place dialogue trigger in room (Make sure the game removes it when this is false)
                roomInspectionDialogue.gameObject.SetActive(true);
            }
        }
        else
        {
            // check the other two just in case (which also allows nothing to happen if it was accidentally called when not supposed to)
            if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnDorothyRoute") || playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnGogglesRoute"))
            {
                //day1EndofDayDialogue.transform.position = playerAnim.transform.position;
                day1EndofDayDialogue.SetActive(true);
                playerAnim.gameObject.GetComponent<Player>().SetFlagBool("RoomInspectionTrigger");

                // place dialogue trigger in room
                roomInspectionDialogue.gameObject.SetActive(true);
                
            }
        }
    }

    // when the player walks into their room again run this
    
    public void StartRoomInspection()
    {
        // check if the player has items
        if (playerAnim.gameObject.GetComponent<Player>().playerInventory.Count > 0)
        {
            // set the contraband dialogue dialogue

            roomInspectionItemsDialogue.transform.position = playerAnim.transform.position;
            roomInspectionDialogue.SetActive(false);

        }
        else
        {
            // the other one

            roomInspectionNoItemsDialogue.transform.position = playerAnim.transform.position;
            roomInspectionDialogue.SetActive(false);
        }
    }

    // call this at the end of the room inspection if player has items
    public void RemoveAllItems()
    {
        playerAnim.GetComponent<Player>().RemoveAllItems();
        //RoomInspectionEndingDialogue();
    }

    public void RoomInspectionEndingDialogue()
    {
        RoomInspectionFinishingDialogue.transform.position = playerAnim.transform.position;
        // find object and move it onto player
        playerAnim.gameObject.GetComponent<Player>().DeactivateFlagBool("RoomInspectionTrigger");
    }

    // call this at the end of a day
    public void EndDaySequence()
    {
        playerAnim.GetComponent<Player>().currentDay++;

        // loading screen (when it exists)
        // maybe have a cutscene thing while its loading
        // load scene for next day
        switch (playerAnim.GetComponent<Player>().currentDay)
        {
            case 2:
                // THESE ARE COMMENTED OUT FOR THE DEMO
                playerAnim.GetComponent<Player>().SavePlayerData();
                //FindObjectOfType<LoadingManager>().StartLoadingArea("Final Level");
                SceneManager.LoadScene("FirstFloor Night");
                break;
        }
    }

    public void StartSecondHallwayCutscene()
    {
        GameObject trigger = GameObject.Find("AllowInteraction");
        trigger.transform.position = new Vector3(11.2f, -12.8f, 0);
        visitorsEntrance.SetActive(true);
        GameObject target = GameObject.Find("VisitorsGuardDestination");
        float x = target.transform.position.x - guardAnim.transform.position.x;
        float y = target.transform.position.y - guardAnim.transform.position.y;

        Vector3 pathway = new Vector3(x, 0, 0);

        pathway = pathway.normalized;

        secondGuard.SetActive(true);
        guard2Anim.SetBool("Start", true);
        playerAnim.GetComponent<Player>().SetFlagBool("HallwayGuardCutsceneActive");
        secondGuard.GetComponent<CutsceneSGMovement>().pathway = pathway;
        secondGuard.GetComponent<CutsceneSGMovement>().activate = true;
    }
    
    public void SendToVisitors()
    {
        //teleportToVisitors.transform.position = playerAnim.transform.position;
    }

    // call when talk with myles on level 1 visitor room is finished
    public void SetNPCSPostVisitors()
    {
        playerAnim.gameObject.GetComponent<Player>().SetFlagBool("TalkedToMyles1");
        // myles npc
        GameObject mylesObj = GameObject.Find("MylesVisitorsDialogue");

        if (mylesObj != null)
        {
            mylesObj.GetComponent<BoxCollider2D>().enabled = false;
        }
        

        // dorothy route

        // goggles route

        // honestly what does the mickey route even connect to?

        // don't forget to put these in CheckOnDelay() too!!
        if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnDorothyRoute"))
        {
            // hide questline goggles
            GameObject gogglesObj = GameObject.Find("GogglesKitchen");

            if (gogglesObj != null)
            {
                gogglesObj.SetActive(false);
            }
            

            // hide questline mickey v, show generic working one instead
            GameObject mickeyQuestObj = GameObject.Find("MickyCutleryQuest");

            if (mickeyQuestObj != null)
            {
                mickeyQuestObj.SetActive(false);
            }
            

            // make generic mickey v visible
            if (MickeyVNoQuest != null)
            {
                MickeyVNoQuest.SetActive(true);
            }
            
        }

        if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnGogglesRoute"))
        {
            // grab mid-quest dorothy cus dialogue will be the same
            // i know i already coded this but it isn't working for some reason
            GameObject original = GameObject.Find("DorothyQuestStart");
            GameObject newObj = GameObject.Find("Dorothy_QuestStarted");

            if (newObj != null && original != null)
            {
                newObj.transform.position = original.transform.position;
                original.SetActive(false);
            }
           


            // THIS COULD BE PUT IN A DIFFERENT FUNCTION TO BE REPEATED (maybe do that when not sleep deprived)
            // hide questline goggles
            GameObject gogglesObj = GameObject.Find("GogglesKitchen");
            if (gogglesObj != null)
            {
                gogglesObj.SetActive(false);
            }
            

            // hide questline mickey v, show generic working one instead
            GameObject mickeyQuestObj = GameObject.Find("MickyCutleryQuest");

            if (mickeyQuestObj != null)
            {
                mickeyQuestObj.SetActive(false);
            }
            
        }

        if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("Level1_MickyVQuestRoute"))
        {

            GameObject dorothyObj = GameObject.Find("DorothyQuestStart");
            if (dorothyObj != null)
            {
                dorothyObj.GetComponent<BoxCollider2D>().enabled = true;
            }
            
            

            GameObject gogglesObj = GameObject.Find("GogglesKitchen");

            if (gogglesObj != null)
            {
                gogglesObj.GetComponent<BoxCollider2D>().enabled = true;
            }
            

        }

    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        StartCoroutine(CheckOnDelay());
    }

    // all the stuff that was in start but delayed so that the save system can sort itself out
    private IEnumerator CheckOnDelay()
    {

        yield return new WaitForSeconds(0.2f);

        // only check these on level 1 cus they're specific to that
        switch (sceneName)
        {
            case "Floor1":
                // prologue fade in manager gets destroyed
                if (playerAnim.gameObject.GetComponent<Player>().currentDay == 1 || playerAnim.gameObject.GetComponent<Player>().currentDay == 2)
                {
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("FirstCutscene_WakeUp"))
                    {

                        fadeOut.gameObject.SetActive(false);

                        GameObject trigger = GameObject.Find("WAKEUP_Trigger");
                        if (trigger != null)
                        {
                            trigger.SetActive(false);
                        }
                        
                    }

                    // no guard
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("FirstCutscene_GuardHallway"))
                    {
                        if (guardAnim != null)
                        {
                            guardAnim.gameObject.SetActive(false);
                        }
                        
                    }

                    // get rid of kitchen dialogue trigger
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("EnteredFoodHallFirstTime"))
                    {
                        GameObject trigger = GameObject.Find("StartKitchenSecurityDialogue");
                        if (trigger != null)
                        {
                            trigger.SetActive(false);
                        }
                    }


                    // get rid of food item in kitchen
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("GotCafeteriaFood"))
                    {
                        GameObject pickup = GameObject.Find("CafeFoodPickup");
                        if (pickup != null)
                        {
                            pickup.SetActive(false);
                        }
                    }

                    // cutlery quest trigger no longer existing
                    if (playerAnim.gameObject.GetComponent<Player>().IsQuestComplete(0))
                    {
                        if (cutleryQuestTrigger != null)
                        {
                            cutleryQuestTrigger.SetActive(false);
                        }
                    }

                    // main kitchen dialogue trigger
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("TalkedToMickyVKitchen"))
                    {
                        GameObject trigger = GameObject.Find("MainKitchenDialogue");
                        if (trigger != null)
                        {
                            trigger.SetActive(false);
                        }                       
                    }

                    // that one mickey obj lol plus anything else that comes with talking to myles
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("TalkedToMyles1"))
                    {
                        // i always type his name wrong for some reason
                        GameObject mickeyVObj = GameObject.Find("NickyV");
                        if (mickeyVObj != null)
                        {
                            mickeyVObj.SetActive(false);
                        }
                        

                        SetNPCSPostVisitors();
                    }

                    // make room inspection appear
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("RoomInspectionTrigger"))
                    {
                        if (roomInspectionDialogue != null && day1EndofDayDialogue != null)
                        {
                            roomInspectionDialogue.SetActive(true);
                            day1EndofDayDialogue.SetActive(true);
                        }
                        
                    }

                    // grapes item appear
                    if (playerAnim.gameObject.GetComponent<Player>().GetFlagBool("DorothyQuest_PhotoSolved"))
                    {
                        if (grapeItem != null)
                        {
                            grapeItem.SetActive(true);
                        }
                        
                    }
                    
                }
                break;
        }
       
    }

    public void StartFadeOut()
    {
        Debug.Log("Fadeout start");
        fadeOut.SetBool("Start", true);
        playerAnim.gameObject.GetComponent<Player>().SetFlagBool("FirstCutscene_WakeUp");
        // start main audio
        FindObjectOfType<AudioManager>().Play("Level1_Sanity1");
        FindObjectOfType<AudioManager>().Play("Level1_Sanity2");
        FindObjectOfType<AudioManager>().Play("Level1_Sanity3");

        // do a funny with the volume
        FindObjectOfType<AudioManager>().ChangeVolume("Level1_Sanity2", 0f);
        FindObjectOfType<AudioManager>().ChangeVolume("Level1_Sanity3", 0f);
    }

    // change player flag (entering the room)
    public void KitchenSecurityDialogue()
    {
        playerAnim.gameObject.GetComponent<Player>().SetFlagBool("EnteredFoodHallFirstTime");
    }

    // change player flag (finished that long dialogue with mickey
    public void KitchenMainDialogue()
    {
        playerAnim.gameObject.GetComponent<Player>().SetFlagBool("TalkedToMickyVKitchen");
    }
}
