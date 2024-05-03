using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerLine : MonoBehaviour
{

    private BoxCollider2D Hitbox;
    public float marginOfError;
    private Vector2 initialPosition;

    public int Score {  get; private set; }

    private void Start()
    {
        Hitbox =  GetComponent<BoxCollider2D>();
        Hitbox.enabled = false;
        initialPosition = Hitbox.offset;
    }

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Up");
            initialPosition.x = -1.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Down");
            initialPosition.x = -0.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Left");
            initialPosition.x = 0.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Right");
            initialPosition.x = 1.5f;
            Hitbox.offset = initialPosition;
            ToggleHitbox();
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT");
        //kill note
        //GameObject.Destroy();
        //increment score
        Score++;

    }

    private void ToggleHitbox()
    {
        Hitbox.enabled = true;
        StartCoroutine(DisableHitbox());
    }


    private IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(marginOfError);
        Hitbox.enabled = false;
    }


}

