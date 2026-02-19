using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerControl playerControl;

    public static GameInput Instance { get; private set; }

    public event EventHandler OnMenuButtonPressed;
    private void Awake()
    {
        Instance = this;
        playerControl = new PlayerControl();
        playerControl.Enable();

        playerControl.Player.MenuAction.performed += Menu_performed;
    }

    private void OnDestroy()
    {
        playerControl.Disable();
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMenuButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsUpActionPressed()
    {
        return playerControl.Player.LanderUp.IsPressed();
    }

    public bool IsRightActionPressed()
    {
        return playerControl.Player.LanderRight.IsPressed();
    }

    public bool IsLeftActionPressed()
    {
        return playerControl.Player.LanderLeft.IsPressed();
    }

    public Vector2 GetMovementInputVector2()
    {
        return playerControl.Player.Movement.ReadValue<Vector2>();
    }


}
