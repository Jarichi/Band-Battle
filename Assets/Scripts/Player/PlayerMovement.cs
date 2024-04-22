using System;
using UnityEngine;
//using Math;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool canMove;
    public float moveSpeed; //movement speed
    public bool isMoving = false;
    public Rigidbody2D rb; //rigid body with reference rb

    public Animator animator;
    private Vector2 moveDirection; //vector with movement direction

    //Animator animator;
    //runs once
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame (dependent on device)
    void Update()
    {
        ProcessInput();
    }


    //Call every fixed framerate (independent of device)
    void FixedUpdate()
    {
        Move();
        Vector3 minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), transform.position.z);
    }

    void ProcessInput()
    {

        if (!canMove) return;

        //process movement inputs and store values in floats
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX == 0f && moveY == 0f)
        {
            isMoving = false;
        } else {
            isMoving = true;
        }

        

        moveDirection = new Vector2(moveX, moveY).normalized; //create movement vector

        

    }

    void Move()
    {
        if (!canMove) return;

        //map vector to rigidbody (hitbox) and calculate movement distance based on movememnt speed.
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        animator.SetFloat("xVelocity", rb.velocity.x/moveSpeed);
        animator.SetFloat("yVelocity", rb.velocity.y/moveSpeed);
        animator.SetBool("isMoving", isMoving);
    }

    public void Disable()
    {
        canMove = false;
    }

    internal void Enable()
    {
        canMove = true;
    }
}
