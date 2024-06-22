using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float smoothTime = .3f;
    [SerializeField]
    private float maxZoom = 1f;
    [SerializeField]
    private float minZoom = 5f;
    [SerializeField]
    private float zoomLimit = 10f;

    private Vector3 velocity;
    private readonly List<PlayerEntity> players = new();
    private Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();
        PlayerList.Instance.PlayerSpawnEvent.AddListener(HandleJoinEvent);
        PlayerList.Instance.PlayerDespawnEvent.AddListener(HandleLeaveEvent);
    }

    private void OnDestroy()
    {
        PlayerList.Instance.PlayerSpawnEvent.RemoveListener(HandleJoinEvent);
        PlayerList.Instance.PlayerDespawnEvent.RemoveListener(HandleLeaveEvent);
    }

    void Update()
    {
        Move();
        Zoom();
    }

    // TODO: move the player list to PlayerInputController
    private void HandleJoinEvent(PlayerEntity player)
    {
        players.Add(player);
    }

    private void HandleLeaveEvent(PlayerEntity player)
    {
        players.Remove(player);
    }

    void Zoom()
    {
        print(players.Count);
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimit);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    void Move()
    {
        if (players.Count == 0) return;


        Vector3 center = GetCenterPoint();
        Vector3 pos = center + offset;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothTime);
    }

    float GetGreatestDistance()
    {
        if (players.Count <= 0) return 0f;
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        players.ForEach(pl => bounds.Encapsulate(pl.transform.position));
        return bounds.size.x;
    }

    Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
        {
            return players[0].transform.position;
        }

        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        players.ForEach(pl => bounds.Encapsulate(pl.transform.position));
        return bounds.center;
    }
}
