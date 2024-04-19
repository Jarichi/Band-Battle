using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteraction : MonoBehaviour
{
    [InspectorButton("OnButtonClicked")]
    public bool startMinigame;
    public PlayerCombat combat;

    public void Start()
    {
        combat = GetComponent<PlayerCombat>();
    }

    //private void OnButtonClicked()
    //{
    //    StartMinigame();
    //}

    //private void StartMinigame()
    //{
    //    minigame.Show(this);
    //}

    //private void StartCombat()
    //{
    //    minigame.Hide();
    //}
}
