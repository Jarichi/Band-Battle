using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : MonoBehaviour
{
    protected Animator animator;
    protected new Collider2D collider;
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
    }

    private void Update()
    {
        
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
        IDamageable? target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            target.OnDamage();
        }
    }

}
