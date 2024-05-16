using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GogglesQuest : MonoBehaviour
{
    [SerializeField] private GameObject gogglesDoor;
    [SerializeField] private GameObject gogglesRollUnder;
    [SerializeField] private GameObject gogglesDoorLocked;
    
    public void SetRollUnderTrigger()
    {
        Debug.Log("SetRollUnderTrigger() called!");
        gogglesDoorLocked.SetActive(false);
        gogglesRollUnder.SetActive(true);
    }

    // give the quest upon picking up the item
    public void GiveQuest()
    {
        GetComponent<QuestManager>().AddQuest(Resources.Load("Quests/GogglesBatteryQuest") as Quest);
    }

    public void UnlockDoor()
    {
        gogglesRollUnder.SetActive(false);
        gogglesDoor.SetActive(true);
    }

    // for giving the battery when the UI is done
    public void GiveBattery()
    {
        GameObject plr = GameObject.Find("Player");
        Player player = plr.GetComponent<Player>();

        player.PickUpItem(Resources.Load("Items/battery") as Item);
    }

    // talking to goggles in the kitchen
    public void GiveCipher()
    {
        GameObject plr = GameObject.Find("Player");
        Player player = plr.GetComponent<Player>();

        if (!player.GetFlagBool("TalkedToGogglesKitchen"))
        {
            player.PickUpItem(Resources.Load("Items/cipher_note") as Item);
            player.SetFlagBool("TalkedToGogglesKitchen");
        }
    }
}
