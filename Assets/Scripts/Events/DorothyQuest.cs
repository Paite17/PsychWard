using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DorothyQuest : MonoBehaviour
{
    [SerializeField] private GameObject grapeItem;
    [SerializeField] private GameObject activatorHitbox;
    [SerializeField] private GameObject dorothyNew;
    [SerializeField] private GameObject dorothyOld;
    public void PlaceItem()
    {
        Debug.Log("PlaceItem() called!");
        grapeItem.SetActive(true);
        activatorHitbox.SetActive(false);
    }

    public void PlaceQuestCompleteDorothy()
    {
        Debug.Log("PlaceQuestCompleteDorothy() called!");
        //GameObject dorothyNew = GameObject.Find("Dorothy_QuestProgressed");
        //GameObject dorothyOld = GameObject.Find("Dorothy_QuestStarted");

        dorothyNew.transform.position = dorothyOld.transform.position;

        dorothyOld.SetActive(false);
    }

    private void Start()
    {
        // check if item should exist
        GameObject plr = GameObject.Find("Player");
        Player player = plr.GetComponent<Player>();

        // if the player has the quest active but hasn't gotten the item yet since upon loading into the game mid quest the item disappears
        // effectively softlocking the game
        if (player.sceneName == "FirstFloor")
        {
            if (player.GetFlagBool("DorothyQuest_Started") && !player.HasItem(4))
            {
                PlaceItem();
            }
        }
        
    }
}
