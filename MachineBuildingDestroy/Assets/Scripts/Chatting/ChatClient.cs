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
    private Chatlog _chatlogFunc;
    private string _data;
    private bool _isDataSend = false;
    
    public byte[] sendbuf = new byte[BufSize-1];
    public byte[] recvbuf = new byte[BufSize];
    public GameObject Chatlog_obj;

    enum ChatCode : byte
    {
        Normal,
        Exit
    }

    private void Awake()
    {
        _ipep = new IPEndPoint(IPAddress.Parse(ServerAddress), Port);
        _chatlogFunc = Chatlog_obj.GetComponent<Chatlog>();
    }

    private void Update()
    {
        if (_isDataSend)
        {
            _chatlogFunc.AddLine(_data);
            _isDataSend = false;
        }
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

    public void SendChat(GameObject obj)
    {
        // InputField에서 텍스트 받아오기
        var inputfield = obj.GetComponent<InputField>();
        string msg = inputfield.text;
        
        // send버퍼 초기화 후 메세지 받아오기
        sendbuf.Initialize();
        sendbuf = Encoding.UTF8.GetBytes(msg);
        
        // 메세지 코드를 temp에 저장 
        byte[] tempbuf = new byte[sendbuf.Length + 1];
        tempbuf[0] = (byte) ChatCode.Normal;

        // 메세지를 temp뒤에 병합
        Array.Copy(sendbuf, 0, tempbuf, 1, sendbuf.Length);
        
        // 메세지 전송 후 InputField비우기
        _client.Send(tempbuf);
        inputfield.text = string.Empty;
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        _data = string.Empty;
        Socket socket = (Socket) ar.AsyncState;

        try
        {
            int recvsize = socket.EndReceive(ar);

            if (recvsize > 0)
            {
                _data = Encoding.UTF8.GetString(recvbuf, 0, recvsize);
                print(_data);
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
        if (!_client.Connected) return;
        sendbuf[0] = (byte) ChatCode.Exit;
        _client.Send(sendbuf);
    }
}
