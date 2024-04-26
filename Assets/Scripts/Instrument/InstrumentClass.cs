using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    public GameObject weapon;
    private Minigame minigame;
    
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

    public void Interact(GameObject player)
    {
        OnPlaying();
        if (player.GetComponent<PlayerCombat>().inCombat)
        {
            return;
        }

        minigame.StartMinigame(player, weapon);
    }


}
