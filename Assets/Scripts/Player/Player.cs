using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject playerEntityPrefab;
    public PersistantData data;

    [HideInInspector]
    public GameObject InGameEntity {get; private set;}
    public MenuInputController MenuInputController { get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }
    private PlayerInput input;
    public bool isReady = false;


    public static EventHandler PlayerSpawnEvent;
    public static EventHandler PlayerDespawnEvent;

    void Start() {
        MenuInputController = GetComponent<MenuInputController>();
        MenuInputController.StartGamePressed += (obj => { isReady = true; });
        MenuInputController.CancelPressed += (obj => { isReady = false; });

        PlayerInputController = GetComponent<PlayerInputController>();
        input = GetComponent<PlayerInput>();
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

    public void UpdateIndex()
    {
        data.index          = PlayerList.Get().IndexOf(this) + 1;
        data.color          = PlayerList.ColorList()[data.index - 1];
        playerEntityPrefab  = PlayerList.PlayerEntitiesList()[data.index - 1];
    }

    public void SwitchActionMap(string name)
    {
        input.SwitchCurrentActionMap(name);
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
    public class PersistantData
    {
        public int index;
        public Color color;
        public int score;
    }
}
