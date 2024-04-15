using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //animator.SetBool("attack", true);
            animator.SetTrigger("attack");
        }
    }
}
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
            if (playerTransform != null)
            {
                this.GetComponent<Transform>().position = playerTransform.position;
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                isPlaying = true;
                OnPlaying();
                Debug.Log(isPlaying);

            }

            //check if is playing the instrument currently
            if (isPlaying)
            {
                //check if you want to enter combat mode
                if (Input.GetKeyDown(KeyCode.I))
                {
                    Debug.Log("in Combat");
                    GameObject.Instantiate(weapon, playerTransform);
                    GameObject.Destroy(this);
                }
            }
        }


    }


}
