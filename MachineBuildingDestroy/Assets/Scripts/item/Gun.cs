using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviourPun
{
    public int Durability = 100;

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
        Durability = 100;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Durability <= 0)
            {
                photonView.RPC("NetworkNoDurabilityCheck", RpcTarget.AllViaServer);
                if (transform.root.GetComponent<PhotonView>().IsMine && Application.platform == RuntimePlatform.Android)
                {
                    // 공격버튼 원래대로 바꾸기
                    _attackButton.ChangeButtonImage(item_box_make.item_type.no_item);
                }
            }
        }
    }

    public void NoDurabilityCheck()
    {
        _PlayerState.nowEquip = false;
        _PlayerState.Item = item_box_make.item_type.no_item;
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void NetworkNoDurabilityCheck()
    {
        NoDurabilityCheck();
    }
}