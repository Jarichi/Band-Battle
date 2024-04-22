using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GuitarWeapon : Weapon
{
    protected override void PositionWeaponOnAttack(AttackDirection direction)
    {
        animator.SetTrigger("attack");
        var position = transform.localPosition;
        var rotation = transform.localRotation.eulerAngles;
        switch (direction)
        {
            case AttackDirection.North:
                break;

            case AttackDirection.East:
                
                break;

            case AttackDirection.South:
                break;

            case AttackDirection.West:
                position = new Vector2(-0.30f, position.y);
                rotation.y = 180;
                break;
        }
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(rotation);
    }
}
