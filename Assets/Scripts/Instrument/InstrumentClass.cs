using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    public GameObject weapon;
    private Minigame minigame;
    private Transform playerTransform;
    
    protected bool inRange = false;
    protected abstract void OnPlaying();

    void Start()
    {
        Debug.Log("Start");
    }

    private void Awake()
    {
        minigame = GetComponent<Minigame>();
    }

    //proximity detection
    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        playerTransform = col.GetComponent<Transform>();
        if(!col.GetComponent<PlayerCombat>().inCombat) GameObject.FindGameObjectWithTag("temp").GetComponent<Canvas>().enabled = true;

    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
        GameObject.FindGameObjectWithTag("temp").GetComponent<Canvas>().enabled = false;

    }

    void Update()
    {
        if (inRange)
        {
            //check if you want to play this instrument.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Interact(playerTransform.gameObject);
                OnPlaying(); 
            }
        }


    }

    public void Interact(GameObject player)
    {
        if (playerTransform.GetComponent<PlayerCombat>().inCombat)
        {
            return;
        }

        minigame.StartMinigame(player, weapon);
    }


}
