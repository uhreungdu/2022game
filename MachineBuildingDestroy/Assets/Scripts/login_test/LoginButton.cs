using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour
{
    [FormerlySerializedAs("IDInput")] public GameObject idInput;
    [FormerlySerializedAs("PWInput")] public GameObject pwInput;
    [FormerlySerializedAs("ErrText")] public GameObject errText;
    public GameObject nManager;
    public GameObject chatClient;
    private Socket _socket;
    [SerializeField] private string[] accountVal;
    private bool _doLogin = false;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_doLogin)
        {
            GetComponent<Button>().interactable = false;
            return;
        }
        GetComponent<Button>().interactable = PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer;
    }

    public void OnClick()
    {
        // �α��� â ��Ȱ��ȭ
        _doLogin = true;
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;
        pwInput.GetComponent<InputField>().interactable = false;
        
        // �α��� ��û
        _socket = ChatClient.GetInstance().GetClientSocket();
        LoginAccount();
    }

    public void LoginAccount()
    {
        byte[] id = Encoding.UTF8.GetBytes(idInput.GetComponent<InputField>().text);
        byte[] pw = Encoding.UTF8.GetBytes(pwInput.GetComponent<InputField>().text);

        byte[] sendBuf = new byte[id.Length + pw.Length + 1 + 2];
        sendBuf[0] = (byte) ChatClient.ChatCode.LoginRequest;
        sendBuf[1] = (byte) id.Length;
        sendBuf[2] = (byte) pw.Length;

        Array.Copy(id, 0, sendBuf, 3, id.Length);
        Array.Copy(pw, 0, sendBuf, 3 + id.Length, pw.Length);
        
        _socket.Send(sendBuf);
    }

    public void ProcessLogin(byte[] result)
    {
        switch (result[2])
        {
            case 0:
            {
                var id = Encoding.UTF8.GetString(result, 8, result[3]);
                var nickname = Encoding.UTF8.GetString(result, 8 + result[3], result[4]);
                var win = result[5];
                var lose = result[6];
                var costume = result[7];
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(id, nickname, win, lose, costume);
                PhotonNetwork.JoinLobby();
                SceneManager.LoadScene("lobby_test");
                break;
            }
            case 1:
            {
                // �α��� ����
                _doLogin = false;
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "ID PW ERR";
                idInput.GetComponent<InputField>().text = "";
                pwInput.GetComponent<InputField>().text = "";
                idInput.GetComponent<InputField>().interactable = true;
                pwInput.GetComponent<InputField>().interactable = true;
                break;
            }
            case 2:
            {
                // ĳ���� �̺���, ���� �ʿ�
                Debug.Log("No Character in account");
                break;
            }
            case 3:
            {
                // �α��� ����
                _doLogin = false;
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "Already Online";
                idInput.GetComponent<InputField>().text = "";
                pwInput.GetComponent<InputField>().text = "";
                idInput.GetComponent<InputField>().interactable = true;
                pwInput.GetComponent<InputField>().interactable = true;
                break;
            }
            case 4:
            {
                var id = Encoding.UTF8.GetString(result, 9, result[3]);
                var nickname = Encoding.UTF8.GetString(result, 9 + result[3], result[4]);
                var roomname = Encoding.UTF8.GetString(result, 9 + result[3] + result[4], result[5]);
                var win = result[6];
                var lose = result[7];
                var costume = result[8];
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "������ �������Դϴ�. �������� �õ��մϴ�.";
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(id, nickname, win, lose, costume);
                PhotonNetwork.JoinRoom(roomname);
                break;
            }
        }
    }
}
