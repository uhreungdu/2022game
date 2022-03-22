using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Slot : MonoBehaviour,IPunObservable
{
    private string _nickname = "";

    public GameObject MasterButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string Nickname
    {
        get => _nickname;
        set => _nickname = value;
    }


    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(_nickname);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            _nickname = (string) stream.ReceiveNext();
        }
    }
    
}
