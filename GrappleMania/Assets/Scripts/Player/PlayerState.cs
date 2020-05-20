using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum EMoveState
    {
        IDLE,
        WALKING,
        RUNNING,
        JUMPING
    }

    public enum EWeaponState
    {
        IDLE,
        FIRING,
        AIMING,
        AIMEDFIRING,
        RELOADING
    }

    public EMoveState MoveState;
    public EWeaponState WeaponState;

    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
                m_InputController = GameManager.Instance.InputController;
            return m_InputController;
        }
    }

    private void Update()
    {
        SetMoveState();
        SetWeaponState();
    }

    void SetWeaponState()
    {
        WeaponState = EWeaponState.IDLE;

        if (InputController.Fire1)
            WeaponState = EWeaponState.FIRING;

        if (InputController.Fire2)
            WeaponState = EWeaponState.AIMING;

        if (InputController.Fire1 && InputController.Fire2)
            WeaponState = EWeaponState.AIMEDFIRING;

        if (InputController.Reload)
            WeaponState = EWeaponState.RELOADING;
    }

    void SetMoveState()
    {
        MoveState = EMoveState.WALKING;

        if (InputController.IsRunning)
            MoveState = EMoveState.RUNNING;
        if (InputController.Jump)
            MoveState = EMoveState.JUMPING;
    }
}
