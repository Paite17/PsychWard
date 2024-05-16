using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueEventSystem
{
    [HideInInspector] public string _name;
    [SerializeField] private UnityEvent onDialogueRun;

    public UnityEvent OnDialogueRun => onDialogueRun;
}
