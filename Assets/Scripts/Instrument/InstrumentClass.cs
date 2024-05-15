using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    public GameObject weapon;
    
    public GameObject minigame;
    //public Minigame minigameScript;

    //[SerializeField]
    
    private Transform playerTransform;
    
    protected bool inRange = false;
    protected abstract void OnPlaying();

    void Start()
    {
        Debug.Log("Start");

    }

    private void LateUpdate()
    {
       // minigameScript = minigame.GetComponent<Minigame>();


    }
    private void Awake()
    {
        print("awake");



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
        //print(weapon);
        //print(minigame);
        if (inRange)
        {
            //check if you want to play this instrument.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Interact(playerTransform.gameObject);
                OnPlaying(); 
                //Instantiate(minigame);
            }
        }


    }

    private void Interact(GameObject player)
    {
        if (playerTransform.GetComponent<PlayerCombat>().inCombat)
        {
            return;
        }

        GameObject minigameObj = Instantiate(minigame, player.transform);
        var minigameScript = minigameObj.GetComponent<Minigame>();
        
        minigameScript.StartMinigame(player, weapon);
    }


}
