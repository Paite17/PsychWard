using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField] private DialogObject dialogueObject;
    [SerializeField] private DialogueEventSystem[] events;

    public DialogObject DialogueObject => dialogueObject;

    public DialogueEventSystem[] Events => events;

    public void ChangeDialogueObject(DialogObject newDialogue)
    {
        this.dialogueObject = newDialogue;
    }

    public void OnValidate()
    {
        if (DialogueObject == null) 
        {
            return;
        }

        if (events != null && events.Length == dialogueObject.HasEvent.Length)
        {
            return;
        }

        if (events == null)
        {
            events = new DialogueEventSystem[dialogueObject.HasEvent.Length];
        }
        else
        {
            Array.Resize(ref events, dialogueObject.HasEvent.Length);
        }

        for (int i = 0; i > dialogueObject.HasEvent.Length; i++)
        {
            if (events[i] != null)
            {
                events[i]._name = dialogueObject.name;
                continue;
            }

            events[i] = new DialogueEventSystem() { _name = dialogueObject.name };
        }
    }
}
