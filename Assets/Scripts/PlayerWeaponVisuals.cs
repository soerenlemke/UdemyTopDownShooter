using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    private static readonly int Reload = Animator.StringToHash("Reload");
    private static readonly int WeaponGrabType = Animator.StringToHash("WeaponGrabType");
    private static readonly int WeaponGrab = Animator.StringToHash("WeaponGrab");
    private static readonly int BusyGrabbingWeapon = Animator.StringToHash("BusyGrabbingWeapon");

    private Animator _animator;
    private bool _isGrabbingWeapon;
    
    [SerializeField] private Transform[] gunTransforms;
    [SerializeField]  private Transform pistol;
    [SerializeField]  private Transform revolver;
    [SerializeField]  private Transform autoRifle;
    [SerializeField]  private Transform shotgun;
    [SerializeField]  private Transform rifle;

    private Transform _currentGun;

    [Header("Rig")]
    [SerializeField] private float rigWeightIncreaseRate;
    private bool _shouldIncreaseRigWeight;
    private Rig _rig;
    
    [Header("Left hand IK")] 
    [SerializeField] private float leftHandIkWeightIncreaseRate;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private Transform leftHandIKTarget;
    private bool _shouldIncreaseLeftHandIKWeight;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _rig = GetComponentInChildren<Rig>();
        
        SwitchOn(pistol);
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && !_isGrabbingWeapon)
        {
            _animator.SetTrigger(Reload);
            ReduceRigWeight();
        }

        UpdateRigWeiht();
        UpdateLeftHandIKWeight();
    }

    private void UpdateLeftHandIKWeight()
    {
        if (_shouldIncreaseLeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIkWeightIncreaseRate * Time.deltaTime;

            if (leftHandIK.weight >= 1)
            {
                _shouldIncreaseLeftHandIKWeight = false;
            }
        }
    }

    private void UpdateRigWeiht()
    {
        if (_shouldIncreaseRigWeight)
        {
            _rig.weight += rigWeightIncreaseRate * Time.deltaTime;

            if (_rig.weight >= 1)
            {
                _shouldIncreaseRigWeight = false;
            }
        }
    }

    private void ReduceRigWeight()
    {
        _rig.weight = 0.15f;
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        ReduceRigWeight();
        _animator.SetFloat(WeaponGrabType, (float)grabType);
        _animator.SetTrigger(WeaponGrab);

        SetBusyGrabbingWeaponTo(true);
    }

    public void SetBusyGrabbingWeaponTo(bool busy)
    {
        _isGrabbingWeapon = busy;
        _animator.SetBool(BusyGrabbingWeapon, _isGrabbingWeapon);
    }

    public void MaximizeRigWeight() => _shouldIncreaseRigWeight = true;
    public void MaximizeLeftHandWeight() => _shouldIncreaseLeftHandIKWeight = true;

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);
        _currentGun = gunTransform;
        
        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        foreach (var gunTransform in gunTransforms)
        {
            gunTransform.gameObject.SetActive(false);
        }
    }

    private void AttachLeftHand()
    {
        var targetTransform = _currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;
        leftHandIKTarget.localPosition = targetTransform.localPosition;
        leftHandIKTarget.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (var i = 1; i < _animator.layerCount; i++)
        {
            _animator.SetLayerWeight(i, 0);
        }
        
        _animator.SetLayerWeight(layerIndex, 1);
    }
    
    private void CheckWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(pistol);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchOn(revolver);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.SideGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchOn(autoRifle);
            SwitchAnimationLayer(1);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchOn(shotgun);
            SwitchAnimationLayer(2);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchOn(rifle);
            SwitchAnimationLayer(3);
            PlayWeaponGrabAnimation(GrabType.BackGrab);
        }
    }
}

public enum GrabType
{
    SideGrab,
    BackGrab
}