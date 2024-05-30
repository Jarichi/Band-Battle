using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DrumWeapon : Weapon
{
    public float attackTime;
    private void Start()
    { 
        base.Start();
        var parent = transform.parent;
        parent.GetComponent<SpriteRenderer>().enabled = false;
    }

    protected override void PositionWeaponOnAttack(AttackDirection direction)
    {
        StartCoroutine(ToggleMovement());
        AnimationSettings(direction);
        AttackSettings(direction);

    }

    private void AnimationSettings(AttackDirection direction)
    {
        animator.SetTrigger("attack");
        switch (direction)
        {
            case AttackDirection.North:
                animator.SetFloat("xDir", 0f);
                animator.SetFloat("yDir", 1f);
                break;

            case AttackDirection.East:
                animator.SetFloat("xDir", 1f);
                animator.SetFloat("yDir", 0f);
                break;

            case AttackDirection.South:
                animator.SetFloat("xDir", 0f);
                animator.SetFloat("yDir", -1f);
                break;

            case AttackDirection.West:
                animator.SetFloat("xDir", -1f);
                animator.SetFloat("yDir", 0f);
                break;
        }
    }
    private void AttackSettings(AttackDirection direction)
    {
        var hitboxPosition = collider.offset; 
        switch (direction)
        {
            case AttackDirection.North:
                hitboxPosition = new Vector2(0, .5f);
                break;

            case AttackDirection.East:
                hitboxPosition = new Vector2(1, 0);
                break;

            case AttackDirection.South:
                hitboxPosition = new Vector2(0, -1.2f);
                break;

            case AttackDirection.West:
                hitboxPosition = new Vector2(-1, 0);
                break;
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


}

