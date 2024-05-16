using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockVisitors : MonoBehaviour
{
    [SerializeField] private GameObject visitorsEntrance;
    [SerializeField] private GameObject visitorsLockedDialogue;
    [SerializeField] private Player player;

    private void Start()
    {
        StartCoroutine(CheckDelay());
    }

    private IEnumerator CheckDelay()
    {
        yield return new WaitForSeconds(0.2f);

        if (player.GetFlagBool("VisitorsRoomLocked"))
        {
            visitorsEntrance.SetActive(false);
            visitorsLockedDialogue.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        if (!player.GetFlagBool("VisitorsRoomLocked"))
        {
            Debug.Log("WORk please");
            visitorsEntrance.SetActive(false);
            visitorsLockedDialogue.SetActive(true);
            player.SetFlagBool("VisitorsRoomLocked");
        }
    }
}
