using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    
    void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    void ShootAction_OnAnyShoot(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake();
    }
}
