using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private PlayerControl playerControl;

    public static GameInput Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        playerControl = new PlayerControl();
        playerControl.Enable();
    }

    private void OnDestroy()
    {
        playerControl.Disable();
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
}
