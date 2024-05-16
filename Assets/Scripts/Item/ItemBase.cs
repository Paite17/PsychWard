using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class ItemBase
{
    public string b_ItemName;
    public string b_ItemDescription;
    public ItemType b_ItemMode;
    //public Sprite b_ItemIcon;
    public int b_ItemID;

    public ItemBase(Item item)
    {
        b_ItemName = item.ItemName;
        b_ItemDescription = item.ItemDescription;
        b_ItemMode = item.ItemMode;
        //b_ItemIcon = item.ItemIcon;
        b_ItemID = item.ItemID;
    }

    // get the icon for an item
    public Sprite GetIconByID()
    {

        var allItems = Resources.LoadAll("Items", typeof(Item)).Cast<Item>();

        foreach (var current in allItems)
        {
            if (current.ItemID == b_ItemID)
            {
                Debug.Log("Got item icon for " +  current.name);
                return current.ItemIcon;
            }
        }

        Debug.LogWarning("Could not find icon");
        return null;
    }

    public void GetItemDetailsByID(int itemID)
    {
        // item database
        var item = Resources.LoadAll("Items", typeof(Item)).Cast<Item>();
        
        // check items
        foreach (var current in item)
        {
            if (current.ItemID == b_ItemID)
            {
                b_ItemName = current.ItemName;
                b_ItemDescription = current.ItemDescription;
                b_ItemMode = current.ItemMode;
                b_ItemID = current.ItemID;
            }
        }
    }
}
