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

public class PlayerCombat : MonoBehaviour, IDamageable
{
    [Range(0f, 3f)]
    public float invincibilityTime;

    public bool inCombat { get; private set; }

    private bool onCooldown;
    private bool invincible;
    private Weapon currentWeapon;
    public int Hitpoints;

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

    public void Engage(GameObject weapon, PlayerMovement movement, string animationTriggerName, float animationLength)
    {
        StartCoroutine(TransitionToCombat(weapon, movement, animationTriggerName, animationLength));
        inCombat = true;
    }

    private IEnumerator TransitionToCombat(GameObject weapon, PlayerMovement movement, string triggerName, float length)
    {
        movement.animator.SetTrigger(triggerName);
        yield return new WaitForSeconds(length);
        movement.Enable();

        GameObject weaponInst = GameObject.Instantiate(weapon, transform);
        weaponInst.transform.parent = transform;
        currentWeapon = weaponInst.GetComponent<Weapon>();
    }

    public void Stop()
    {
        inCombat = false;
    }

    void Attack(AttackDirection direction)
    {
        if (onCooldown)
        {
            return;
        }

        if (gameObject.transform.childCount > 0)
        {
            currentWeapon.ExecuteAttack(direction);
            onCooldown = true;
            StartCoroutine(EndCooldown(currentWeapon.cooldown));
        }
    }

    private IEnumerator EndCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        onCooldown = false;
    }

    public void OnDamage()
    {
        if (invincible)
        {
            return;
        }

        invincible = true;
        Debug.Log("Im HIT!");
        StartCoroutine(EndInvincibility());
    }

    private IEnumerator EndInvincibility()
    {
        yield return new WaitForSeconds(invincibilityTime);
        onCooldown = false;
    }

    public void Die()
    {
        if (Hitpoints == 0)
        {
            Debug.Log("Oh NOOoooo o o *dies*");
        }
    }
}
