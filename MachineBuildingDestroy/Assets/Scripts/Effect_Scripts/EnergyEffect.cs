using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyEffect : MonoBehaviour    
{
    public VisualEffect charging;

    public VisualEffect beamEffect;
    public ParticleSystem windwaveEffect;
    public ParticleSystem windlazerEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        beamEffect.Stop();
        charging.Stop();
        windwaveEffect.Stop();
        windlazerEffect.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void play_charging()
    {
        charging.Play();
    }

    public void play_BeamEffect()
    {
        beamEffect.Play();
        windwaveEffect.Play();
        windlazerEffect.Play();
    }
}
