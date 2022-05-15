using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine.UI;

public class ChatClient : MonoBehaviour
{
    private static ChatClient instance;
    
    private const string ServerAddress = "121.139.87.70";
    private const int Port = 9887;
    private const int BufSize = 128;
    private Socket _client;
    private IPEndPoint _ipep;
    private bool _isDataSend = false;
    private Task _loginTask; 
    
    public byte[] sendbuf = new byte[BufSize-1];
    public byte[] recvbuf = new byte[BufSize];
    public Chatlog chatLog;
    public GameObject loginWaitingWindow;
    
    public enum ChatCode : byte
    {
        Normal,
        Exit,
        EnterRoom,
        RoomChat,
        ExitRoom,
        MakeRoom,
        LoginRequest,
        LoginResult
    }
    
    public static ChatClient GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<ChatClient>();
            if (instance == null)
            {
                GameObject container = new GameObject("ChatClient");
                instance = container.AddComponent<ChatClient>();
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
        ConnectToChatServer();
    }

    private void Update()
    {
        if (_isDataSend)
        {
            switch (recvbuf[0])
            {
                case (byte)ChatCode.LoginResult:
                    GameObject.Find("LoginButton").GetComponent<LoginButton>().ProcessLogin(recvbuf);
                    break;
                default:
                    chatLog.AddLine(recvbuf);
                    break;
            }
            
            _isDataSend = false;
        }
    }

    public Socket GetClientSocket()
    {
        return _client;
    }

    async void ConnectToChatServer()
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
                throw;
            }
        });
        loginWaitingWindow.SetActive(false);
        _client.BeginReceive(recvbuf, 0, BufSize, 0,
            ReceiveCallback, _client);
    }

    public void StartReceive()
    {
        _client.BeginReceive(recvbuf, 0, BufSize, 0,
            ReceiveCallback, _client);
    }

    public void DisconnectFromChatServer()
    {
        if (!_client.Connected) return;
        sendbuf[0] = (byte)ChatCode.Exit;
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
                _isDataSend = true;
                _client.BeginReceive(recvbuf, 0, BufSize, 0,
                    ReceiveCallback, _client);
            }
        }
        catch (Exception ex)
        {
            print("Disconnected");
            DisconnectFromChatServer();
        }
    }

    private void OnApplicationQuit()
    {
        if (_client == null) return;
        if (!_client.Connected) return;
        sendbuf[0] = (byte) ChatCode.Exit;
        _client.Send(sendbuf);
    }
}
