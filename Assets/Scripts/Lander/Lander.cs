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
    public event EventHandler<OnLandedEventArgs> OnLanded;

    public enum LandingType
    {
        Success,
        Crash
    }

    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public int scoreMultiplier;
    }


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
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        switch (GameManager.Instance.state)
        {
            case GameManager.GameState.WaitingToStart:
                if (Keyboard.current.wKey.isPressed ||
                    Keyboard.current.dKey.isPressed ||
                    Keyboard.current.aKey.isPressed)
                {
                    GameManager.Instance.state = GameManager.GameState.Normal;
                }
                break;
            case GameManager.GameState.Normal:
                if (fuelAmount <= 0)
                {
                    return;
                }

                if (Keyboard.current.wKey.isPressed ||
                    Keyboard.current.dKey.isPressed ||
                    Keyboard.current.aKey.isPressed)
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
                break;
            case GameManager.GameState.Loading:

                break;
            case GameManager.GameState.GameOver:
                break;
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
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.Crash,
                dotVector = 0,
                landingSpeed = 0,
                score = 0,
                scoreMultiplier = 0

            });
            return;
        }

        // Get angle of impact
        float dot = Vector2.Dot(transform.up, Vector2.up);
        float minAngle = 0.95f;

        // Get speed
        float speed = collision.relativeVelocity.magnitude;
        float maxSpeed = 4f;

        if (dot < minAngle || speed > maxSpeed)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.Crash,
                dotVector = 0,
                landingSpeed = 0,
                score = 0,
                scoreMultiplier = 0

            });
            return;
        }

        float baseScore = 100f;
        float landingSpeedScore = Mathf.Clamp01(1f - (speed / maxSpeed));
        float dotScore = Mathf.Max(0f, dot);

        int finalScore = (int)MathF.Round(((landingSpeedScore + dotScore) / 2) * baseScore);
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dot,
            landingSpeed = speed,
            score = finalScore,
            scoreMultiplier = launchPad.GetBonusPoints()

        });

        // GameManager.Instance.state = GameManager.GameState.Loading;


        // GameManager.Instance.AddScore(finalScore * launchPad.GetBonusPoints());

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
