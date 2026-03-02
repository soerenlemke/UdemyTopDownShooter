using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls _controls;

    public Vector2 moveInput;
    public Vector2 aimInput;

    private void Awake()
    {
        _controls = new PlayerControls();
        
        _controls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        _controls.Character.Movement.canceled += _ => moveInput = Vector2.zero;
        
        _controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        _controls.Character.Aim.canceled += _ => aimInput = Vector2.zero;
    }
    
    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
