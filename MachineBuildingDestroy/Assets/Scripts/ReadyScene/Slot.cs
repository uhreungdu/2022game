using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slot : MonoBehaviour,IPunObservable
{
    private string _nickname = "";
    private bool _is_ready = false;

    public GameObject MasterButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Player 존재하는지 체크
            foreach (var target in PhotonNetwork.PlayerList)
            {
                // 있다면 return;
                if (target.NickName == _nickname) return;
            }
            // 없으면 변수 초기화
            _nickname = "";
            _is_ready = false;
        }
    }

    public string Nickname
    {
        get => _nickname;
        set => _nickname = value;
    }

    public bool IsReady
    {
        get => _is_ready;
        set => _is_ready = value;
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(_nickname);
            stream.SendNext(_is_ready);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            _nickname = (string) stream.ReceiveNext();
            _is_ready = (bool) stream.ReceiveNext();
        }
    }
    
}
