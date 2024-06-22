using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    public Player Player { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerCombat Combat { get; private set; }
    public PlayerRhythm Rhythm { get; private set; }
    void Start()
    {
        Player = GetComponentInParent<Player>();
        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Rhythm = GetComponent<PlayerRhythm>();
    }
}
