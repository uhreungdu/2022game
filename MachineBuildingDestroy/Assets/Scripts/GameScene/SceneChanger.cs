using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    private GameObject _myInRoomInfo;
    // Start is called before the first frame update
    void Start()
    {
        _myInRoomInfo = GameObject.Find("Myroominfo");
    }

    public void SceneChange(String Scenename)
    {
        Destroy(_myInRoomInfo);
        PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
