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

    //Plays sound effect when swinging. TODO: Chat with May to see if this can be done in a better way
    //since I don't have enough knowledge to know if this needs to be an interface or can just be a static method.
    protected override void PlaySound()
    {
        
        
        sfxSource.clip = sfx[Random.Range(0, sfx.Length)];
        sfxSource.pitch = Random.Range(1-PitchmodPercentage, 1+PitchmodPercentage);
        sfxSource.Play();
    }


}
