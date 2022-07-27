using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviourPun
{
    public int Durability = 200;

    public PlayerState _PlayerState;
    
    private AttackButton _attackButton;
    // Update is called once per frame

    void Start()
    {
        _PlayerState = transform.root.GetComponent<PlayerState>();
        
        if (transform.root.GetComponent<PhotonView>().IsMine && Application.platform == RuntimePlatform.Android)
            _attackButton = GameObject.Find("AttackButton").GetComponent<AttackButton>();
    }

    private void Awake()
    {
        Durability = 200;
    }

    void Update()
    {
        if (Durability <= 0)
        {
            photonView.RPC("NoDurabilityCheck", RpcTarget.AllViaServer);
            if (transform.root.GetComponent<PhotonView>().IsMine && Application.platform == RuntimePlatform.Android)
            {
                // 공격버튼 원래대로 바꾸기
                _attackButton.ChangeButtonImage(item_box_make.item_type.no_item);
            }
        }
    }
    
    [PunRPC]
    void NoDurabilityCheck()
    {
        if (Durability <= 0)
        {
            _PlayerState.nowEquip = false;
            _PlayerState.Item = item_box_make.item_type.no_item;
            gameObject.SetActive(false);
        }
    }
    
}