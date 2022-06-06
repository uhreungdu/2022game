using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.VFX;

public class EnergyEffect : MonoBehaviour
{
    public VisualEffect charging;

    public VisualEffect beamEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        beamEffect.Stop();
        charging.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void play_charging()
    {
        charging.Play();
    }

    void play_BeamEffect()
    {
        beamEffect.Play();
    }
}
