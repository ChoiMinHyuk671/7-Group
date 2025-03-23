using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageable
{
    event Action OnDamage;
    void RPC_TakeDamage(float damage);
}
