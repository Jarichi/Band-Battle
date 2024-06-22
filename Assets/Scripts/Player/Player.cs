using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject playerEntityPrefab;
    public GameData data;
    public int Index { get; private set; }
    public Color Color { get; private set; }
    public PlayerEntity Entity {get; private set;}
    public PlayerInput Input { get; private set; }
    public bool isReady = false;

    void Start()
    {
        Input = GetComponent<PlayerInput>();
        Input.actions["Start Game"].performed += SetReady;
        Input.actions["Cancel"].performed += SetUnready;
        UpdateIndex();

        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        Input.actions["Start Game"].performed -= SetReady;
        Input.actions["Cancel"].performed -= SetUnready;
    }

    private void SetReady(InputAction.CallbackContext ctx)
    {
        if (SceneManager.GetActiveScene().name == "Character Selection")
            isReady = true;
    }

    private void SetUnready(InputAction.CallbackContext ctx)
    {
        if (SceneManager.GetActiveScene().name == "Character Selection")
            isReady = false;
    }

    public static Player ByID(int id)
    {
        return GameObject.Find(id.ToString()).GetComponent<Player>();
    }

    public static Player ByID(string id)
    {
        return GameObject.Find(id).GetComponent<Player>();
    }

    public string ID()
    {
        return gameObject.name;
    }

    public void UpdateIndex()
    {
        Index = PlayerList.Get().IndexOf(this) + 1;
        data.color = PlayerList.ColorList()[Index - 1];
        playerEntityPrefab = PlayerList.PlayerEntitiesList()[Index - 1];
    }

    public void SwitchActionMap(string name)
    {
        Input.SwitchCurrentActionMap(name);
    }

    public void DisableAllControls()
    {
        Input.DeactivateInput();
    }

    public void EnableControls()
    {
        Input.ActivateInput();
    }

    public void Spawn(Vector2 position) {
        playerEntityPrefab.transform.position = position;
        Entity = Instantiate(playerEntityPrefab, gameObject.transform).GetComponent<PlayerEntity>();

        PlayerList.Instance.PlayerSpawnEvent?.Invoke(Entity);
    }
    
    public void Despawn()
    {
        PlayerList.Instance.PlayerDespawnEvent?.Invoke(Entity);
        Destroy(Entity.gameObject);
    }

    [Serializable]
    public class GameData
    {
        public int totalScore;
        public bool isBandLeader;
        public Color color;
    }

    public static string MenuActionMap = "Menu";
    public static string PlayActionMap = "Player";
}
