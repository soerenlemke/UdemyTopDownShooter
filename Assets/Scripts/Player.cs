using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControls Controls;

    private void Awake()
    {
        Controls = new PlayerControls();
    }
    
    private void OnEnable()
    {
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
