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
    protected abstract void OnPlaying();

    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        playerTransform = col.GetComponent<Transform>();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
    }

    public void StartMinigame(GameObject player)
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
