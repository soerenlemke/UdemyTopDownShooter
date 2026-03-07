using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals _visuals;

    private void Start()
    {
        _visuals = GetComponentInParent<PlayerWeaponVisuals>();
    }

    public void ReloadIsOver()
    {
        _visuals.MaximizeRigWeight();
    }

    public void ReturnRig()
    {
        _visuals.MaximizeRigWeight();
        _visuals.MaximizeLeftHandWeight();
    }
    
    public void WeaponGrabIsOver()
    {
        _visuals.SetBusyGrabbingWeaponTo(false);
    }
}