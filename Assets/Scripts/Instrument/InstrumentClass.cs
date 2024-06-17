using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    public GameObject weapon;
    
    public GameObject minigame;
    
    private Transform playerTransform;
    [SerializeField] private int beatmapChannel;

    protected bool inRange = false;
    protected abstract void OnPlaying();

    public string fmodParameterName;

    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        playerTransform = col.GetComponent<Transform>();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
    }

    public bool IsCorrectChannel(int channel)
    {
        return channel == beatmapChannel;
    }

    public abstract void DeleteInstrument();

}
