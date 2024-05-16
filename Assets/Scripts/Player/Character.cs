using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    [SerializeField] private string charName;
    [SerializeField] private Sprite charImage;
    [SerializeField][TextArea] private string charDescription;
    //[SerializeField] private List<Quest> unlockedQuests;
    [SerializeField] private List<Sprite> unlockedPhotos;
    [SerializeField] private List<AudioClip> voice;

    /*public void AddQuest(Quest questToAdd)
    {
        Debug.Log("Added new quest to " + charName);
        unlockedQuests.Add(questToAdd);
    } */

    public void AddPhoto(Sprite photoToAdd)
    {
        Debug.Log("Added new photo to " + charName);
        unlockedPhotos.Add(photoToAdd);
    }

    // public getters for variables
    public string CharName
    {
        get { return charName; }
        set { charName = value; }
    }
    public Sprite CharImage
    {
        get { return charImage; }
        set { charImage = value; }
    }
    /*public List<Quest> UnlockedQuests 
    { 
        get {  return unlockedQuests; } 
        set {   unlockedQuests = value; }
    } */

    public List<Sprite> UnlockedPhotos
    {
        get { return unlockedPhotos; }
        set { unlockedPhotos = value; }
    }

    public List<AudioClip> Voice
    {
        get { return voice; }
    }

    public string CharDescription
    {
        get { return charDescription; }
    }
}
