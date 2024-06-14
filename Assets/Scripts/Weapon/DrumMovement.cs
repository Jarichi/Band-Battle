using UnityEngine;
using UnityEngine.EventSystems;

public class DrumMovement : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement parentMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        parentMovement = GetComponentInParent<PlayerMovement>();
    }

    void FixedUpdate()
    {
        Move();
    }


    void Move()
    {
        var x = parentMovement.MoveDirection.normalized.x;
        var y = parentMovement.MoveDirection.normalized.y;
        animator.SetFloat("xVelocity", x);
        animator.SetFloat("yVelocity", y);
        animator.SetBool("isMoving", !(x == 0f && y == 0f));
    }
}
