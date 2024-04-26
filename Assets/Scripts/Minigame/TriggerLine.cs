using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerLine : SongChecker
{
    // Start is called before the first frame update
    private BoxCollider2D Hitbox;
    public float marginOfError;
    private Vector2 initialPosition;
    

    public enum NoteDirection

    {
        Up, Down, Left, Right
    }
    public NoteDirection tempNotes;


    private void Start()
    {
        Hitbox =  GetComponent<BoxCollider2D>();
        Hitbox.enabled = false;
        initialPosition = Hitbox.offset;
    }
    private void Update()
    {
        PlayerInput();
        MoveHitbox(tempNotes);


    }

    private void MoveHitbox(NoteDirection note)
    {
        switch (note)
        {
            case NoteDirection.Up:
                Debug.Log("Up");
                initialPosition.x = -1.5f;
                Hitbox.offset = initialPosition;
                //change offset according to note value -1.5
                break;
                
            case NoteDirection.Down:
                Debug.Log("Down");
                initialPosition.x = -0.5f;
                Hitbox.offset = initialPosition;
                break;

            case NoteDirection.Left:
                Debug.Log("Left");
                initialPosition.x = 0.5f;
                Hitbox.offset = initialPosition;
                break;

            case NoteDirection.Right:
                Debug.Log("Right");
                initialPosition.x = 1.5f;
                Hitbox.offset = initialPosition;
                break; 
            

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HIT");
        //destroy note prefab
        //kill note
        //increment score

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

