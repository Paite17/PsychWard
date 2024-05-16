using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


// script for some events on the roof, namely the enemy spawn
public class RoofManager : MonoBehaviour
{
    [SerializeField] private GameObject monster;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D playerLight;
    [SerializeField] CameraFollow camFollow;
    [SerializeField] private Transform camDestination;
    [SerializeField] private float camMoveSpeed;

    private Vector3 pathway;

    private float monsterSpawnTimer;

    private bool moveCam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        monsterSpawnTimer += Time.deltaTime;

        if (monsterSpawnTimer > maxSpawnTime)
        {
            monster.SetActive(true);
        }

        if (moveCam)
        {
            camFollow.transform.position += pathway * camMoveSpeed * Time.deltaTime;
        }
    }


    public void StartEndingSequence()
    {
        StartCoroutine(EndingSequence());
    }

    private IEnumerator EndingSequence()
    {
        monster.SetActive(false);

        globalLight.intensity = 0.5f;
        playerLight.intensity = 0.25f;

        camFollow.dontFollow = true;

        float y = camDestination.position.y - monster.transform.position.y;

        Vector3 path = new Vector3(0, y, 0);

        pathway = path.normalized;

        moveCam = true;

        yield return new WaitForSeconds(2f);

        FindObjectOfType<AudioManager>().StopMusic("Final_Boss");
        //SceneManager.LoadScene("EndOfDemo");

        // actual final dialogue next...

    }
}
