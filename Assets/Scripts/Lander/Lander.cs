using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    private Rigidbody2D landerRb;
    [Header("Lander Speed")]
    [SerializeField] private float landerSpeed = 550f;
    [SerializeField] private float turnSpeed = 50f;

    // Event Handlers
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;

    // Singleton
    public static Lander Instance;

    private void Awake()
    {
        landerRb = GetComponent<Rigidbody2D>();

        Instance = this;
    }
    // Move Lander
    private void Update()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (Keyboard.current.wKey.isPressed)
        {
            // Move up
            landerRb.AddForce(transform.up * landerSpeed * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }

        if (Keyboard.current.aKey.isPressed)
        {
            // move left
            landerRb.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);

        }
        if (Keyboard.current.dKey.isPressed)
        {
            // move right
            landerRb.AddTorque(-turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);


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
            // add points
            GameManager.Instance.AddScore(coinPickup.GetPoints());
            // Destroy the coin pickup
            coinPickup.DestroySelf();
        }
    }
}
