using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PhotoState
{
    FRONT,
    BACK,
    FRONT_UNFRAMED,
    BACK_UNFRAMED
}

public class PhotoFramePuzzle : MonoBehaviour
{
    [SerializeField] private Slider photoSlider;
    [SerializeField] private Sprite photoFront;
    [SerializeField] private Sprite photoBack;
    [SerializeField] private QuestManager qm;
    [SerializeField] private Player player;
    [SerializeField] private Sprite unframedPhotoFront;
    [SerializeField] private Sprite unframedPhotoBack;
    [SerializeField] private GameObject grapeObj;

    [SerializeField] private List<UI_Screws> screws;
    [SerializeField] private int screwedCount;

    private PhotoState state = PhotoState.FRONT;

    private bool hasTurnedOnce;

    // called when the value of the slider changes
    public void CheckValue()
    {
        // check if the value is 1
        if (photoSlider.value == 1)
        {
            if (state == PhotoState.FRONT) 
            {
                state = PhotoState.BACK;
            }
            else if (state == PhotoState.FRONT_UNFRAMED)
            {
                state = PhotoState.BACK_UNFRAMED;
            }
            
        }
        else if (photoSlider.value == 0)
        {
            if (state == PhotoState.BACK)
            {
                state = PhotoState.FRONT;
            }
            else if (state == PhotoState.BACK_UNFRAMED)
            {
                state = PhotoState.FRONT_UNFRAMED;
            }
            
        }

        ChangeSprite();
    }

    public void UpdateScrews()
    {
        for (int i = 0; i < screws.Count; i++)
        {
            if (screws[i].state == ScrewState.UNSCREWED)
            {
                screwedCount++;
            }
        }

        if (screwedCount == 10)
        {
            state = PhotoState.BACK_UNFRAMED;
            ChangeSprite();
        }
    }


    private void ChangeSprite()
    {
        GameObject photoObj = GameObject.Find("Photoframe_BackgroundImg");
        Image photoImg = photoObj.GetComponent<Image>();
        switch (state)
        {
            case PhotoState.FRONT:
                photoImg.sprite = photoFront;

                // make screws invis
                for (int i = 0; i < screws.Count; i++)
                {
                    screws[i].gameObject.SetActive(false);
                }
                break;
            case PhotoState.BACK:
                photoImg.sprite = photoBack;
                // make screws visible
                for (int i = 0; i < screws.Count; i++)
                {
                    if (screws[i].state != ScrewState.UNSCREWED)
                    {
                        screws[i].gameObject.SetActive(true);
                    }
                }
                
                break;
            case PhotoState.FRONT_UNFRAMED:
                photoImg.sprite = unframedPhotoFront;
                break;
            case PhotoState.BACK_UNFRAMED:
                photoImg.sprite = unframedPhotoBack;
                QuestProgress();
                break;
        }

        
    }

    // progress the quest that is tied to this
    private void QuestProgress()
    {
        // check if the quest exists first
        if (player.IsQuestActive(1))
        {
            if (!player.HasItem(5))
            {
                // spawn grapes
                if (!player.GetFlagBool("DorothyQuest_PhotoSolved"))
                {
                    grapeObj.SetActive(true);
                    player.SetFlagBool("DorothyQuest_PhotoSolved");
                    //player.PickUpItem(Resources.Load("Items/grape") as Item);
                    
                }
                
            }
        }
        else
        {
            Debug.Log("Dorothy Quest either has already been complete or hasn't started, don't do anything here");
        }
    }

    private void Start()
    {
        
    }
}
