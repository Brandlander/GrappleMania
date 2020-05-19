using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField]Firing grappleGun;

    public Firing GrappleGun
    {
        get
        {
            return grappleGun;
        }
    }

    void Update()
    {
        // Can't shoot if running
        if (GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.RUNNING)
            return;

        // Can't shoot if not aiming
        if (GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.IDLE)
            return;
        if (GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.RELOADING)
            return;

        if (GameManager.Instance.InputController.Fire1 && GameManager.Instance.InputController.Fire2)
        {
            grappleGun.Fire();
        }
    }
}
