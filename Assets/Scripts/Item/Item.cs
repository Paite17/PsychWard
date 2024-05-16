using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Items")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField][TextArea] private string itemDescription;
    [SerializeField] private ItemType itemMode;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int itemID;

    public string ItemName
    { 
        get { return itemName; } 
    }

    public string ItemDescription
    {
        get { return itemDescription; }
    }

    public ItemType ItemMode
    {
        get { return itemMode; }
    }

    public Sprite ItemIcon
    {
        get { return itemIcon; }
    }

    public int ItemID
    {
        get { return itemID; }
    }
}


public enum ItemType
{
    NORMAL,
    IMPORTANT
}