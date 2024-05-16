using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "DialogueUI/DialogueObject")]

public class DialogObject : ScriptableObject
{
    // TODO: make a 'dialogue' class that contains text, name, voice and portrait and then make a list in this to store all of those in a cleaner way

    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;
    /*[SerializeField] private string[] charName;
    [SerializeField] private Sprite[] charPortrait;
    [SerializeField] private AudioClip[] charVoice; */
    [SerializeField] private Character[] character;
    [SerializeField] private bool[] hasEvent;
   // [SerializeField] private UnityEvent[] onDialogueRun;
    public string[] Dialogue => dialogue;

    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;

    //public string[] CharName => charName;

    //public Sprite[] CharPortrait => charPortrait;

    //public AudioClip[] CharVoice => charVoice;

    public Character[] Character => character;

    public bool[] HasEvent => hasEvent;

    //public UnityEvent[] OnDialogueRun => onDialogueRun;
}
