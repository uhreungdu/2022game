using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject foot_obj;
    public EnergyEffect _EnergyEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFooteffect()
    {
        GameObject create_obj = Instantiate(foot_obj,transform);
        create_obj.transform.position = gameObject.transform.position + new Vector3(0,0.5f,0);
        create_obj.transform.parent = null;
        Destroy(create_obj,1.5f);
    }
    
    void play_charging()
    {
        _EnergyEffect.play_charging();
    }

    void play_BeamEffect()
    {
        _EnergyEffect.play_BeamEffect();
    }
}
