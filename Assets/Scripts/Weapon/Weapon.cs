using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : MonoBehaviour
{
    public string combatAnimationName;
    [Range(0f, 3f)]
    public float combatAnimationTime;
    protected Animator animator;
    protected new Collider2D collider;
    public AudioClip[] sfx;
    private AudioSource sfxSource;
    private PlayerCombat wielder;

    [Range(0, .2f)]
    public float PitchmodPercentage;

    [Range(.5f, 10f)]
    public float cooldown;

    [Range(.1f, 5f)]
    public float startupTime;

    [Range(.1f, 10f)]
    public float activeHitboxTime;

    [SerializeField]
    private bool allowVerticalAttack;
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        sfxSource = GetComponent<AudioSource>();
    }

    public void SetWielder(PlayerCombat wielder)
    {
        this.wielder = wielder;
    }

    public void ExecuteAttack(AttackDirection direction)
    {
        if (!allowVerticalAttack && (direction == AttackDirection.North || direction == AttackDirection.South)) return;

        var initialPosition = transform.localPosition;
        var initialRotation = transform.localRotation;
        PositionWeaponOnAttack(direction);
        StartCoroutine(ResetPosition(initialPosition, initialRotation));
        StartCoroutine(EnableHitbox());
    }

    private IEnumerator ResetPosition(Vector3 initialPosition, Quaternion initialRotation)
    {
        yield return new WaitForSeconds(cooldown);
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }

    private IEnumerator EnableHitbox()
    {
        yield return new WaitForSeconds(startupTime);
        collider.enabled = true;
        PlaySound();
        StartCoroutine(DisableHitbox());
    }
    private IEnumerator DisableHitbox()
    {
        yield return new WaitForSeconds(activeHitboxTime);
        collider.enabled = false;
    }

    protected abstract void PositionWeaponOnAttack(AttackDirection direction);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isDamagable = collision.gameObject.TryGetComponent(out IDamageable target);
        if (isDamagable)
        {
            target.OnDamage(wielder);
        }
    }

    private void PlaySound()
    {
        sfxSource.clip = sfx[UnityEngine.Random.Range(0, sfx.Length)];
        sfxSource.pitch = UnityEngine.Random.Range(1f - PitchmodPercentage, 1f + PitchmodPercentage);
        sfxSource.Play();
    }

}
