using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MicWeapon : Weapon
{
    protected override void PositionWeapon(AttackDirection direction)
    {
        animator.SetTrigger("attack");
        var position = transform.localPosition;
        var rotation = transform.localRotation.eulerAngles;
        switch (direction)
        {
            case AttackDirection.North:
                position.y += 1;
                rotation.z = 0;
                break;

            case AttackDirection.East:
                position.x += 1;
                rotation.z = -90;
                break;

            case AttackDirection.South:
                position.y -= 1;
                rotation.z = -180;
                break;

            case AttackDirection.West:
                position.x -= 1;
                rotation.z = 90;
                break;
        }
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(rotation);
    }
}
