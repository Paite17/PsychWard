using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class QuestBase
{
    public string b_QuestName;
    public string b_Description;
    public QuestType b_Type;
    public int b_QuestID;
    //public Character b_Character;
    //public Item b_Reward;
    public int itemID;
    public int moneyReward;

    public QuestBase(Quest quest)
    {
        b_QuestName = quest.QuestName;
        b_Description = quest.Description;
        b_Type = quest.Type;
        b_QuestID = quest.QuestID;
        //b_Character = quest.Character;
        //b_Reward = quest.Reward;
        if (quest.Reward != null)
        {
            itemID = quest.Reward.ItemID;
        }
        else
        {
            itemID = -1;
        }

        moneyReward = quest.MoneyReward;
    }

    // get the original quest ScriptableObject
    public Quest GetQuestObject()
    {
        var allQuests = Resources.LoadAll("Quests", typeof(Quest)).Cast<Quest>();

        foreach (var current in allQuests)
        {
            if (current.QuestID == b_QuestID)
            {
                return current;
            }
        }

        return null;
    }
}
