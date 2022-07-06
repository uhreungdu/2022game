using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HammerEffect : MonoBehaviour
{
    public GameObject effect_obj;
    public GameObject Hit_effect_obj;
    public GameObject Tail_obj;
    public BoxCollider box_col;
    public Effect_control wave_effect;
    public VisualEffect Hit_VFX;
    // Start is called before the first frame update
    void Start()
    {
        wave_effect = effect_obj.GetComponent<Effect_control>();
        box_col = gameObject.GetComponent<BoxCollider>();
        Hit_VFX = Hit_effect_obj.GetComponent<VisualEffect>();
        effect_obj.SetActive(false);
        Tail_obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (box_col.enabled == true)
        {
            Tail_obj.SetActive(true);
            
        }
        else
        {
            Tail_obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"||other.tag == "Wall" || other.tag == "Obstcle_Item")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                effect_obj.SetActive(true);
                wave_effect.Play_Particles();
                Hit_effect_obj.SetActive(true);
                Vector3 centerpo = (other.ClosestPoint(transform.position) + transform.position)/2f;
                Hit_effect_obj.transform.position = centerpo;
                Hit_VFX.Play();
            }
        }
    }
}
