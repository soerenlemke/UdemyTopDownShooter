using System;
using UnityEngine;

public class WeaponVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransforms;
    
    [SerializeField]  private Transform pistol;
    [SerializeField]  private Transform revolver;
    [SerializeField]  private Transform autoRifle;
    [SerializeField]  private Transform shotgun;
    [SerializeField]  private Transform rifle;

    private Transform _currentGun;
    
    [Header("Left hand IK")]
    [SerializeField] private Transform leftHand;

    private void Start()
    {
        SwitchOn(pistol);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchOn(pistol);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchOn(revolver);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchOn(autoRifle);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SwitchOn(shotgun);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            SwitchOn(rifle);
    }

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
}
