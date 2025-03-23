using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

interface IStats
{
    float hp {get; set;}
    float maxHp {get; set;}
    public NetworkBool IsDead {get; set;}
}
