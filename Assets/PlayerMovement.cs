using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float Gravity = 9.81f;
    
    private PlayerControls _controls;
    private CharacterController _characterController;

    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    private Vector3 _movementDirection;
    private float _verticalVelocity;

    [Header("Aim info")] 
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 _lookingDirection;
    
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
        AimTowardsMouse();
    }

    private void AimTowardsMouse()
    {
        var ray = Camera.main.ScreenPointToRay(_aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            _lookingDirection = hitInfo.point - transform.position;
            _lookingDirection.y = 0f;
            _lookingDirection.Normalize();
            
            transform.forward = _lookingDirection;
            
            aim.position = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }
    }

    private void ApplyMovement()
    {
        _movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        ApplyGravity();

        if (_movementDirection.magnitude > 0)
        {
            _characterController.Move(_movementDirection * (Time.deltaTime * walkSpeed));
        }
    }

    private void ApplyGravity()
    {
        if (!_characterController.isGrounded)
        {
            _verticalVelocity -= Gravity * Time.deltaTime;
            _movementDirection.y = _verticalVelocity;
        }
        else
        {
            _verticalVelocity = -0.5f;
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
