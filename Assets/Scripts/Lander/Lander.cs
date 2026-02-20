using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    private Rigidbody2D landerRb;
    [Header("Lander Speed")]
    [SerializeField] private float landerSpeed = 550f;
    [SerializeField] private float turnSpeed = 50f;

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
            landerRb.AddForce(transform.up * landerSpeed * Time.deltaTime);
        }

        if (Keyboard.current.aKey.isPressed)
        {
            // move left
            landerRb.AddTorque(turnSpeed * Time.deltaTime);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            // move right
            landerRb.AddTorque(-turnSpeed * Time.deltaTime);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle landing and crashes
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelPickup>(out FuelPickup fuelPickup))
        {
            // add fuel

            // Destroy the fuel pickup

            fuelPickup.DestroySelf();
        }
        if (collision.gameObject.TryGetComponent<CoinPickup>(out CoinPickup coinPickup))
        {
            // add fuel

            // Destroy the coin pickup
            coinPickup.DestroySelf();
        }
    }
}
