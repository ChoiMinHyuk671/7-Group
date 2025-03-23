using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Gun : NetworkBehaviour
{
    public static Gun Instance;
    private void Awake() {
        Instance = this;
    }
}
