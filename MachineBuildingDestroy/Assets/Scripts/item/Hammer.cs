using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Hammer : MonoBehaviourPun
{
    public int Durability = 2;
    public PlayerState _PlayerState;
    public AudioClip HitClip;

    private AttackButton _attackButton;

    // Update is called once per frame
    void Start()
    {
        _PlayerState = transform.root.GetComponent<PlayerState>();
        HitClip = Resources.Load<AudioClip>("Sounds/Hammer");
        if (transform.root.GetComponent<PhotonView>().IsMine && Application.platform == RuntimePlatform.Android)
            _attackButton = GameObject.Find("AttackButton").GetComponent<AttackButton>();
    }

    private void OnEnable()
    {
        Durability = 2;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Durability <= 0)
            {
                photonView.RPC("NetworkHummerNoDurabilityCheck", RpcTarget.AllViaServer);
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
    public void NetworkHummerNoDurabilityCheck()
    {
        NoDurabilityCheck();
    }
}