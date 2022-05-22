using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.VFX;

public class Hand_Effect : MonoBehaviourPun
{
    // Start is called before the first frame update
    public List<GameObject> effect_obj = new List<GameObject>();
    private List<ParticleSystem> _particleSystems = new List<ParticleSystem>();
    public GameObject Tail_obj;
    public VisualEffect Hit_VFX;
    public BoxCollider box_col;
    void Start()
    {
        /*
        effect_obj = Instantiate(Resources.Load<GameObject>("Hit_Effect"));
        effect_obj.transform.SetParent(gameObject.transform);
        Tail_obj = Instantiate(Resources.Load<GameObject>("Attack_Tail"));
        Tail_obj.transform.SetParent(gameObject.transform);
        
        pos_set = gameObject.transform.position;
        Tail_obj.transform.Translate(pos_set);
        */
        box_col = gameObject.GetComponent<BoxCollider>();
        foreach (var eGameObject in effect_obj)
        {
            ParticleSystem particleSystem = eGameObject.GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                _particleSystems.Add(particleSystem);
            }
        }
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
                Vector3 centerpo = Vector3.zero;
                foreach (var eGameObject in effect_obj)
                {
                    centerpo = (other.ClosestPoint(transform.position) + transform.position)/2f;
                    eGameObject.transform.position = centerpo;
                }
                centerpo = (other.ClosestPoint(transform.position) + transform.position)/2f;
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateRoomObject($"Effect/{effect_obj[1].name}", centerpo, effect_obj[1].transform.rotation);
                }
                Hit_VFX.Play();
            }
        }
    }

    [PunRPC]
    private void NetworkHitEffectInstantiate(Vector3 position)
    {
        photonView.RPC("HitEffectInstantiate", RpcTarget.AllViaServer, position);;
    }

    private void HitEffectInstantiate(Vector3 position)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(effect_obj[1].name, position, effect_obj[1].transform.rotation);
        }
    }
}
