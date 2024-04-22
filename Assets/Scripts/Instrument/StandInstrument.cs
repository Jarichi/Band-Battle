using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandInstrument : Instrument
{
    public Sprite emptyStand;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnPlaying()
    {
        spriteRenderer.sprite = emptyStand;
        Debug.Log("replace sprite");
    }
}
