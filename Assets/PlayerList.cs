using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    public static readonly List<Player> Players = new();

    public static int Count() {
        return Players.Count;
    }
}
