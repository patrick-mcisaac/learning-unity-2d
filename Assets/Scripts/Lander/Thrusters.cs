using System;
using UnityEngine;

public class Thrusters : MonoBehaviour
{

    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem middleThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;

    private Lander lander;

    private void Awake()
    {
        lander = gameObject.GetComponent<Lander>();

        lander.OnUpForce += LanderOnUpForce;
        lander.OnLeftForce += LanderOnLeftForce;
        lander.OnRightForce += LanderOnRightForce;
        lander.OnBeforeForce += LanderOnBeforeForce;

        SetEnableParticleSystem(leftThrusterParticleSystem, false);
        SetEnableParticleSystem(middleThrusterParticleSystem, false);
        SetEnableParticleSystem(rightThrusterParticleSystem, false);

    }



    private void SetEnableParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    private void LanderOnBeforeForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(leftThrusterParticleSystem, false);
        SetEnableParticleSystem(middleThrusterParticleSystem, false);
        SetEnableParticleSystem(rightThrusterParticleSystem, false);
    }

    private void LanderOnUpForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(leftThrusterParticleSystem, true);
        SetEnableParticleSystem(middleThrusterParticleSystem, true);
        SetEnableParticleSystem(rightThrusterParticleSystem, true);
    }

    private void LanderOnLeftForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(rightThrusterParticleSystem, true);
    }

    private void LanderOnRightForce(object sender, EventArgs e)
    {
        SetEnableParticleSystem(leftThrusterParticleSystem, true);
    }


}
