using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<ItemBase> playerInventory;
    public float sanity;
    public int currentLevel;
    public List<string> charactersMet;
    public List<QuestBase> currentQuests;
    public List<QuestBase> completedQuests;
    public int eventTokens;
    public List<PlayerFlags> playerFlags;
    public float playerX;
    public float playerY;
    public int currentDay;
    public bool isNight;
    public bool changingFloor;
    public int money;


    public PlayerData(Player player)
    {
        playerInventory = player.playerInventory;
        sanity =  player.sanity;
        currentLevel = player.currentLevel;
        charactersMet = player.charactersMet;
        currentQuests = player.currentQuests;
        completedQuests = player.completedQuests;
        eventTokens = player.eventTokens;
        playerFlags = player.playerFlags;
        playerX = player.playerX;
        playerY = player.playerY;
        currentDay = player.currentDay;
        isNight = player.isNight;
        changingFloor = player.changingFloor;
        money = player.money;
    }
}
