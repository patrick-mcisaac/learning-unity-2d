using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    [Header("Rocket Speed")]
    [SerializeField] private float rocketSpeed = 700f;
    [SerializeField] private float turnSpeed = 100f;

    private float softLandingVelocityMagnitude = 3f;




    private void Awake()
    {
        landerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            landerRigidbody2D.AddForce(rocketSpeed * transform.up * Time.deltaTime);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-turnSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("Landed too hard");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .97f;
        if (dotVector < minDotVector)
        {
            Debug.Log("Landed on a too steep an angle");
            return;
        }

        Debug.Log("Successful Landing!");
    }

}
