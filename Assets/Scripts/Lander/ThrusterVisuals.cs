using System;
using UnityEngine;

public class ThrusterVisuals : MonoBehaviour
{
    [Header("Thrusters")]
    [SerializeField] private ParticleSystem middleThruster;
    [SerializeField] private ParticleSystem rightThruster;
    [SerializeField] private ParticleSystem leftThruster;

    private void Awake()
    {
        SetEnableParticleSystem(middleThruster, false);
        SetEnableParticleSystem(rightThruster, false);
        SetEnableParticleSystem(leftThruster, false);
    }

    private void Start()
    {
        Lander.Instance.OnUpForce += Lander_OnUpForce;
        Lander.Instance.OnRightForce += Lander_OnRightForce;
        Lander.Instance.OnLeftForce += Lander_OnLeftForce;
        Lander.Instance.OnBeforeForce += Lander_OnBeforeForce;
    }

    private void SetEnableParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    private void Lander_OnUpForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(middleThruster, true);
        SetEnableParticleSystem(rightThruster, true);
        SetEnableParticleSystem(leftThruster, true);
    }

    private void Lander_OnRightForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(leftThruster, true);
    }

    private void Lander_OnLeftForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(rightThruster, true);
    }

    private void Lander_OnBeforeForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(middleThruster, false);
        SetEnableParticleSystem(rightThruster, false);
        SetEnableParticleSystem(leftThruster, false);
    }
}
