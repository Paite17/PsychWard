using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this will provide limited safe storage for some items
public class StorageBox : MonoBehaviour
{
    public List<ItemBase> storedItems;

    public int storageLimit = 3;

    

    private UIScript ui;

    public void AddItemToStorage(ItemBase itemToAdd, Player player)
    {
        if (storedItems.Count < storageLimit) 
        {
            storedItems.Add(itemToAdd);
            //player.playerInventory.Remove(itemToAdd);
        }
        else
        {
            // display storage full prompt
        }
    }

    public void RemoveItemFromStorage(ItemBase itemToRemove, Player currentPlayer)
    {
        currentPlayer.playerInventory.Add(itemToRemove);
        storedItems.Remove(itemToRemove);
    }

    private void Start()
    {
        ui = GameObject.Find("UIManager").GetComponent<UIScript>();

        if (SaveSystem.DoesStorageFileExist())
        {
            LoadStorage();
        }
        else
        {
            SaveSystem.CreateStorageFile(this);
            SaveStorage();
        }
    }

    public void SaveStorage()
    {
        SaveSystem.SaveStorage(this);
    }

    public void LoadStorage()
    {
        StorageData data = SaveSystem.LoadStorage();

        storedItems = data.storedItems;
    }
}
