using UnityEngine;

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
        animator.SetFloat("xVelocity", parentMovement.moveX);
        animator.SetFloat("yVelocity", parentMovement.moveY);
        animator.SetBool("isMoving", parentMovement.isMoving);
    }
}
