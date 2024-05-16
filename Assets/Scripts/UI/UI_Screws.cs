using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ScrewState
{
    SCREWED,
    UNSCREWED
}

public class UI_Screws : MonoBehaviour, IPointerDownHandler
{
    public ScrewState state;

    public void OnPointerDown(PointerEventData eventData)
    {
        state = ScrewState.UNSCREWED;
        StateChange();
    }

    // stuff that happens when the state changes
   private void StateChange()
    {
        if (state == ScrewState.UNSCREWED)
        {
            // maybe sfx????
           gameObject.SetActive(false);

            GameObject uiObj = GameObject.Find("PhotoFrameUI");
            PhotoFramePuzzle puzzle = uiObj.GetComponent<PhotoFramePuzzle>();

            puzzle.UpdateScrews();
        }
    }
}
