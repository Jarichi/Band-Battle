using System.Collections;
using UnityEngine;

public class DrumWeapon : Weapon
{
    public float attackTime;   
    private void Start()
    {
        base.Start();
        var parent = transform.parent;
        parent.GetComponent<SpriteRenderer>().enabled = false;
    }

    protected override void PositionWeaponOnAttack(Vector2 direction)
    {
        StartCoroutine(ToggleMovement());
        AnimationSettings(direction);
        AttackSettings(direction);

    }

    private void AnimationSettings(Vector2 direction)
    {
        animator.SetTrigger("attack");
        if (direction.y > 0)
        {
            animator.SetFloat("xDir", 0f);
            animator.SetFloat("yDir", 1f);
        } else if (direction.x > 0)
        {
            animator.SetFloat("xDir", 1f);
            animator.SetFloat("yDir", 0f);
        } else if (direction.y < 0)
        {
            animator.SetFloat("xDir", 0f);
            animator.SetFloat("yDir", -1f);
        }
        else if (direction.x < 0)
        {
            animator.SetFloat("xDir", -1f);
            animator.SetFloat("yDir", 0f);
        }
    }

    private void AttackSettings(Vector2 direction)
    {
        var hitboxPosition = collider.offset;
        if (direction.y > 0)
        {
            hitboxPosition = new Vector2(0, 1.2f);
        }
        else if (direction.x > 0)
        {
            hitboxPosition = new Vector2(1, 0);
        }
        else if (direction.y < 0)
        {
            hitboxPosition = new Vector2(0, -1.2f);
        }
        else if (direction.x < 0)
        {
            hitboxPosition = new Vector2(-1, 0);
        }
        collider.offset = hitboxPosition;
    }

    private IEnumerator ToggleMovement()
    {
        var movement = GetComponentInParent<PlayerMovement>();
        movement.Disable();
        yield return new WaitForSeconds(attackTime);
        movement.Enable();

    }

    protected override WeaponPositioning UpPosition()
    {
        throw new System.NotImplementedException();
    }

    protected override WeaponPositioning RightPosition()
    {
        throw new System.NotImplementedException();
    }

    protected override WeaponPositioning DownPosition()
    {
        throw new System.NotImplementedException();
    }

    protected override WeaponPositioning LeftPosition()
    {
        throw new System.NotImplementedException();
    }
}

