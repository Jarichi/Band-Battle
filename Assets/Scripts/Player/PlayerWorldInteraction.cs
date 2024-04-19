using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteraction : MonoBehaviour
{
    public PlayerCombat combat;

    public void Start()
    {
        combat = GetComponent<PlayerCombat>();
    }
}
