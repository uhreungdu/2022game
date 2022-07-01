using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class RoomBlock : MonoBehaviour
{
    public GameObject roomName;
    public GameObject playerNum;
    
    private string _iname;
    private string _ename;
    private int _nowP;
    private int _maxP;
    private bool _ingame;
    private Socket _client;

    private LobbyManager _lobbyManager;
    private Account _account;

    // Start is called before the first frame update
    void Start()
    {
        _lobbyManager = LobbyManager.GetInstance();
        _account = Account.GetInstance();
        _client = LoginDBConnection.GetInstance().GetClientSocket();
    }

    public void SetVariables(string internalName, string externalName, int nowPlayerNum, int maxPlayerNum, bool ingame)
    {
        _iname = internalName;
        _ename = externalName;
        _nowP = nowPlayerNum;   
        _maxP = maxPlayerNum;
        _ingame = ingame;
        SetText();
    }

    void SetText()
    {
        if (_iname != "")
        {
            roomName.GetComponent<Text>().text = _ename;
            playerNum.GetComponent<Text>().text = _nowP + " / " + _maxP;
            GetComponent<Button>().interactable = _ingame == false;
            if (_ingame != true) GetComponent<Button>().interactable = _nowP != _maxP;
        }
        else
        {
            roomName.GetComponent<Text>().text = "";
            playerNum.GetComponent<Text>().text = "";
            GetComponent<Button>().interactable = false;
        }
    }

    public void EnterRoom()
    {
        SendEnterRoom();
        PhotonNetwork.JoinRoom(_iname);
    }
    
    private void SendEnterRoom()
    {
        byte[] tempBuf = Encoding.UTF8.GetBytes(_iname);
        
        byte[] sendBuf = new byte[tempBuf.Length + 1];
        sendBuf[0] = (byte) LoginDBConnection.ChatCode.EnterRoom;
        
        Array.Copy(tempBuf, 0, sendBuf, 1, tempBuf.Length);
        
        _client.Send(sendBuf);
    }

    
}
