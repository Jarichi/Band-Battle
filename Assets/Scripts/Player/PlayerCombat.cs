using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackDirection
{
    North,
    East,
    South,
    West
}

public class PlayerCombat : MonoBehaviour
{
    private bool onCooldown;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Attack(AttackDirection.West);
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Attack(AttackDirection.East);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Attack(AttackDirection.North);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Attack(AttackDirection.South);
        }

    }
    void Attack(AttackDirection direction)
    {
        if (onCooldown)
        {
            return;
        }

        if (gameObject.transform.childCount > 0)
        {
            Weapon weapon = gameObject.transform.GetComponentInChildren<Weapon>();
            weapon.ExecuteAttack(direction);
            onCooldown = true;
            StartCoroutine(EndCooldown(weapon.cooldown));
        }
    }

    private IEnumerator EndCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onCooldown = false;
    }

}
