using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    public GameObject weapon;
    
    public GameObject minigame;
    
    private Transform playerTransform;
    
    protected bool inRange = false;
    public abstract void OnPlaying();

    void Start()
    {
        Debug.Log("Start");

    }

    private void Awake()
    {
        print("awake");



    }

    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        playerTransform = col.GetComponent<Transform>();
        if (!col.GetComponent<PlayerCombat>().inCombat)
        {
            GameObject.FindGameObjectWithTag("temp").GetComponent<Canvas>().enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
        GameObject.FindGameObjectWithTag("temp").GetComponent<Canvas>().enabled = false;

    }

    /*void Update()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Interact(playerTransform.gameObject);
                OnPlaying(); 
            }
        }


    }*/

    public void Interact(GameObject player)
    {
        OnPlaying();
        if (player.GetComponent<PlayerCombat>().inCombat)
        {
            return;
        }

        GameObject minigameObj = Instantiate(minigame, player.transform);
        var minigameScript = minigameObj.GetComponent<Minigame>();
        
        minigameScript.StartMinigame(player, weapon);
    }


}
