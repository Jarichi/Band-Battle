using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Instrument : MonoBehaviour
{
    protected abstract void OnPlaying();
    public GameObject weapon;
    protected bool inRange = false;
    protected bool isPlaying = false;
    private Transform playerTransform;


    public BoxCollider2D tr;

    void Start()
    {
        tr = GetComponent<BoxCollider2D>();
    }
    //proximity detection
    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        Debug.Log(inRange);
        playerTransform = col.GetComponentInParent<Transform>();
    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
        Debug.Log("Is in range: " + inRange);

    }

    void Update()
    {
        if (inRange)
        {


            //check if you want to play this instrument.
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (playerTransform != null)
                {
                    //sets the location of this object to the location of the player.
                    this.GetComponent<Transform>().position = playerTransform.position;
                }

                isPlaying = true;
                OnPlaying(); 
                Debug.Log(isPlaying);
                //TODO: add a way to tell the instrument which object currently is interacting with it. Also add a check to verify what
                //instrument is within range to avoid picking up 2 instruments at once.
                //note: OnPlaying() might need to be changed to a function that deletes the active hitbox to prevent weird collision bug.

            }

            //check if is playing the instrument currently
            if (isPlaying)
            {
                //check if you want to enter combat mode
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Debug.Log("in Combat");
                    GameObject weaponInst = GameObject.Instantiate(weapon, playerTransform);
                    weaponInst.transform.parent = playerTransform;
                    GameObject.Destroy(gameObject);
                }
            }
        }


    }


}
