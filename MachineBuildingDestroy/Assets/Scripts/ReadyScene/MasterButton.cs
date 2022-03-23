using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

public class MasterButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        var targetName = transform.parent.GetComponent<Slot>().Nickname;
        Player[] p = PhotonNetwork.PlayerList;
        for (int i = 0; i < p.Length; ++i)
        {
            if (p[i].NickName == targetName)
            {
                PhotonNetwork.SetMasterClient(p[i]);
                break;
            }
        }
    }
}
