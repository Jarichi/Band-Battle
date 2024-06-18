using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    public GameObject weapon;
    
    public GameObject minigame;

    protected bool inRange = false;

    public string id;
    protected abstract void OnPlaying();

    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
    }

    public abstract void DeleteInstrument();

}
