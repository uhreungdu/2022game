using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMarkEffectControl : MonoBehaviour
{
    public List<GameObject> effects = new List<GameObject>();

    public List<ParticleSystem> particles = new List<ParticleSystem>();

    public List<GameObject> effect_obj = new List<GameObject>();

    public GameManager _gameManager;

    public GameObject Landmarkobj;
    public GameObject feild_Tex;

    public bool can_effect;
    // Start is called before the first frame update
    void Start()
    {
        setting_effects();
        _gameManager = GameManager.GetInstance();
        can_effect = true;
        feild_Tex.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.EManager.landmakr_Create)
        {
            LandMarkObject dBulidingObject = Landmarkobj.GetComponent<LandMarkObject>();
            if (Landmarkobj.transform.position.y < 0 && can_effect)
            {
                Play_Effects();
                can_effect = false;
            }

            if (dBulidingObject.dead)
            {
                Play_Effects();
            }
        }
    }

    public void setting_effects()
    {
        for (int i = 0; i < effects.Count; ++i)
        {
            if (effects[i] != null)
            {
                particles.Add(effects[i].GetComponent<ParticleSystem>());
                effects[i].SetActive(false);
            }
        }
        for (int i = 0; i < effect_obj.Count; ++i)
        {
            if (effect_obj[i] != null)
            {
                effect_obj[i].SetActive(false);
            }
        }
    }

    public void Play_Effects()
    {
        for (int i = 0; i < effects.Count; ++i)
        {
            if (effects[i] != null)
            {
                effects[i].SetActive(can_effect);
            }
        }
        for (int i = 0; i < effect_obj.Count; ++i)
        {
            if (effect_obj[i] != null)
            {
                effect_obj[i].SetActive(can_effect);
            }
        }

        if (can_effect == true)
        {
            for (int i = 0; i < particles.Count; ++i)
            {
                if (particles[i] != null)
                {
                    particles[i].Play();
                }
            }
        }
        feild_Tex.SetActive(false);
    }
}
