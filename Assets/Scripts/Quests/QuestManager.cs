using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Notifications notification;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // add quest to player's quest list
    public void AddQuest(Quest addition)
    {
        bool done = false;
        if (!done)
        {
            done = true;

            Debug.Log("Added quest " + addition.QuestName + " to player quests");

            QuestBase questAddition = new QuestBase(addition);
            player.currentQuests.Add(questAddition);
            player.SortQuestList();
            player.eventTokens--;
            notification.ShowQuestNotif(questAddition);
        }
    }

    // find the quest scriptableobject that matches the current questbase object
    private Quest FindMatchingQuest(QuestBase questBase)
    {
        Quest[] questList = FindObjectsOfType<Quest>();

        for (int i = 0; i < questList.Length; i++)
        {
            Debug.Log(questList[i].QuestName);
            if (questBase.b_QuestID == questList[i].QuestID)
            {
                return questList[i];
            }
        }

        return null;
    }



    // marks a quest as complete and give reward if applicable
    public void QuestCompletion(int id)
    { 
        //player.currentQuests[id].IsComplete = true;

        // look through each quest and see if it matches id
        for (int i = 0; i < player.currentQuests.Count; i++)
        {
            if (id == player.currentQuests[i].b_QuestID)
            {
                Debug.Log(player.currentQuests[i].b_QuestName);
                notification.ShowQuestCompleteNotif(player.currentQuests[i]);
                

                // check if the quest has an item
                if (player.currentQuests[i].itemID != -1)
                {
                    Quest quest = player.currentQuests[i].GetQuestObject();
                    Debug.Log(quest.Reward.ItemName);
                    Debug.Log("Given reward " + quest.Reward.ItemName + " to player");
                    player.PickUpItem(quest.Reward);
                }

                player.money += player.currentQuests[i].moneyReward;

                Debug.Log("Set quest " + player.currentQuests[i].b_QuestName + " as complete");
                player.completedQuests.Add(player.currentQuests[i]);
                player.currentQuests.Remove(player.currentQuests[i]);
            }
        }
    }
}
