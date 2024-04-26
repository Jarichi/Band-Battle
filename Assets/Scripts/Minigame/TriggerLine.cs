using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerLine : SongChecker
{
    // Start is called before the first frame update
    private BoxCollider2D Hitbox;
    public float marginOfError;

    enum Notevalue
    {
        Up, Down, Left, Right
    }

    private void Start()
    {
       Hitbox =  GetComponent<BoxCollider2D>();
        Hitbox.enabled = false;
    }
    private void Update()
    {
        PlayerInput();


    }

    private void MoveHitbox(Notevalue note)
    {
        switch (note)
        {
            case Notevalue.Up:
                Debug.Log("Up");

                //change offset according to note value -1.5
                break;
                
            case Notevalue.Down:
                Debug.Log("Down");
                break;

            case Notevalue.Left:
                Debug.Log("Left");
                break;

            case Notevalue.Right:
                Debug.Log("Right");
                break; 
            

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT");
        //destroy note prefab
    }

    private void PlayerInput ()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHitbox();
        }
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

