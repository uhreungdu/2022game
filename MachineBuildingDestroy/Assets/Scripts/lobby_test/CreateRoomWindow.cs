using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CreateRoomWindow : MonoBehaviour
{
    private LobbyManager gManager;
    private Account _account;
    private string _iname;  // internalRoomName
    private string _ename;  // externalRoomName
    private byte _playerNum;
    private string _gameMode;
    private Socket _client;

    private int[] _playerNumArray = new int[3];
    private string[] _gameModeArray = new string[3];

    public GameObject roomNameField;
    public GameObject playerNumField;
    public GameObject gameModeField;
    public GameObject okButton;

    public GameObject DarkBackground;

    private void Awake()
    {
        _playerNumArray[0] = 6;
        _playerNumArray[1] = 4;
        _playerNumArray[2] = 2;
        _gameModeArray[0] = "Option A";
        _gameModeArray[1] = "Option B";
        _gameModeArray[2] = "Option C";
    }

    void Start()
    {
        gManager = LobbyManager.GetInstance();
        _account = Account.GetInstance();
        _client = LoginDBConnection.GetInstance().GetClientSocket();
    }

    private void FixedUpdate()
    {
        okButton.GetComponent<Button>().interactable = roomNameField.GetComponent<InputField>().text != "";
    }

    public void OnClick(bool val)
    {
        // OK
        if (val)
        {
            var roomName =roomNameField.GetComponent<InputField>().text;
            _playerNum = (byte)_playerNumArray[playerNumField.GetComponent<Dropdown>().value];
           // _gameMode = _gameModeArray[gameModeField.GetComponent<Dropdown>().value];
            CreateRoom(roomName);
        }
        //Cancel
        else
        {
            roomNameField.GetComponent<InputField>().text = "";
            playerNumField.GetComponent<Dropdown>().value = 0;
           // gameModeField.GetComponent<Dropdown>().value = 0;
            
            DarkBackground.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void CreateRoom(string roomName)
    {
        _ename = roomName;
        _iname = _ename + +Random.Range(0, 9999) + DateTime.Now.ToString(" HH-mm-ss");

        SendMakeRoom();
        PhotonNetwork.JoinOrCreateRoom(_iname, new RoomOptions { MaxPlayers = _playerNum }, null);
    }
    
    private void SendMakeRoom()
    {
        byte[] iname = Encoding.UTF8.GetBytes(_iname);
        byte[] ename = Encoding.UTF8.GetBytes(_ename);

        byte[] sendBuf = new byte[iname.Length + ename.Length + 1 + 3];
        sendBuf[0] = (byte) LoginDBConnection.DBPacketType.MakeRoom;
        sendBuf[1] = (byte) iname.Length;
        sendBuf[2] = (byte) ename.Length;
        sendBuf[3] = (byte) _playerNum;  // maxPlayerNum
        
        Array.Copy(iname, 0, sendBuf, 4, iname.Length);
        Array.Copy(ename, 0, sendBuf, 4 + iname.Length, ename.Length);
        
        _client.Send(sendBuf);
     
    }
}
