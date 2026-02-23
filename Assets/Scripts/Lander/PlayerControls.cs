using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private LanderControls landerControls;

    public static PlayerControls Instance { get; private set; }

    public event EventHandler OnPausePressed;

    private void Awake()
    {
        Instance = this;
        landerControls = new LanderControls();
        landerControls.Enable();

        landerControls.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        landerControls.Disable();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        OnPausePressed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsUpActionPressed()
    {
        return landerControls.Player.Up.IsPressed();
    }

    public bool IsLeftActionPressed()
    {
        return landerControls.Player.Left.IsPressed();
    }
    public bool IsRightActionPressed()
    {
        return landerControls.Player.Right.IsPressed();
    }

    public Vector2 IsMoving()
    {
        return landerControls.Player.Movement.ReadValue<Vector2>();
    }
}