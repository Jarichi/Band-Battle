using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicWeapon : Weapon
{
    protected override void PositionWeapon(AttackDirection direction)
    {
        animator.SetTrigger("attack");
        var position = transform.localPosition;
        var rotation = transform.localRotation;
        switch (direction)
        {
            case AttackDirection.North:
                position.y += 1;
                break;

            case AttackDirection.East:
                position.x += 1;
                
                break;

            case AttackDirection.South:
                position.y -= 1;
                rotation.z = 90;
                break;

            case AttackDirection.West:
                position.x -= 1;
                rotation.y = -180;
                break;
        }
        transform.localPosition = position;
        transform.localRotation = rotation;
    }
}
