using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour
{
    [SerializeField] private GameObject questNotif;
    [SerializeField] private Text questName;
    [SerializeField] private Text questUnlocked;
    [SerializeField] private Animator questNotifAnimator;

    [SerializeField] private GameObject itemNotif;
    [SerializeField] private Text itemName;
    [SerializeField] private Animator itemNotifAnimator;

    public void ShowQuestNotif(QuestBase thisQuest)
    {
        StartCoroutine(QuestNotification(thisQuest, false));
    }

    public void ShowQuestCompleteNotif(QuestBase thisQuest)
    {
        StartCoroutine(QuestNotification(thisQuest, true));
    }

    private IEnumerator QuestNotification(QuestBase thisQuest, bool notifType)
    {
        if (!notifType)
        {
            questUnlocked.text = "New Quest:";
        }
        else
        {
            questUnlocked.text = "Quest Complete!";
        }

        questNotif.SetActive(true);

        questName.text = thisQuest.b_QuestName;

        yield return new WaitForSeconds(2.5f);

        questNotifAnimator.SetBool("Exit", true);

        yield return new WaitForSeconds(0.5f);

        questNotif.SetActive(false);
    }

    public void ShowItemNotif(Item thisItem)
    {
        StartCoroutine (ItemNotification(thisItem));
    }

    private IEnumerator ItemNotification(Item thisItem)
    {
        itemNotif.SetActive(true);

        itemName.text = thisItem.ItemName;

        yield return new WaitForSeconds(2.5f);

        itemNotifAnimator.SetBool("Exit", true);

        yield return new WaitForSeconds(0.5f);

        itemNotif.SetActive(false);
    }
}
