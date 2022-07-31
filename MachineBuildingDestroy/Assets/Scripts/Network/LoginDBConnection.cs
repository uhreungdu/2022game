using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine.UI;

public class LoginDBConnection : MonoBehaviour
{
    private static LoginDBConnection instance;
    
    private const string ServerAddress = "121.139.87.70";
    private const int Port = 15001;
    private const int BufSize = 256;
    private Socket _client;
    private IPEndPoint _ipep;
    private bool _isDataSend = false;

    private int _recvCursor = 0;
    private int _readCursor = -1;

    public string roomName;
    
    private byte[] sendbuf = new byte[BufSize-1];
    private byte[] recvbuf = new byte[BufSize];
    public GameObject loginWaitingWindow;
    public GameObject socketErrWindow;
    
    public enum DBPacketType : byte
    {
        Exit,
        EnterRoom,
        ExitRoom,
        MakeRoom,
        LoginRequest,
        LoginResult,
        RoomListRequest,
        RoomListResult,
        AccountInfoRequest,
        AccountInfoResult,
        GameResult,
        EnterRoomResult
    }
    
    public static LoginDBConnection GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LoginDBConnection>();
            if (instance == null)
            {
                GameObject container = new GameObject("LoginDBConnection");
                instance = container.AddComponent<LoginDBConnection>();
            }
        }

        return instance;
    }

    private void Awake()
    {
        var obj = FindObjectsOfType<NetworkManager>();
        _ipep = new IPEndPoint(IPAddress.Parse(ServerAddress), Port);
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        loginWaitingWindow.SetActive(true);
        ConnectToServer();
    }

    private void Update()
    {
        if (!_isDataSend) return;
        if (recvbuf[0] > _recvCursor) return;
        int loopnum = 0;
        while (_readCursor < _recvCursor)
        {
            loopnum++;
            if (loopnum > 10000)
            {
                _isDataSend = false;
                return;
            }

            switch (recvbuf[1])
            {
                case (byte) DBPacketType.LoginResult:
                    GameObject.Find("LoginButton").GetComponent<LoginButton>().ProcessLogin(recvbuf);
                    break;
                case (byte) DBPacketType.RoomListResult:
                    Debug.Log("print");
                    GameObject.Find("RoomList").GetComponent<RoomList>().SetRoomList(recvbuf);
                    break;
                case (byte) DBPacketType.AccountInfoResult:
                    Account.GetInstance().RefreshAccount(recvbuf[2], recvbuf[3]);
                    break;
                case (byte) DBPacketType.EnterRoomResult:
                    switch (recvbuf[2])
                    {
                        case 0:
                            PhotonNetwork.JoinRoom(roomName);
                            break;
                        case 1:
                            Debug.Log("MAX USER");
                            break;
                        case 2:
                            Debug.Log("GAME ALREADY STARTED");
                            break;
                        case 3:
                            Debug.Log("NO ROOM EXIST");
                            break;
                    }
                    break;
            }

            _readCursor = recvbuf[0];
            Array.Copy(recvbuf, _readCursor, recvbuf, 0, BufSize - _readCursor);
            _recvCursor = _recvCursor - _readCursor;
            _readCursor = 0;
        }
        _isDataSend = false;
    }

    public Socket GetClientSocket()
    {
        return _client;
    }

    async void ConnectToServer()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await Task.Run(() =>
        {
            try
            {
                _client.Connect(_ipep);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                socketErrWindow.SetActive(true);
                throw;
            }
        });
        loginWaitingWindow.SetActive(false);
        StartReceive();
    }

    private void StartReceive()
    {
        _client.BeginReceive(recvbuf, 0, BufSize, 0,
            ReceiveCallback, _client);
    }

    private void DisconnectFromChatServer()
    {
        if (!_client.Connected) return;
        sendbuf[0] = (byte)DBPacketType.Exit;
        _client.Send(sendbuf);
        _client.Close();
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        Socket socket = (Socket) ar.AsyncState;

        try
        {
            int recvsize = socket.EndReceive(ar);

            if (recvsize > 0)
            {
                Debug.Log(recvsize);
                _client.BeginReceive(recvbuf, _recvCursor, BufSize-_recvCursor, 0,
                    ReceiveCallback, _client);
                _recvCursor = recvsize;
                _isDataSend = true;
            }
        }
        catch (Exception ex)
        {
            print("Disconnected");
            socketErrWindow.SetActive(true);
            DisconnectFromChatServer();
        }
    }

    private void OnApplicationQuit()
    {
        if (_client == null) return;
        if (!_client.Connected) return;
        sendbuf[0] = (byte) DBPacketType.Exit;
        _client.Send(sendbuf);
    }
}
