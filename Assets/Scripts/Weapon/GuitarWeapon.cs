using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarWeapon : Weapon
{
    protected override void PositionWeaponOnAttack(AttackDirection direction)
    {
        animator.SetTrigger("attack");
    }
}
