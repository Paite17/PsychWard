using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// the entire purpose of this script is to hold whatever item is currently on that spot in the UI
public class InvItemAssign : MonoBehaviour
{
    public ItemBase currentItem;

    public Sprite invIcon;

    public void SetImageIconAsItem()
    {
        invIcon = currentItem.GetIconByID();
        //GetComponentInChildren<Image>().sprite = invIcon;
        transform.GetChild(0).GetComponent<Image>().sprite = invIcon;
    }
}
