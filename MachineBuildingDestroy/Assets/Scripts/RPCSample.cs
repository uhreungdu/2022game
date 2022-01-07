using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCSample : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            // PhotonView가 존재하는 오브젝트에 RPC를 보냄
            photonView.RPC("ChatMessage", RpcTarget.Others, "test", "TTEESSTT");
        }
    }

    // 원격 호출을 원하는 함수에 태그 적용
    [PunRPC]
    void ChatMessage(string a, string b, PhotonMessageInfo info)
    {
        Debug.Log(string.Format("Info: {0} | {1} | {2}", info.Sender, info.photonView, info.SentServerTimestamp));
        Debug.Log(string.Format("ChatMessage {0} {1}", a, b));

    }
}
