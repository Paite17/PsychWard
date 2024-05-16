using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickSpoon : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private QuestManager qm;
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private GameObject useMouseIndicator;
    private ClickState state = ClickState.UNCLICKED;

    public void OnPointerDown(PointerEventData eventData)
    {
        state = ClickState.CLICKED;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        state = ClickState.UNCLICKED;
    }

    private void Update()
    {
        switch (state)
        {
            case ClickState.CLICKED:
                // obj position follow mouse
                transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
                break;
            case ClickState.UNCLICKED:
                // don't do the above
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CutleryPuzzle_OutsideDrawer")
        {
            // end puzzle and subsequent quest
            useMouseIndicator.SetActive(false);
            puzzleUI.SetActive(false);

            qm.QuestCompletion(0);

            // get player movement and set move to true
            GameObject plr = GameObject.Find("Player");
            PlayerMovement plrMove = plr.GetComponent<PlayerMovement>();

            GameObject cutleryTrigger = GameObject.Find("CutleryQuestStartTrigger");
            cutleryTrigger.SetActive(false);
            // SET TRIGGER FOR QUEST BEING DONE 

            plrMove.canMove = true;

            // start kitchen cutscene
            GameObject cutscene = GameObject.Find("CutsceneManager");
            CutsceneManager cm = cutscene.GetComponent<CutsceneManager>();

            cm.SetVisitorsCutsceneReady();
            cm.RoomInspectionInitialise();
        }
    }
}

public enum ClickState
{
    CLICKED,
    UNCLICKED
}
