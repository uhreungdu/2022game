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
            // PhotonView�� �����ϴ� ������Ʈ�� RPC�� ����
            photonView.RPC("ChatMessage", RpcTarget.Others, "test", "TTEESSTT");
        }
    }

    // ���� ȣ���� ���ϴ� �Լ��� �±� ����
    [PunRPC]
    void ChatMessage(string a, string b, PhotonMessageInfo info)
    {
        Debug.Log(string.Format("Info: {0} | {1} | {2}", info.Sender, info.photonView, info.SentServerTimestamp));
        Debug.Log(string.Format("ChatMessage {0} {1}", a, b));

    }
}
