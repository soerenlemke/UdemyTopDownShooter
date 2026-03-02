using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls _controls;
    private CharacterController _characterController;

    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    private Vector3 _movementDirection;
    
    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private void Awake()
    {
        _controls = new PlayerControls();
        
        _controls.Character.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Character.Movement.canceled += _ => _moveInput = Vector2.zero;
        
        _controls.Character.Aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
        _controls.Character.Aim.canceled += _ => _aimInput = Vector2.zero;
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        _movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (_movementDirection.magnitude > 0)
        {
            _characterController.Move(_movementDirection * (Time.deltaTime * walkSpeed));
        }
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
