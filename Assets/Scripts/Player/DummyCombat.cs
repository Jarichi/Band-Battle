using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DummyCombat : MonoBehaviour, IDamageable
{
    public Animator animator;
    public int Hitpoints;
    [SerializeField] private ParticleSystem DamageParticles;

    private ParticleSystem DamageParticleInstance;
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
        
    }

    public void OnDamage()
    {
        animator.SetTrigger("Hit");
        Hitpoints--;
        screenShake.Shake();
        SpawnParticles();
    }

    private void SpawnParticles()
    {
        //create particles at the current position.
        DamageParticles = Instantiate(DamageParticles, transform.position, Quaternion.identity);
        
    }
}
