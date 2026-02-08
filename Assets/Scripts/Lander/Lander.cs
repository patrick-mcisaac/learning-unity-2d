using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidbody2D;

    [SerializeField] private float rocketSpeed = 700f;
    [SerializeField] private float turnSpeed = 100f;

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

}
