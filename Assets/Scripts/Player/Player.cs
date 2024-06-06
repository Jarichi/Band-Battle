using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject playerEntityPrefab;

    [HideInInspector]
    public GameObject InGameEntity {get; private set;}


    public static System.EventHandler PlayerSpawnEvent;
    public static System.EventHandler PlayerDespawnEvent;

     void Start() {
        Spawn(Vector2.zero);
     }

    public void Spawn(Vector2 position) {
        playerEntityPrefab.transform.position = position;
        InGameEntity = Instantiate(playerEntityPrefab, gameObject.transform);
        InGameEntity.name = InGameEntity.GetInstanceID().ToString();

        PlayerSpawnEvent?.Invoke(this, new EventArgs());
    }
    
    public void Despawn() {
        Destroy(InGameEntity);
        PlayerDespawnEvent?.Invoke(this, new EventArgs());
    }
}
