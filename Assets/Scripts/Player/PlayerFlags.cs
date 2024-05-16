using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 
[System.Serializable]
public class PlayerFlags
{
    [SerializeField] private string flagName;
    [SerializeField] private bool isActive;

    public string FlagName
    {
        get { return flagName; }
    }

    public bool IsActive
    {
        get { return isActive; }
        set { isActive = value; }
    }
}
