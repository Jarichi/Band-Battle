using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    private Transform transform;
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector2 boundsMin;
    [SerializeField]
    private Vector2 boundsMax;
    private void Start()
    {
        transform= GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(player.position.x, boundsMin.x, boundsMax.x);
        pos.y = Mathf.Clamp(player.position.y, boundsMin.y, boundsMax.y);
        transform.position= pos;
    }
}
