using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : MonoBehaviour
{
    protected class WeaponPositioning
    {
        public static readonly WeaponPositioning zero = new(Vector2.zero, Vector3.zero);
        public readonly Vector2 position;
        public readonly Vector3 rotation;
        public WeaponPositioning(Vector2 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }
    public string combatAnimationName;
    [Range(0f, 3f)]
    public float combatAnimationTime;
    protected Animator animator;
    protected new Collider2D collider;
    public AudioClip[] sfx;
    [SerializeField]
    public int damage;
    private AudioSource sfxSource;
    private PlayerEntity wielder;

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
    protected void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        sfxSource = GetComponent<AudioSource>();
    }

    public void SetWielder(PlayerEntity wielder)
    {
        this.wielder = wielder;
    }

    public void ExecuteAttack(Vector2 direction)
    {
        var initialPosition = transform.localPosition;
        var initialRotation = transform.localRotation;
        PositionWeaponOnAttack(direction);
        StartCoroutine(ResetPosition(initialPosition, initialRotation));
        StartCoroutine(EnableHitbox());
    }

    public bool CanAttackInDirection(Vector2 direction)
    {
        return !(!allowVerticalAttack && (direction == Vector2.up || direction == Vector2.down));
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

    protected virtual void PositionWeaponOnAttack(Vector2 direction)
    {
        animator.SetTrigger("attack");
        var rotation = transform.localRotation.eulerAngles;
        var p = WeaponPositioning.zero;
        if (direction.y > 0)
        {
            p = UpPosition();
        }
        else if (direction.x > 0)
        {
            p = RightPosition();
        }
        else if (direction.y < 0)
        {
            p = DownPosition();
        }
        else if (direction.x < 0)
        {
            p = LeftPosition();
        }
        transform.localPosition = p.position;
        transform.localRotation = Quaternion.Euler(p.rotation);
    }

    protected abstract WeaponPositioning UpPosition();
    protected abstract WeaponPositioning RightPosition();
    protected abstract WeaponPositioning DownPosition();
    protected abstract WeaponPositioning LeftPosition();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isDamagable = collision.gameObject.TryGetComponent(out IDamageable target);
        if (isDamagable)
        {
            target.OnDamage(damage, wielder);
        }
    }

    private void PlaySound()
    {
        sfxSource.clip = sfx[UnityEngine.Random.Range(0, sfx.Length)];
        sfxSource.pitch = UnityEngine.Random.Range(1f - PitchmodPercentage, 1f + PitchmodPercentage);
        sfxSource.Play();
    }

}
