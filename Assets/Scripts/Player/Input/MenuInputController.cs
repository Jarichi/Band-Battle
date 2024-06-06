using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputController : MonoBehaviour
{
    public void OnPlayerJoin(PlayerInput input)
    {
        var player = input.gameObject.GetComponent<Player>();
        PlayerList.Players.Add(player);
        Debug.Log("A player has connected: " + player.name);
    }
    public void OnPlayerLeave(PlayerInput input)
    {
        var player = input.gameObject.GetComponent<Player>();
        Debug.Log("A player has disconnected: " + player.name);
        PlayerList.Players.Remove(player);
    }
}
