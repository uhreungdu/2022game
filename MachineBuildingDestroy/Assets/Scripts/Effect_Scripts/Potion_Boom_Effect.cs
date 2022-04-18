using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Potion_Boom_Effect : MonoBehaviourPun
{
    // Start is called before the first frame update
    public float delay = 3f;
    public float countdowm;
    public bool check_boom;
    public GameObject Boom_Effect;
    public GameObject Getobj;
    public GameObject Tail_obj;
    void Start()
    {
        countdowm = delay;
        check_boom = true;
        Getobj = Resources.Load<GameObject>("Throw_Tail");
        Tail_obj = Instantiate(Getobj);
        Tail_obj.transform.SetParent(gameObject.transform);
        Tail_obj.transform.Translate(gameObject.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        countdowm -= Time.deltaTime;
        if (countdowm <= 0f && !check_boom)
        {
            Boom();
            check_boom = true;
        }
    }

    public void Boom()
    {
        //터지는 이펙트 발동
        Boom_Effect = Resources.Load<GameObject>("BoomEffect");
        PhotonNetwork.Instantiate(Boom_Effect.name, transform.position, Quaternion.identity);
        //현재 보이는 방향으로 발사
        
        
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnCollisionStay(Collision coll)
    {
        check_boom = false;
        /*
        if (coll.transform.CompareTag("Floor"))
        {
            
        }
        */
    }
}
