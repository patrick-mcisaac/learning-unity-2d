using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    // left off at physics control
    private void Update()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            Debug.Log("Up");
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            Debug.Log("Left");
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            Debug.Log("Right");
        }
    }
}
