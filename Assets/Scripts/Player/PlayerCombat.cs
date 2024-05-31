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
    private PlayerInputController input;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    public int Hitpoints;

    [SerializeField] private ParticleSystem DamageParticles;
    private ScreenShake screenShake;

    private void Start()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        input = GetComponent<PlayerInputController>();
        rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        screenShake = FindFirstObjectByType<ScreenShake>();
    }

    void Update()
    {

        if (input.HorizontalAttack < 0)
        {
            Attack(AttackDirection.West);
        } else if (input.HorizontalAttack > 0)
        {
            Attack(AttackDirection.East);
        }
        else if (input.VerticalAttack > 0)
        {
            Attack(AttackDirection.North);
        }
        else if (input.VerticalAttack < 0)
        {
            Attack(AttackDirection.South);
        }

    }

    public void Engage(GameObject weapon, PlayerMovement movement)
    {
        Debug.Log("transition to combat.");
        invincible = true;
        Destroy(GetComponentInChildren<GuitarMinigame>().gameObject);
        movement.GetComponent<PlayerWorldInteraction>().ChosenInstrument.DeleteInstrument();
        StartCoroutine(TransitionToCombat(weapon, movement));
        inCombat = true;
    }

    private IEnumerator TransitionToCombat(GameObject weapon, PlayerMovement movement)
    {
        Weapon weaponInfo = weapon.GetComponent<Weapon>();
        movement.animator.SetTrigger(weaponInfo.combatAnimationName);
        yield return new WaitForSeconds(weaponInfo.combatAnimationTime);
        movement.Enable();

        GameObject weaponInst = GameObject.Instantiate(weapon, transform);
        weaponInst.transform.parent = transform;
        currentWeapon = weaponInst.GetComponent<Weapon>();
        currentWeapon.SetWielder(movement.GetComponent<PlayerCombat>());
        invincible = false;
    }

    public void Stop()
    {
        inCombat = false;
    }

    void Attack(AttackDirection direction)
    {
        if (onCooldown || !inCombat)
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

    public void OnDamage(PlayerCombat attacker)
    {
        if (invincible)
        {
            return;
        }

        if (!inCombat)
        {
            Engage(GetComponent<PlayerWorldInteraction>().ChosenInstrument.weapon, GetComponent<PlayerMovement>());
            return;
        }

        invincible = true;

        var forcePower = 30f;
        var forceDirection = transform.position - attacker.transform.position;
        rigidbody.AddForce(forceDirection * forcePower, ForceMode2D.Force);
        Hitpoints--;
        screenShake.Shake();
        SpawnParticles();
        if (Hitpoints <= 0)
        {
            Die();
        }
        spriteRenderer.color = Color.red;
        StartCoroutine(EndInvincibility());
    }

    private void SpawnParticles()
    {
        Instantiate(DamageParticles, transform.position, Quaternion.identity);
    }

    private IEnumerator EndInvincibility()
    {
        yield return new WaitForSeconds(invincibilityTime);
        invincible = false;
        spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
    }


}
