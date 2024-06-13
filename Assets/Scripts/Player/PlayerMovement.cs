using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool canMove = false;
    public float moveSpeed;
    public bool isMoving = false;
    public Rigidbody2D rb; 
    public float moveX, moveY;

    public Animator animator;
    private Vector2 moveDirection;
    private PlayerInput input;
    
    void Start()
    {
        input = Player.OfEntity(gameObject).Input;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        ProcessInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInput()
    {
        if (!canMove) return;

        //TODO: control stick, smooth movement
        moveX = input.actions["Horizontal Movement"].ReadValue<float>();
        moveY = input.actions["Vertical Movement"].ReadValue<float>();

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

        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
        
        animator.SetFloat("xVelocity", moveX);
        animator.SetFloat("yVelocity", moveY);
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
