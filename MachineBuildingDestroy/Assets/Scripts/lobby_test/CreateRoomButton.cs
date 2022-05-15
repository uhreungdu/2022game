using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Random = UnityEngine.Random;

public class CreateRoomButton : MonoBehaviour
{
    private LobbyManager gManager;
    private Account _account;
    private string _iname;  // internalRoomName
    private string _ename;  // externalRoomName
    private Socket _client;

    // Start is called before the first frame update
    void Start()
    {
        gManager = LobbyManager.GetInstance();
        _account = Account.GetInstance();
        _client = ChatClient.GetInstance().GetClientSocket();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom()
    {
        _ename = _account.GetPlayerNickname() + "ÀÇ ¹æ" + Random.Range(0, 9999);
        _iname = _ename + System.DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");

        SendMakeRoom();
        PhotonNetwork.JoinOrCreateRoom(_iname, new RoomOptions { MaxPlayers = 6 }, null);
    }
    
    private void SendMakeRoom()
    {
        byte[] iname = Encoding.UTF8.GetBytes(_iname);
        byte[] ename = Encoding.UTF8.GetBytes(_ename);

        byte[] sendBuf = new byte[iname.Length + ename.Length + 1 + 3];
        sendBuf[0] = (byte) ChatClient.ChatCode.MakeRoom;
        sendBuf[1] = (byte) iname.Length;
        sendBuf[2] = (byte) ename.Length;
        sendBuf[3] = (byte) 6;  // maxPlayerNum
        
        Array.Copy(iname, 0, sendBuf, 4, iname.Length);
        Array.Copy(ename, 0, sendBuf, 4 + iname.Length, ename.Length);
        
        _client.Send(sendBuf);
     
    }
}
