using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class CameraControls : MonoBehaviour
{
    [InspectorButton("PrintPlayers")]
    public bool printPlayers;

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
    private List<GameObject> players;
    private Camera cam;
    private void Start()
    {
        players = new List<GameObject>();
        cam = GetComponent<Camera>();
        PlayerInputController.PlayerJoinEvent += HandleJoinEvent;
        PlayerInputController.PlayerLeaveEvent += HandleLeaveEvent;
    }

    void Update()
    {
        Move();
        Zoom();
    }

    // TODO: move the player list to PlayerInputController
    private void HandleJoinEvent(object sender, EventArgs e)
    {
        players.Add((GameObject)sender);
    }

    private void HandleLeaveEvent(object sender, EventArgs e)
    {
        players.Remove((GameObject)sender);
    }

    void Zoom()
    {
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
