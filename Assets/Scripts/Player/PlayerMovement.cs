using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private bool canMove;
    public float moveSpeed;
    public bool isMoving = false;
    public Rigidbody2D rb; 

    public Animator animator;
    private Vector2 moveDirection; 
    private PlayerInputController input;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        input = GetComponent<PlayerInputController>();
    }

    void Update()
    {
        ProcessInput();
    }

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

        float moveX = input.HorizontalMovement;
        float moveY = input.VerticalMovement;

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
