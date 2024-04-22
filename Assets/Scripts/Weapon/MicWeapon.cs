using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MicWeapon : Weapon
{
    protected override void PositionWeaponOnAttack(AttackDirection direction)
    {
        animator.SetTrigger("attack");
        var position = transform.localPosition;
        var rotation = transform.localRotation.eulerAngles;
        switch (direction)
        {
            case AttackDirection.North:
                position = new Vector2(0, .5f);
                rotation.z = 90;
                break;

            case AttackDirection.East:
                position = new Vector2(1, 0);
                break;

            case AttackDirection.South:
                position = new Vector2(0, -1.2f);
                rotation.z = -90;
                break;

            case AttackDirection.West:
                position = new Vector2(-1, 0);
                rotation.z = -180;
                break;
        }
        transform.localPosition = position;
        transform.localRotation = Quaternion.Euler(rotation);
    }

    protected override void PlaySound()
    {
        sfxSource.clip = sfx[Random.Range(0, sfx.Length)];
        sfxSource.pitch = Random.Range(1 - PitchmodPercentage, 1 + PitchmodPercentage);
        sfxSource.Play();
    }
}

