using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Math;


public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed; //movement speed
    public Rigidbody2D rb; //rigid body with reference rb
    private Vector2 moveDirection; //vector with movement direction
    public bool isMoving = false;

    //Animator animator;
    //runs once
    void Start()
    {
        //animator = GetComponent<animator>();

    }

    // Update is called once per frame (dependent on device)
    void Update()
    {
        ProcessInput();


    }


    //Call every fixed framerate (independent of device)
    void FixedUpdate()
    {
        move();
    }

    void ProcessInput()
    {
        //process movement inputs and store values in floats
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        /*if (moveX != 0f || moveY != 0f)
        {
            animator.SetBool("isMoving", true);
            isMoving = true;
        } else
        {
            animator.SetBool("isMoving", false); //might replace with isMoving !isMoving
            isMoving= false;
        }
        */

        moveDirection = new Vector2(moveX, moveY); //create movement vector


    }

    void move()
    {
        //map vector to rigidbody (hitbox) and calculate movement distance based on movememnt speed.
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        //animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        //animator.SetFloat("yVelocity", Math.Abs(rb.velocity.y));


    }
}
