using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DummyCombat : MonoBehaviour, IDamageable
{
    public Animator animator;
    public int Hitpoints;
    [SerializeField] private ParticleSystem DamageParticles;

    private ParticleSystem DamageParticleInstance; //redundant

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();  
        //DamageParticles = GetComponent<ParticleSystem>(); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDamage()
    {
        animator.SetTrigger("Hit");
        SpawnParticles();
        Hitpoints--;
        
    }

    public void Die()
    {
        if (Hitpoints == 0)
        {
            Debug.Log("*dies*");
        }
    }

    private void SpawnParticles()
    {
        //create particles at the current position.
        Debug.Log("spawn particle object");
        Instantiate(DamageParticles, transform.position, Quaternion.identity);
        
    }
}
