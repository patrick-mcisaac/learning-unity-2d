using System;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{

    private Rigidbody2D landerRb;
    [Header("Lander Speed")]
    [SerializeField] private float landerSpeed = 550f;
    [SerializeField] private float turnSpeed = 50f;

    private float maxFuel = 10f;
    private float fuelAmount;

    // Event Handlers
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnGameStart;

    // Singleton
    public static Lander Instance;

    private void Awake()
    {
        landerRb = GetComponent<Rigidbody2D>();

        Instance = this;

        fuelAmount = maxFuel;
    }

    private void Start()
    {
        GameManager.Instance.state = GameManager.GameState.WaitingToStart;
    }

    // Move Lander
    private void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WaitingToStart
        && Keyboard.current.wKey.isPressed
        || Keyboard.current.dKey.isPressed
        || Keyboard.current.aKey.isPressed)
        {
            GameManager.Instance.state = GameManager.GameState.Normal;
            OnGameStart?.Invoke(this, EventArgs.Empty);
        }
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (fuelAmount <= 0)
        {
            return;
        }

        if (Keyboard.current.wKey.isPressed || Keyboard.current.dKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            ConsumeFuel();
        }
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

    private void ConsumeFuel()
    {
        fuelAmount -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Handle landing and crashes
        if (!collision.gameObject.TryGetComponent<LaunchPad>(out LaunchPad launchPad))
        {
            Debug.Log("Crash!");
            return;
        }

        // Get angle of impact
        float dot = Vector2.Dot(transform.up, Vector2.up);
        float minAngle = 0.95f;

        if (dot < minAngle)
        {
            Debug.Log("Too steep angle, CRASH!");
            return;
        }

        // Get speed
        float speed = collision.relativeVelocity.magnitude;
        float maxSpeed = 4f;
        if (speed > maxSpeed)
        {
            Debug.Log("TOO FAST!! CRASH!");
            return;
        }

        Debug.Log("Successful Landing!");
        // Add points for successful landing
        float baseScore = 100f;
        float landingSpeedScore = Mathf.Clamp01(1f - (speed / maxSpeed));
        float dotScore = Mathf.Max(0f, dot);

        int finalScore = (int)MathF.Round(((landingSpeedScore + dotScore) / 2) * baseScore);

        GameManager.Instance.AddScore(finalScore * launchPad.GetBonusPoints());

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelPickup>(out FuelPickup fuelPickup))
        {
            // add fuel
            fuelAmount = maxFuel;

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

    public float GetFuelAmountNormalized()
    {
        // (value - min) / (max - min) since min is 0 i can leave that out
        return fuelAmount / maxFuel;
    }
}
