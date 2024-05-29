using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicMinigame : Minigame
{
    //hardcoded song. We might need to write a script that can convert a midi file to some sort of quantised beat-accurate representation of a song
    protected override string GetCombatAnimationName()
    {
        return "ObtainMic";
    }




}
