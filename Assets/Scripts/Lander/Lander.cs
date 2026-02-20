using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    private Rigidbody2D landerRb;
    private float landerSpeed = 2f;
    private float torqueSpeed = .5f;

    private void Awake()
    {
        landerRb = GetComponent<Rigidbody2D>();
    }
    // Move Lander
    private void FixedUpdate()
    {
        if (Keyboard.current.wKey.isPressed)
        {
            // Move up
            landerRb.linearVelocityY = landerSpeed;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            // move left
            landerRb.AddTorque(torqueSpeed);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            // move right
            landerRb.AddTorque(-torqueSpeed);

        }
    }
}
