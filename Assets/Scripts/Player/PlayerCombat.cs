using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour, IDamageable
{
    [Range(0f, 3f)]
    public float invincibilityTime;

    public bool inCombat { get; private set; }

    private PlayerEntity playerEntity;
    private bool onCooldown;
    private bool invincible;
    private Weapon currentWeapon;
    private PlayerInput input;
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
        playerEntity= GetComponent<PlayerEntity>();
        input = playerEntity.Player.Input;
        rigidbody = GetComponent<Rigidbody2D>();
        StartingHP = Hitpoints;
    }
    private void Awake()
    {
        screenShake = FindFirstObjectByType<ScreenShake>();
    }

    void Update()
    {
        var attackDirection = input.actions["Attack"].ReadValue<Vector2>();
        engageCombatUI.SetActive(allowCombat && !inCombat);
        if (attackDirection.x != 0 || attackDirection.y != 0)
        {
            Attack(attackDirection);
        }

    }

    public void Engage(GameObject weapon)
    {
        invincible = true;
        Destroy(GetComponentInChildren<GuitarMinigame>().gameObject);
        var instrument = playerEntity.Rhythm.ChosenInstrument;
        instrument.DeleteInstrument();
        Game.Instance.DisableAudioChannel(instrument.id);
        StartCoroutine(TransitionToCombat(weapon));
        inCombat = true;

        var loadResource = Resources.Load<GameObject>("Prefabs/UI/Health Bar");

        GameObject healthBar = Instantiate(loadResource, this.transform);

        this.healthBar = healthBar.GetComponent<HealthBar>();

        var playerColour = playerEntity.Player.data.color;
        this.healthBar.SetColour(playerColour);
    }

    private IEnumerator TransitionToCombat(GameObject weapon)
    {
        Weapon weaponInfo = weapon.GetComponent<Weapon>();
        playerEntity.Movement.animator.SetTrigger(weaponInfo.combatAnimationName);
        yield return new WaitForSeconds(weaponInfo.combatAnimationTime);
        playerEntity.Movement.Enable();

        GameObject weaponInst = Instantiate(weapon, transform);
        weaponInst.transform.parent = transform;
        currentWeapon = weaponInst.GetComponent<Weapon>();
        currentWeapon.SetWielder(playerEntity);
        invincible = false;
    }

    public void Stop()
    {
        inCombat = false;
    }

    void Attack(Vector2 direction)
    {
        if (onCooldown || !inCombat)
        {
            return;
        }

        if (!currentWeapon.CanAttackInDirection(direction)) return;
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

    public void OnDamage(int damage, PlayerEntity attacker)
    {
        if (invincible)
        {
            return;
        }

        if (!inCombat)
        {
            Engage(playerEntity.Rhythm.ChosenInstrument.weapon);
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

    public void Die(PlayerEntity cause)
    {
        dead = true;
        playerEntity.Movement.animator.SetTrigger("Death");
        playerEntity.Movement.Disable();
        playerEntity.Player.data.isBandLeader = false;
        PlayerRhythm playerRhythm = playerEntity.Rhythm;
        PlayerRhythm attackerRhythm = cause.Rhythm;
        var stolenPoints = playerRhythm.GetScore() * .20d;
        playerRhythm.DecreaseScore(stolenPoints);
        attackerRhythm.AddScore(stolenPoints);

        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(gameObject.transform.GetChild(i).gameObject);
        }

        var survivors = PlayerList.Get().Where(p =>
            !p.Entity.Combat.dead
        );
        if (survivors.Count() <= 1 ) {
            Debug.Log("one player remaining!");
            var lastManStanding = survivors.ElementAt(0);
            lastManStanding.data.isBandLeader = true;
            Game.Instance.End();
        }
    }
}
