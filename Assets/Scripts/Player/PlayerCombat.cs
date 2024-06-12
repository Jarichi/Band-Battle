using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private PlayerMovement movement;
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    public int Hitpoints;
    private int StartingHP;
    [HideInInspector]
    public bool dead = false;
    public bool allowCombat = false;

    private HealthBar healthBar;
    [SerializeField]
    private GameObject engageCombatUI;

    [SerializeField] private ParticleSystem DamageParticles;
    private ScreenShake screenShake;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        input = GetComponentInParent<PlayerInputController>();
        movement = GetComponent<PlayerMovement>();
        rigidbody = GetComponent<Rigidbody2D>();
        StartingHP = Hitpoints;
    }
    private void Awake()
    {
        screenShake = FindFirstObjectByType<ScreenShake>();
    }

    void Update()
    {
        engageCombatUI.SetActive(allowCombat && !inCombat);
        if (input.HorizontalAttack < 0)
        {
            Attack(AttackDirection.West);
        }
        else if (input.HorizontalAttack > 0)
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
        invincible = true;
        Destroy(GetComponentInChildren<GuitarMinigame>().gameObject);
        var instrument = movement.GetComponent<PlayerWorldInteraction>().ChosenInstrument;
        instrument.DeleteInstrument();
        Game.Instance.DisableAudioChannel(instrument.fmodParameterName);
        StartCoroutine(TransitionToCombat(weapon, movement));
        inCombat = true;

        var loadResource = Resources.Load<GameObject>("Prefabs/UI/Health Bar");

        GameObject healthBar = Instantiate(loadResource, this.transform);

        this.healthBar = healthBar.GetComponent<HealthBar>();

        var playerColour = transform.root.GetComponent<Player>().data.color;
        this.healthBar.SetColour(playerColour);
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

    public void OnDamage(int damage, PlayerCombat attacker)
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
        Hitpoints -= damage;
        screenShake.Shake();
        SpawnParticles();

        healthBar.UpdateHealthbar(StartingHP, Hitpoints);

        if (Hitpoints <= 0)
        {
            Die(attacker);
        }
        spriteRenderer.color = Color.red;

        if (currentWeapon.TryGetComponent<DrumWeapon>(out var drumweapon))
        {
            print("weapon is drums, turn red");
            drumweapon.GetComponent<SpriteRenderer>().color = Color.red;
        }

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

        if (!currentWeapon.IsDestroyed() && currentWeapon.TryGetComponent<DrumWeapon>(out var drumweapon))
        {
            print("weapon is drums, turn white");

            drumweapon.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public void Die(PlayerCombat cause)
    {
        dead = true;
        movement.animator.SetTrigger("Death");
        movement.Disable();
        Player.OfEntity(gameObject).data.isBandLeader = false;
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        var survivors = PlayerList.Get().Where(p =>
            !p.InGameEntity.GetComponent<PlayerCombat>().dead
        );
        if (survivors.Count() <= 1 ) {
            Debug.Log("one player remaining!");
            var lastManStanding = survivors.ElementAt(0);
            lastManStanding.data.isBandLeader = true;
            print(lastManStanding);
            Game.Instance.End();
        }
    }
}
