using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponVisualController : MonoBehaviour
{
    private static readonly int Reload = Animator.StringToHash("Reload");
    private static readonly int WeaponGrabType = Animator.StringToHash("WeaponGrabType");
    private static readonly int WeaponGrab = Animator.StringToHash("WeaponGrab");

    private Animator _animator;
    
    [SerializeField] private Transform[] gunTransforms;
    [SerializeField]  private Transform pistol;
    [SerializeField]  private Transform revolver;
    [SerializeField]  private Transform autoRifle;
    [SerializeField]  private Transform shotgun;
    [SerializeField]  private Transform rifle;

    private Transform _currentGun;
    
    [Header("Left hand IK")]
    [SerializeField] private Transform leftHand;

    [Header("Rig")]
    [SerializeField] private float rigIncreaseStep;
    private Rig _rig;
    private bool _rigShouldBeIncreased;

    private void Start()
    {
        SwitchOn(pistol);
        
        _animator = GetComponentInChildren<Animator>();
        _rig = GetComponentInChildren<Rig>();
    }

    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R))
        {
            _animator.SetTrigger(Reload);
            PauseRig();
        }

        if (_rigShouldBeIncreased)
        {
            _rig.weight += rigIncreaseStep * Time.deltaTime;

            if (_rig.weight >= 1)
            {
                _rigShouldBeIncreased = false;
            }
        }
    }

    private void PauseRig()
    {
        _rig.weight = 0.15f;
    }

    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        PauseRig();
        _animator.SetFloat(WeaponGrabType, (float)grabType);
        _animator.SetTrigger(WeaponGrab);
    }
    
    public void ReturnRigWeihtToOne() => _rigShouldBeIncreased = true;

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
        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
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