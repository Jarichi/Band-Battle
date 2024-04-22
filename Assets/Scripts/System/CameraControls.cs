using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector2 minPosition;
    [SerializeField]
    private Vector2 maxPosition;

    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(player.position.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(player.position.y, minPosition.y, maxPosition.y);
        transform.position= pos;
    }
}
