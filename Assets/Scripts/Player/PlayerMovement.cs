using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool canMove = false;
    public float moveSpeed;
    public Rigidbody2D rb; 

    public Animator animator;
    public Vector2 MoveDirection { get; private set; }
    private PlayerInput input;
    
    void Start()
    {
        input = GetComponent<PlayerEntity>().Player.Input;
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
        MoveDirection = input.actions["Move"].ReadValue<Vector2>();
    }

    void Move()
    {
        if (!canMove) return;

        transform.Translate(moveSpeed * Time.deltaTime * MoveDirection);

        var x = MoveDirection.normalized.x;
        var y = MoveDirection.normalized.y;
        animator.SetFloat("xVelocity", x);
        animator.SetFloat("yVelocity", y);
        animator.SetBool("isMoving", !(x == 0f && y == 0f));
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
