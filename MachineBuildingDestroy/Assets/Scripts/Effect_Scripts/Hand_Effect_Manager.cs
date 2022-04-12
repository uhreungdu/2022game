using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.VFX;

public class Hand_Effect_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider Hand_Box;
    public ParticleSystem use_Ef;
    public GameObject get_Obj;
    public GameObject effect_obj;
    public VisualEffect Hit_VFX;
    void Start()
    {
        effect_obj = Instantiate(Resources.Load<GameObject>("Hit_Effect"));
        effect_obj.transform.SetParent(gameObject.transform);
        
        Hit_VFX = effect_obj.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"||other.tag == "Wall" || other.tag == "Obstcle_Item")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                effect_obj.transform.position = other.ClosestPointOnBounds(transform.position);
                Hit_VFX.Play();
            }
        }
    }
}
