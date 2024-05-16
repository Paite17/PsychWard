using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quest", menuName = "Quests")]
public class Quest : ScriptableObject
{
    [SerializeField] private string questName;
    [SerializeField][TextArea] private string description;
    [SerializeField] private QuestType type;
    [SerializeField] private int questID;
    //[SerializeField] private bool isComplete;
    [SerializeField] private Character character;
    [SerializeField] private Item reward;
    [SerializeField] private int moneyReward;

    public string QuestName
    {
        get { return questName; }
    }

    public string Description
    {
        get { return description; }
    }

    public QuestType Type
    {
        get { return type; }
    }

    public int QuestID
    {
        get { return questID; }
    }

    /*public bool IsComplete
    {
        get { return isComplete; }
        set { isComplete = value; }
    } */

    public Character Character
    {
        get { return character; }
    }

    public Item Reward
    {
        get { return reward; }
    }

    public int MoneyReward
    { 
        get { return moneyReward; } 
    }
}
