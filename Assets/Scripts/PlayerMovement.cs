using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player _player;
    
    private const float Gravity = 9.81f;
    
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");
    private static readonly int ZVelocity = Animator.StringToHash("zVelocity");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    

    private PlayerControls _controls;
    private CharacterController _characterController;
    private Animator _animator;

    [Header("Movement info")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float _speed;
    private Vector3 _movementDirection;
    private float _verticalVelocity;
    private bool _isRunning;

    [Header("Aim info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 _lookingDirection;
    
    private Vector2 _moveInput;
    private Vector2 _aimInput;

    private void Start()
    {
        _player = GetComponent<Player>();
        
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();

        _speed = walkSpeed;
        
        AssignInputEvents();
    }

    private void Update()
    {
        ApplyMovement();
        AimTowardsMouse();
        AnimatorControllers();
    }
    
    private void AssignInputEvents()
    {
        _controls = _player.Controls;
        
        _controls.Character.Movement.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _controls.Character.Movement.canceled += _ => _moveInput = Vector2.zero;
        
        _controls.Character.Aim.performed += ctx => _aimInput = ctx.ReadValue<Vector2>();
        _controls.Character.Aim.canceled += _ => _aimInput = Vector2.zero;
        
        _controls.Character.Run.performed += _ =>
        {
            _speed = runSpeed;
            _isRunning = true;
        };
        _controls.Character.Run.canceled += _ =>
        {
            _speed = walkSpeed;
            _isRunning = false;
        };
    }

    private void ApplyMovement()
    {
        _movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        ApplyGravity();

        if (_movementDirection.magnitude > 0)
        {
            _characterController.Move(_movementDirection * (Time.deltaTime * _speed));
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

    private void AimTowardsMouse()
    {
        var ray = Camera.main.ScreenPointToRay(_aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            _lookingDirection = hitInfo.point - transform.position;
            _lookingDirection.y = 0f;
            _lookingDirection.Normalize();
            
            transform.forward = _lookingDirection;
            
            aim.position = new Vector3(hitInfo.point.x, transform.position.y + 1, hitInfo.point.z);
        }
    }
    
    private void AnimatorControllers()
    {
        var xVelocity = Vector3.Dot(_movementDirection.normalized, transform.right);
        var zVelocity = Vector3.Dot(_movementDirection.normalized, transform.forward);
        
        _animator.SetFloat(XVelocity, xVelocity, .1f, Time.deltaTime);
        _animator.SetFloat(ZVelocity, zVelocity,  .1f, Time.deltaTime);

        var playRunAnimation = _isRunning && _movementDirection.magnitude > 0;
        _animator.SetBool(IsRunning, playRunAnimation);
    }
}
