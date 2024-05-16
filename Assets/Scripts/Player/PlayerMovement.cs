using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 8.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private UIScript UI;
    [SerializeField] private GameObject interactionNotif;
    [SerializeField] private LayerMask Hidden;
    [SerializeField] private LayerMask Default;

    // making these bools is gonna get messy at some point so here's an idea to improve this
    // make a class that contains an enum or something to identify the trigger type
    // on the collision function we do the getcomponent strat i've applied to some of the other triggers
    // check through the enum and match the function to it upon pressing the interaction key
    [SerializeField] private bool onDoor;
    [SerializeField] private bool onCutleryQuestStart;
    [SerializeField] private bool onItem;
    [SerializeField] private bool onKeypad;
    [SerializeField] private bool onHideSpot;
    [SerializeField] private bool onStorageBox;

    public bool isHiding;

    [SerializeField] private GameObject cutleryPuzzle;
    [SerializeField] private QuestManager qm;

    [SerializeField] private CutsceneManager cm;


    private ItemPickup currentItemPickup;

    private float horizontal;
    private float vertical;

    public bool canMove = true;

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable { get; set; }

    private CameraFollow cam;

    private LoadTransition areaTransition;

    public bool used = false;

    public bool inCutscene = false;

    private bool interactActive;

    private DialogueActivator currentDialogueActivator;

    private bool transitionActive;

    private float transitionCooldown;
    [SerializeField] private float transitionCooldownTime;

    private DebugUI debugMoment;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Player>().sceneName != "MainMenu")
        {
            debugMoment = GameObject.Find("DebugManager").GetComponent<DebugUI>();
        }

        GameObject camera = GameObject.Find("Main Camera");
        cam = camera.GetComponent<CameraFollow>();
        transitionCooldown = transitionCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        // decide if the player can move
        if (DialogueUI.isOpen || UI.currentState != MenuState.NONE || inCutscene || debugMoment.menuState != DebugMenuState.NONE)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
        
        // cooldown for door transition
        if (transitionActive)
        {
            transitionCooldown -= Time.deltaTime;
        }

        if (transitionCooldown < 0)
        {
            transitionActive = false;
            transitionCooldown = transitionCooldownTime;
        }
        
        if (interactActive && !GetComponent<Player>().GetFlagBool("HallwayGuardCutsceneActive"))
        {
            if (!inCutscene && !DialogueUI.isOpen)
            {
                interactionNotif.SetActive(true);
            }
            
        }
        else
        {
            interactionNotif.SetActive(false);
        }

        if (canMove)
        {
            rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);
            return;
        }


        if (currentDialogueActivator != null)
        {
            if (currentDialogueActivator.activateOnCollision)
            {
                if (!DialogueUI.isOpen)
                {
                    Interactable.Interact(this);
                }
                
            }
        }

        // sprite animation
        if (horizontal != 0 || vertical != 0)
        {
            if (canMove)
            {
                animator.SetBool("isMoving", true);
            }           
        }
        else
        {
            if (canMove)
            {
                animator.SetBool("isMoving", false);
            }
        }

        // flip sprite
        if (horizontal > 0)
        {
            if (canMove)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
            
        }
        else if (horizontal < 0)
        {
            if (canMove)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            
        }

        //Debug.Log("Horiontal = " + horizontal);
    }

    // input system calls this when movement keys are pressed
    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
    }

    public void Hide(InputAction.CallbackContext context)
    {
            if (onHideSpot)
            {
                if (context.canceled)
                {
                    isHiding = false;
                }
                else
                {
                    isHiding = true;
                
                }
            }
    }


    // player input for interaction
    public void InteractionKey(InputAction.CallbackContext context)
    {
        
        // begin dialogue
        if (UI.currentState == MenuState.NONE)
        {
            if (!GetComponent<Player>().GetFlagBool("HallwayGuardCutsceneActive"))
            {
                if (Interactable != null && !DialogueUI.isOpen)
                {
                    Interactable.Interact(this);
                }

                if (!DialogueUI.isOpen)
                {
                    // start door transition
                    if (onDoor && !onStorageBox && !transitionActive)
                    {
                        areaTransition.BeginTransition();
                        transitionActive = true;
                    }

                    // level 1 quest
                    if (onCutleryQuestStart)
                    {
                        onCutleryQuestStart = false;
                        //GameObject quest = GameObject.Find("CutleryPuzzleUI");
                        cutleryPuzzle.SetActive(true);
                        canMove = false;
                        UI.CutleryPuzzleActivate();

                        // start quest
                        /*Quest quest = Resources.Load("Quests/CutleryQuest") as Quest;
                        qm.AddQuest(quest); */
                    }

                    if (onKeypad)
                    {
                        if (!onDoor)
                        {
                            if (!GetComponent<Player>().GetFlagBool("KeypadUsed") && GetComponent<Player>().GetFlagBool("Level1_OnGogglesRoute"))
                            {
                                used = true;
                                UI.OpenKeypad();
                            }

                        }
                    }

                    if (onItem)
                    {

                        // get item
                        if (!currentItemPickup.isRandom && !currentItemPickup.isCurrency)
                        {
                            switch (currentItemPickup.itemToGive.ItemName)
                            {
                                case "Grapes":
                                    // back to hard-coding lolol
                                    // event
                                    qm.gameObject.GetComponent<DorothyQuest>().PlaceQuestCompleteDorothy();
                                    GetComponent<Player>().SetFlagBool("DorothyQuest_GrapesGot");
                                    break;
                                case "Framed Picture":
                                    // have this until we have item usage

                                    UI.OpenPhotoFrameUI();
                                    break;
                                case "Cafeteria Food":
                                    GetComponent<Player>().SetFlagBool("GotCafeteriaFood");
                                    break;
                            }
                            GetComponent<Player>().PickUpItem(currentItemPickup.itemToGive);
                            Destroy(currentItemPickup.gameObject);
                        }
                        // finish this later (when needed)
                    }

                    if (onStorageBox)
                    {
                        UI.OpenStorageUI();
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            // door marker visible
            onDoor = true;
            interactActive = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // rewritten into a switch statement
        switch (collision.gameObject.tag)
        {
            case "CamStop":
                // stop camera at the corner of a room
                cam.dontFollow = true;
                break;
            case "Door":
                // door marker visible
                onDoor = true;
                areaTransition = collision.gameObject.GetComponent<LoadTransition>();
                interactActive = true;
                break;
            case "CamYActive":
                // allow the y axis to move on the camera
                cam.allowYAxis = true;
                break;
            case "CutleryPuzzleStart":
                // set flag for interaction key to start the cutlery puzzle
                onCutleryQuestStart = true;
                interactActive = true;
                break;
            case "ItemPickup":
                onItem = true;
                currentItemPickup = collision.gameObject.GetComponent<ItemPickup>();
                interactActive = true;
                break;
            case "DorothyQuest_Activator":
                Debug.Log("Collided with quest activator!");
                qm.gameObject.GetComponent<DorothyQuest>().PlaceItem();
                break;
            case "KeypadActivator":
                onKeypad = true;
                interactActive = true;
                break;
            case "DialogueTrigger":
                interactActive = true;
                currentDialogueActivator = collision.gameObject.GetComponent<DialogueActivator>();
                break;
            case "HideSpot":
                onHideSpot= true;
                break;
            case "StartFirstCutscene":
                if (!GetComponent<Player>().GetFlagBool("FirstCutscene_GuardHallway"))
                {
                    cm.StartFirstHallwayCutscene();                    
                    GetComponent<Player>().SetFlagBool("FirstCutscene_GuardHallway");
                }
                break;
            case "AllowInteraction":
                GetComponent<Player>().DeactivateFlagBool("HallwayGuardCutsceneActive");
                break;
            case "SecondGuardHallway":
                if (!GetComponent<Player>().GetFlagBool("HallwayGuardSecondScene"))
                {
                    cm.StartSecondHallwayCutscene();
                    GetComponent<Player>().SetFlagBool("HallwayGuardSecondScene");
                }
                break;
            case "StorageBox":
                onStorageBox = true;
                interactActive = true;
                break;
            case "Roof_EndTrigger":
                // end of demo
                RoofManager rm = GameObject.Find("RoofManager").GetComponent<RoofManager>();
                rm.StartEndingSequence();
                break;
            case "FinalBoss":
                FindObjectOfType<AudioManager>().StopMusic("Final_Boss");
                SceneManager.LoadScene("GameOver");
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "CamStop":
                cam.dontFollow = false;
                break;
            case "Door":
                onDoor = false;
                interactActive = false;
                break;
            case "CamYActive":
                cam.allowYAxis = false;
                break;
            case "CutleryPuzzleStart":
                onCutleryQuestStart = false;
                interactActive = false;
                break;
            case "ItemPickup":
                onItem = false;
                interactActive = false;
                break;
            case "KeypadActivator":
                onKeypad = false;
                interactActive = false;
                break;
            case "DialogueTrigger":
                interactActive = false;
                currentDialogueActivator = null;
                break;
            case "HideSpot":
                onHideSpot = false;
                break;
            case "StorageBox":
                onStorageBox = false;
                interactActive = false;
                break;
        }
    }
    
}
