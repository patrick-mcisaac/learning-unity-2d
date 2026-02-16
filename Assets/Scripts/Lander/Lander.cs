using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    private Rigidbody2D landerRigidbody2D;

    public static Lander Instance { get; private set; }

    [Header("Rocket Speed")]
    [SerializeField] private float rocketSpeed = 700f;
    [SerializeField] private float turnSpeed = 100f;

    private float softLandingVelocityMagnitude = 3f;

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnStateChangeArgs> OnStateChange;
    public class OnStateChangeArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType LandingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float ScoreMultiplier;

    }

    public enum LandingType
    {
        Success,
        WrongLandingArea,
        TooSteepAngle,
        TooFastLanding
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver
    }

    private float fuelAmount;
    private float fuelAmountMax = 10f;
    public State state;


    private void Awake()
    {
        fuelAmount = fuelAmountMax;
        state = State.WaitingToStart;
        landerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
        Instance = this;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            case State.WaitingToStart:
                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsRightActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed())
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;
                    SetState(State.Normal);


                }
                break;
            case State.Normal:
                if (fuelAmount < 0f)
                {
                    return;
                }

                if (GameInput.Instance.IsUpActionPressed() ||
                    GameInput.Instance.IsRightActionPressed() ||
                    GameInput.Instance.IsLeftActionPressed())
                {
                    ConsumeFuel();
                }

                if (GameInput.Instance.IsUpActionPressed())
                {
                    landerRigidbody2D.AddForce(rocketSpeed * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.IsLeftActionPressed())
                {
                    landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.IsRightActionPressed())
                {
                    landerRigidbody2D.AddTorque(-turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;

        }


    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                LandingType = LandingType.WrongLandingArea,
                dotVector = 0,
                landingSpeed = 0,
                ScoreMultiplier = 0,
                score = 0
            });
            SetState(State.GameOver);
            return;
        }

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;

        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                LandingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                ScoreMultiplier = 0,
                score = 0
            });
            SetState(State.GameOver);
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .97f;
        if (dotVector < minDotVector)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                LandingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                ScoreMultiplier = 0,
                score = 0
            });
            SetState(State.GameOver);
            return;
        }


        float maxScoreLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;



        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.ScoreMultiplier());

        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            LandingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            ScoreMultiplier = landingPad.ScoreMultiplier(),
            score = score
        });
        SetState(State.GameOver);
    }

    private void ConsumeFuel()
    {
        float fuelConsumption = 1f;
        fuelAmount -= fuelConsumption * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.TryGetComponent<FuelPickup>(out FuelPickup fuelPickup))
        {
            float addFuelAmount = 10f;
            fuelAmount += addFuelAmount;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            fuelPickup.DestroySelf();
        }

        if (collision2D.gameObject.TryGetComponent<CoinPickup>(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChange?.Invoke(this, new OnStateChangeArgs
        {
            state = state
        });
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocity.x;
    }

    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocity.y;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
}
