using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drums : Instrument
{
    public override void DeleteInstrument()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    protected override void OnPlaying() {} //empty function
    //when playing the drums, replace drum object with drums and player, and make player object invisible.
    //OR, make invisible and start animation for the player to start playing drums

}
