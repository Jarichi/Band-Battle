using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(int damage, PlayerEntity attacker);
    public void Die(PlayerEntity cause);
    
}
