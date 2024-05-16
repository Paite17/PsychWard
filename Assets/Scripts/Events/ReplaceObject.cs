using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceObject : MonoBehaviour
{
    [SerializeField] private GameObject original_;
    [SerializeField] private GameObject newObj_;
    // Different functions for replacing a character object with another

    // TODO: replace hallway objects during cutscene

    // for after cutlery quest
    public void PlaceDorothyQuestStart()
    {
        GameObject original = GameObject.Find("DorothyNoQuest");
        GameObject newObj = GameObject.Find("DorothyQuestStart");

        newObj.transform.position = original.transform.position;
        original.SetActive(false);
    }

    // reverse of the above
    public void PlaceDorothyNoQuest()
    {
        
        GameObject original = GameObject.Find("DorothyQuestStart");
        GameObject newObj = GameObject.Find("DorothyNoQuest");

        newObj.transform.position = original.transform.position;
        original.SetActive(false);
    }

    // Starting dorothy quest
    public void ReplaceDorothyObject()
    {
        Debug.Log("ReplaceDorothyObject() called!");
        GameObject original = GameObject.Find("DorothyQuestStart");
        GameObject newObj = GameObject.Find("Dorothy_QuestStarted");

        GameObject plrObj = GameObject.Find("Player");
        Player player = plrObj.GetComponent<Player>();
        if (!player.gameObject.GetComponent<Player>().GetFlagBool("Level1_OnGogglesRoute"))
        {
            newObj.transform.position = original.transform.position;
            original.SetActive(false);
        }
        // set save flag
    }

    // post-quest object (this is totally not confusing at all)
    public void ReplaceDorothyObjectAgain()
    {
        newObj_.transform.position = original_.transform.position;
        //original.SetActive(false);
    }

    public void ReplaceGogglesObject()
    {
        GameObject original = GameObject.Find("GogglesNPC");
        GameObject newObj = GameObject.Find("GogglesNPC2");

        newObj.transform.position = original.transform.position;
        original.SetActive(false);
    }

    // change mickey v object to the quest one 
    public void ReplaceMickeyVObject()
    {
        GameObject original = GameObject.Find("NickyV");
        GameObject newObj = GameObject.Find("MickyCutleryQuest");

        newObj.transform.position = original.transform.position;
        original.SetActive(false);

        
    }

    // change mickey v object to midquest
    public void MickeyVMidQuest()
    {
        GameObject original = GameObject.Find("MickyCutleryQuest");
        GameObject newObj = GameObject.Find("MickeyVMidCutleryQuest");

        newObj.transform.position = original.transform.position;
        original.SetActive(false);
    }
}
