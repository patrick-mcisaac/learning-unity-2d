using System;
using UnityEngine;


public class Lander : MonoBehaviour
{

    private Rigidbody2D landerRb;
    [Header("Lander Speed")]
    [SerializeField] private float landerSpeed = 550f;
    [SerializeField] private float turnSpeed = 50f;

    private float maxFuel = 10f;
    private float fuelAmount;
    private float deadZone = 0.2f;

    // Event Handlers
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;

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
                if (PlayerControls.Instance.IsUpActionPressed() ||
                    PlayerControls.Instance.IsLeftActionPressed() ||
                    PlayerControls.Instance.IsRightActionPressed() ||
                    PlayerControls.Instance.IsMoving().x > deadZone ||
                    PlayerControls.Instance.IsMoving().y > deadZone)
                {
                    GameManager.Instance.state = GameManager.GameState.Normal;
                }
                break;
            case GameManager.GameState.Normal:
                if (fuelAmount <= 0)
                {
                    return;
                }

                if (PlayerControls.Instance.IsUpActionPressed() ||
                    PlayerControls.Instance.IsLeftActionPressed() ||
                    PlayerControls.Instance.IsRightActionPressed() ||
                    PlayerControls.Instance.IsMoving().x > deadZone ||
                    PlayerControls.Instance.IsMoving().y > deadZone)
                {
                    ConsumeFuel();
                }
                if (PlayerControls.Instance.IsUpActionPressed() || PlayerControls.Instance.IsMoving().y > deadZone)
                {
                    // Move up
                    landerRb.AddForce(transform.up * landerSpeed * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }

                if (PlayerControls.Instance.IsLeftActionPressed() || PlayerControls.Instance.IsMoving().x < -deadZone)
                {
                    // move left
                    landerRb.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);

                }
                if (PlayerControls.Instance.IsRightActionPressed() || PlayerControls.Instance.IsMoving().x > deadZone)
                {
                    // move right
                    landerRb.AddTorque(-turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }

                break;
            case GameManager.GameState.Paused:

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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FuelPickup>(out FuelPickup fuelPickup))
        {
            // add fuel
            fuelAmount = maxFuel;
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
            // Destroy the fuel pickup
            fuelPickup.DestroySelf();
        }
        if (collision.gameObject.TryGetComponent<CoinPickup>(out CoinPickup coinPickup))
        {
            // add points
            GameManager.Instance.AddScore(coinPickup.GetPoints());
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            // Destroy the coin pickup
            coinPickup.DestroySelf();
        }
    }

    public float GetFuelAmountNormalized()
    {
        // (value - min) / (max - min) since min is 0 i can leave that out
        return fuelAmount / maxFuel;
    }

    public float GetLinearVelocityX()
    {
        return landerRb.linearVelocity.x;
    }

    public float GetLinearVelocityY()
    {
        return landerRb.linearVelocity.y;
    }
}
