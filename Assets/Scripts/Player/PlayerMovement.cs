using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool canMove;
    public float moveSpeed;
    public bool isMoving = false;
    public Rigidbody2D rb; 
    public float moveX, moveY;

    public Animator animator;
    private Vector2 moveDirection; 
    private PlayerInputController input;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponentInParent<PlayerInputController>();
    }


    void Update()
    {
        ProcessInput();
    }

    void FixedUpdate()
    {
        Move();
       // transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void ProcessInput()
    {
        if (!canMove) return;

        moveX = input.HorizontalMovement;
        moveY = input.VerticalMovement;

        if (moveX == 0f && moveY == 0f)
        {
            isMoving = false;
        } else {
            isMoving = true;
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        if (!canMove) return;

        transform.Translate(moveDirection * Time.deltaTime * moveSpeed);
        
        animator.SetFloat("xVelocity", input.HorizontalMovement);
        animator.SetFloat("yVelocity", input.VerticalMovement);
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
