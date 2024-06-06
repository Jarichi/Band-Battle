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
    }

    public override void DeleteInstrument()
    {
        spriteRenderer.sprite = emptyStand;
    }

}
