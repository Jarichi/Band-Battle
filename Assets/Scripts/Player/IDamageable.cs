using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void OnDamage(int damage, PlayerCombat attacker);
    public void Die(PlayerCombat cause);
    
}
