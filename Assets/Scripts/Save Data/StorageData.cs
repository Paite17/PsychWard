using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageData
{
    public List<ItemBase> storedItems;

    public StorageData(StorageBox storage)
    {
        storedItems = storage.storedItems;
    }
}
