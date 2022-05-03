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
        _ename = _account.GetPlayerNickname() + "�� ��" + Random.Range(0, 9999);
        _iname = _ename + System.DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");

        SendEnterRoom();
        PhotonNetwork.JoinOrCreateRoom(_iname, new RoomOptions { MaxPlayers = 6 }, null);
    }
    
    private void SendEnterRoom()
    {
        byte[] iname = Encoding.UTF8.GetBytes(_iname);
        byte[] ename = Encoding.UTF8.GetBytes(_ename);

        byte[] sendBuf = new byte[iname.Length + ename.Length + 1 + 4];
        sendBuf[0] = (byte) ChatClient.ChatCode.MakeRoom;
        sendBuf[1] = (byte) iname.Length;
        sendBuf[2] = (byte) ename.Length;
        sendBuf[3] = (byte) 1;  // nowPlayerNum
        sendBuf[4] = (byte) 6;  // maxPlayerNum
        
        Array.Copy(iname, 0, sendBuf, 5, iname.Length);
        Array.Copy(ename, 0, sendBuf, 5 + iname.Length, ename.Length);
        
        _client.Send(sendBuf);
     
    }
    IEnumerator WebRequest(string ename, string iname)
    {
        WWWForm form = new WWWForm();
        form.AddField("iname", "\""+iname+"\"" );
        form.AddField("ename", "\"" + ename + "\"");
        form.AddField("nowPnum", 1);
        form.AddField("maxPnum", 6);
        form.AddField("Pname", "\"" + _account.GetPlayerNickname() + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/room_make.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            
        }
    }
}
