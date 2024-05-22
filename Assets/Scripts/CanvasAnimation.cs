using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimation : MonoBehaviour //might alse need to inherit the player class to disable animation when combat
{

    private Animator GUIAnimation;


    // Start is called before the first frame update
    void Start()
    {
        GUIAnimation = GetComponent<Animator>();
        //GUIAnimation.muted = false;
        PlayAnimation();
    }


    void PlayAnimation()
    {
        //play animation ONLY after the can vast gets muted.
        // GUIAnimation.muted = true;
// GUIAnimation.SetTrigger("GuitarGUI");



    }


}
