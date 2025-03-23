using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAttack
{
    float damage {get; set;}
    void OnAttack();
}
