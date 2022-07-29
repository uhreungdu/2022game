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
    public GameObject platformImage;
    public Sprite[] platformIcons;
    
    private void FixedUpdate()
    {
        SetText();
        
        if (!PhotonNetwork.IsMasterClient) return;
        
        // Player 존재하는지 체크
        if (PhotonNetwork.PlayerList.Any(target => target.NickName == Nickname)) return;
        
        // 없으면 변수 초기화
        Nickname = "";
        IsReady = false;
        Platform = 2;
    }

    private void SetText()
    {
        nicknameText.GetComponent<Text>().text = Nickname;
        platformImage.GetComponent<Image>().sprite = platformIcons[Platform];
        if (PhotonNetwork.IsMasterClient)
        {
            statusText.GetComponent<Text>().text = "";
        }
        else
        {
            statusText.GetComponent<Text>().text = IsReady ? "ready!" : "";
        }
    }
    
    public string Nickname { get; set; } = "";

    public bool IsReady { get; set; } = false;

    public int Platform { get; set; } = 2;

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
            Platform = (int) stream.ReceiveNext();
        }
    }
    
}
