using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;


public class Microphone : MonoBehaviour
{
    public bool inRange = false;
    public bool isPlaying = false;
    public bool inCombat = false;
    private bool spawned = false;
    public BoxCollider2D tr;
    public Animator animator;

    public GameObject emptyInstrument;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        tr = GetComponent<BoxCollider2D>();
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        inRange = true;
        Debug.Log(inRange);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        inRange = false;
        Debug.Log("Is in range: " + inRange);
    }


    
    
    // Update is called once per frame
    void Update()
    {
        //check if in range of the object (player hitbox intersects the trigger hitbox)
        if (inRange) {
            if(Input.GetKeyDown(KeyCode.U))
            {
                isPlaying = true;
                Debug.Log(isPlaying);

            }

            //check if is playing the instrument currently
            if (isPlaying) {

                //spawn how the object looks while playing the instrument ONCE
                while (!spawned)
                {
                    Instantiate(emptyInstrument, new Vector2(0, 2), Quaternion.identity);
                    spawned = true;

                }

                //check if you want to enter combat mode
                if(Input.GetKeyDown(KeyCode.I))
                {
                    isPlaying = false;
                    inCombat = true;
                    animator.SetBool("IsWeapon", inCombat);
                    Debug.Log(inCombat);
                }
            }

            
        }



    }
}
