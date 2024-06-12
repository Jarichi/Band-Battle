using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerList : MonoBehaviour
{
    public static PlayerList Instance;
    private static readonly List<string> Players = new();
    [SerializeField]
    private Color[] playerColors = new Color[4];
    [SerializeField] private GameObject[] playerEntities = new GameObject[4];
    [HideInInspector]
    public PlayerInputManager inputManager;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        inputManager = GetComponent<PlayerInputManager>();
    }

    public static List<Player> Get()
    {
        return Players.Select(id => Player.ByID(id)).ToList();
    }

    public static List<string> GetAllIDs()
    {
        return Players;
    }

    public static Color[] ColorList()
    {
        return Instance.playerColors;
    }
    public static GameObject[] PlayerEntitiesList()
    {
        return Instance.playerEntities;
    }

    public void OnPlayerJoin(PlayerInput input)
    {
        var id = input.GetInstanceID().ToString();
        input.gameObject.name = id;
        Players.Add(id);
        Debug.Log("A player has connected: " + input.gameObject.name);
    }

    public void OnPlayerLeave(PlayerInput input)
    {
        Get().ForEach(p => p.UpdateIndex());
        Debug.Log("A player has disconnected: " + input.gameObject.name);
        Players.Remove(input.gameObject.name);
    }
}
