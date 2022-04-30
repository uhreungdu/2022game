using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Photon.Pun;
using UnityEngine.UI;

public class ChatClient : MonoBehaviour
{
    private const string ServerAddress = "121.139.87.70";
    private const int Port = 9888;
    private const int BufSize = 128;
    private Socket _client;
    private IPEndPoint _ipep;
    private bool _isDataSend = false;
    
    public byte[] sendbuf = new byte[BufSize-1];
    public byte[] recvbuf = new byte[BufSize];
    public Chatlog chatLog;

    public enum ChatCode : byte
    {
        Normal,
        Exit,
        EnterRoom,
        RoomChat,
        ExitRoom
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _ipep = new IPEndPoint(IPAddress.Parse(ServerAddress), Port);
    }

    private void Update()
    {
        if (_isDataSend)
        {
            chatLog.AddLine(recvbuf);
            _isDataSend = false;
        }
    }

    public Socket GetClientSocket()
    {
        return _client;
    }

    public void ConnectToChatServer()
    {
        _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _client.Connect(_ipep);
        byte[] senddata = Encoding.UTF8.GetBytes(PhotonNetwork.NickName);
        _client.Send(senddata);
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
