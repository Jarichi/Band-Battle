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

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //DamageParticles = GetComponent<ParticleSystem>(); 

    }

    private void Awake()
    {
        screenShake = GameObject.FindFirstObjectByType<ScreenShake>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hitpoints <= 0)
        {
            Die();            
        }
    }

    public void OnDamage(PlayerCombat attacker)
    {
        animator.SetTrigger("Hit");
        Hitpoints--;
        screenShake.Shake();
        SpawnParticles();
    }

    private void SpawnParticles()
    {
        Instantiate(DamageParticles, transform.position, Quaternion.identity);
        
    }

    public void Die()
    {
        GameObject.Destroy(gameObject);
        Debug.Log("die");
    }
}
