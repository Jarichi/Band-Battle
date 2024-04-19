using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    Canvas canvas;
    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    public void StartMinigame(PlayerWorldInteraction player)
    {
        canvas.enabled = true;
    }
}
