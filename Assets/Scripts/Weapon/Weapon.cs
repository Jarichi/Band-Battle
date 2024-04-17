using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Weapon : MonoBehaviour
{
    protected Animator animator;
    [Range(0f, 10f)]
    public float cooldown;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ExecuteAttack(AttackDirection direction)
    {
        var initialPosition = transform.localPosition;
        var initialRotation = transform.localRotation;
        PositionWeapon(direction);
        StartCoroutine(ResetPosition(initialPosition, initialRotation));

    }

    private IEnumerator ResetPosition(Vector3 initialPosition, Quaternion initialRotation)
    {
        yield return new WaitForSeconds(cooldown);
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }


    protected abstract void PositionWeapon(AttackDirection direction);

}
