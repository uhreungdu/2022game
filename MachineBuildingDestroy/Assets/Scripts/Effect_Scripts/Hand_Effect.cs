using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.VFX;

public class Hand_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider Hand_Box;
    public ParticleSystem use_Ef;
    public GameObject get_Obj;
    public GameObject effect_obj;
    public GameObject Tail_obj;
    public VisualEffect Hit_VFX;
    public Vector3 pos_set;
    public BoxCollider box_col;
    public GameObject _lHandGameObject;
    public GameObject _RHandGameObject;
    void Start()
    {
        effect_obj = Instantiate(Resources.Load<GameObject>("Hit_Effect"));
        effect_obj.transform.SetParent(gameObject.transform);
        Tail_obj = Instantiate(Resources.Load<GameObject>("Attack_Tail"));
        Tail_obj.transform.SetParent(gameObject.transform);
        
        pos_set = gameObject.transform.position;
        Tail_obj.transform.Translate(pos_set);
        Hit_VFX = effect_obj.GetComponent<VisualEffect>();
        box_col = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (box_col.enabled == true)
        {
            pos_set = gameObject.transform.position;
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
                Vector3 centerpo = (other.ClosestPoint(transform.position) + transform.position)/2f;
                effect_obj.transform.position = centerpo;
                Hit_VFX.Play();
            }
        }
    }
}
