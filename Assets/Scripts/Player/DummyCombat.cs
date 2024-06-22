using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DummyCombat : MonoBehaviour, IDamageable
{
    public Animator animator;
    public int Hitpoints;
    [SerializeField] private ParticleSystem DamageParticles;
    private ScreenShake screenShake;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        screenShake = FindFirstObjectByType<ScreenShake>();
    }

    void Update()
    {
        if (Hitpoints <= 0)
        {
            Die(null);            
        }
    }

    public void OnDamage(int damage, PlayerEntity attacker)
    {
        animator.SetTrigger("Hit");
        Hitpoints =- damage;
        screenShake.Shake();
        SpawnParticles();
    }

    private void SpawnParticles()
    {
        Instantiate(DamageParticles, transform.position, Quaternion.identity);
    }

    public void Die(PlayerEntity cause)
    {
        Destroy(gameObject);
    }
}
