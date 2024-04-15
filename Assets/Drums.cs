using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drums : MonoBehaviour
{
   
    public bool inRange = false;
    public bool isPlaying = false;
    public bool inCombat = false;
    public BoxCollider2D tr;
    public Animator animator;
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
        if(animator != null)
        {
           if (inRange) 
            {
                if(Input.GetKeyDown(KeyCode.U))
                {
                    isPlaying = true;
                    Debug.Log(isPlaying);
                }

                if (isPlaying) 
                {
                    if(Input.GetKeyDown(KeyCode.I))
                    {
                        isPlaying = false;
                        inCombat = true;
                        animator.SetBool("isWeapon", inCombat);
                        Debug.Log(inCombat);
                    }
                }

            
            }
           
        }
    }

    
}
