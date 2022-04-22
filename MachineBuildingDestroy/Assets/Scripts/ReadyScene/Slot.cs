using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Slot : MonoBehaviour,IPunObservable
{
    public GameObject nicknameText;
    public GameObject statusText;
    public GameObject masterButton;
    public GameObject platformText;
    
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
        SetText();
        
        if (!PhotonNetwork.IsMasterClient) return;
        
        // Player 존재하는지 체크
        if (PhotonNetwork.PlayerList.Any(target => target.NickName == Nickname)) return;
        
        // 없으면 변수 초기화
        Nickname = "";
        IsReady = false;
        Platform = "";
    }

    private void SetText()
    {
        nicknameText.GetComponent<Text>().text = Nickname;
        statusText.GetComponent<Text>().text = IsReady ? "ready!" : "";
        platformText.GetComponent<Text>().text = Platform;
    }
    
    public string Nickname { get; set; } = "";

    public bool IsReady { get; set; } = false;

    public string Platform { get; set; } = "";

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(Nickname);
            stream.SendNext(IsReady);
            stream.SendNext(Platform);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            Nickname = (string) stream.ReceiveNext();
            IsReady = (bool) stream.ReceiveNext();
            Platform = (string) stream.ReceiveNext();
        }
    }
    
}
