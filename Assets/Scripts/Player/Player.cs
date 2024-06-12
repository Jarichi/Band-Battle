using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject playerEntityPrefab;
    public GameData data;
    public int Index { get; private set; }
    public Color Color { get; private set; }
    public GameObject InGameEntity {get; private set;}
    public MenuInputController MenuInputController { get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }
    public PlayerInput Input { get; private set; }
    public bool isReady = false;


    public static EventHandler PlayerSpawnEvent;
    public static EventHandler PlayerDespawnEvent;

    void Start() {
        MenuInputController = GetComponent<MenuInputController>();
        MenuInputController.StartGamePressed += (obj => { isReady = true; });
        MenuInputController.CancelPressed += (obj => { isReady = false; });

        PlayerInputController = GetComponent<PlayerInputController>();
        Input = GetComponent<PlayerInput>();
        UpdateIndex();

        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        MenuInputController.StartGamePressed = null;
        MenuInputController.CancelPressed = null;
    }

    public static Player OfEntity(GameObject playerEntity)
    {
        return playerEntity.GetComponentInParent<Player>();
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
        Color = PlayerList.ColorList()[Index - 1];
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
        InGameEntity = Instantiate(playerEntityPrefab, gameObject.transform);

        PlayerSpawnEvent?.Invoke(this, new EventArgs());
    }
    
    public void Despawn() {
        Destroy(InGameEntity);
        PlayerDespawnEvent?.Invoke(this, new EventArgs());
    }

    [Serializable]
    public class GameData
    {
        public int score;
        public bool isBandLeader;
        public Color color;
    }

    public static string MenuActionMap = "Menu";
    public static string PlayActionMap = "Player";
}
