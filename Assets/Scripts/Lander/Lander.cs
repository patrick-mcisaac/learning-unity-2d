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

    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;



    private void Awake()
    {
        landerRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);
        if (Keyboard.current.upArrowKey.isPressed)
        {
            landerRigidbody2D.AddForce(rocketSpeed * transform.up * Time.deltaTime);
            OnUpForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            landerRigidbody2D.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            landerRigidbody2D.AddTorque(-turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {

        if (!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            Debug.Log("crashed on terrain");
            return;
        }

        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;

        if (relativeVelocityMagnitude > softLandingVelocityMagnitude)
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
        float maxScoreLandingAngle = 100f;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreLandingAngle;

        float maxScoreAmountLandingSpeed = 100f;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;

        Debug.Log($"landing angle score: {landingAngleScore}");
        Debug.Log($"landingSpeedScore: {landingSpeedScore}");

        int score = Mathf.RoundToInt((landingAngleScore + landingSpeedScore) * landingPad.ScoreMultiplier());

        Debug.Log($"Score: {score}");
    }

}
