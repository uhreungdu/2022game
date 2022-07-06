using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlots : MonoBehaviourPun
{
    private bool init = false;
    private int maxPlayer;
    public GameObject[] slots = new GameObject[6];
    public GameObject info;
    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.Find("Myroominfo");
        maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        for (var i = 5; i > maxPlayer - 1; --i)
        {
            slots[i].GetComponent<Image>().color = Color.gray;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!init)
        {
            var nickname = PhotonNetwork.NickName;
            photonView.RPC("SetSlotByQuick",RpcTarget.MasterClient, nickname, Application.platform);
            init = true;
        }
        
        var num = -1;
        
        // 자신 슬롯 번호 갱신
        for (var i = 0; i < maxPlayer; ++i)
        {
            var slotComponent = slots[i].GetComponent<Slot>();
            if (slotComponent.Nickname != PhotonNetwork.NickName) continue;
            info.GetComponent<MyInRoomInfo>().mySlotNum = i;
            num = i;
            break;
        }
        
        // 자신 슬롯 색상 강조
        for (var i = 0; i < maxPlayer; ++i)
        {
            var slotComponent = slots[i].GetComponent<Slot>();
            slots[i].GetComponent<Image>().color = num!=i ? Color.white : Color.yellow;
        }
        
        // 룸 정보 갱신
        for (var i = 0; i < maxPlayer; ++i)
        {
            var slotComponent = slots[i].GetComponent<Slot>();
            info.GetComponent<MyInRoomInfo>().RenewInfo(i, slotComponent);
            slots[i].GetComponent<Image>().color = num!=i ? Color.white : Color.yellow;
        }
        
        // Master 넘기는 버튼 활성화 갱신
        if (PhotonNetwork.IsMasterClient)
        {
            for (var i = 0; i < maxPlayer; ++i)
            {
                var slotComponent = slots[i].GetComponent<Slot>();
                if (slotComponent.Nickname != "")
                    slotComponent.masterButton.GetComponent<Button>().interactable = i != num;
                else slotComponent.masterButton.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (var i = 0; i < maxPlayer; ++i)
            {
                slots[i].GetComponent<Slot>().masterButton.GetComponent<Button>().interactable = false;
            }
        }
    }
    
    [PunRPC]
    void SetSlotByQuick(string nickname, RuntimePlatform platform)
    {
        var slot = transform.GetComponent<CharacterSlots>();
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;
        for (var i = 0; i < maxPlayer; ++i)
        {
            var target = slot.slots[i].GetComponent<Slot>();
            if (target.Nickname != "") continue;
            target.Nickname = nickname;
            target.IsReady = false;
            target.Platform = platform == RuntimePlatform.Android ? 1 : 0;
            Debug.Log(target.Platform);
            break;

        }
    }
    
}
