using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIScreen : ScreenUICore
{
    void Awake()
    {
        ScreenUICore.Show(GetComponent<ScreenUICore>());
    }
}
