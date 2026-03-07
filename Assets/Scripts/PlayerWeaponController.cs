using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player _player;
    
    private static readonly int Fire = Animator.StringToHash("Fire");

    private void Start()
    {
        _player = GetComponent<Player>();
        _player.Controls.Character.Fire.performed += _ => Shoot();
    }

    private void Shoot()
    {
        GetComponentInChildren<Animator>().SetTrigger(Fire);
    }
}