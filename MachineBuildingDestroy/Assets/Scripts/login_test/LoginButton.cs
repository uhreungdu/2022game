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
    [FormerlySerializedAs("MakeCharWindow")] public GameObject makeCharWindow;
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
        chatClient.GetComponent<ChatClient>().ConnectToChatServer();
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
        switch (result[1])
        {
            case 0:
            {
                var id = Encoding.UTF8.GetString(result, 4, result[2]);
                var nickname = Encoding.UTF8.GetString(result, 4 + result[2], result[3]);
                var length = 4 + result[2] + result[3] + 3;
                var win = result[length - 2 - 1];
                var lose = result[length - 1 - 1];
                var costume = result[length - 1];
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
                break;
            }
            case 2:
            {
                // ĳ���� �̺���, ���� �ʿ�
                makeCharWindow.SetActive(true);
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
                break;
            }
            case 4:
            {
                var id = Encoding.UTF8.GetString(result, 5, result[2]);
                var nickname = Encoding.UTF8.GetString(result, 5 + result[2], result[3]);
                var roomname = Encoding.UTF8.GetString(result, 5 + result[2] + result[3], result[4]);
                var length = 5 + result[2] + result[3] + result[4] + 3;
                var win = result[length - 2 - 1];
                var lose = result[length - 1 - 1];
                var costume = result[length - 1];
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "������ �������Դϴ�. �������� �õ��մϴ�.";
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(id, nickname, win, lose, costume);
                PhotonNetwork.JoinRoom(roomname);
                break;
            }
        }
    }
    
    public IEnumerator LoginRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", "\""+idInput.GetComponent<InputField>().text+"\"") ;
        form.AddField("pw", "\"" + pwInput.GetComponent<InputField>().text + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/login/login_account.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            // �α��� â Ȱ��ȭ
            _doLogin = false;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            accountVal = results.Split(';');
            // �α��� â Ȱ��ȭ
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
            if (GetStringDataValue(accountVal[0],"Msg:") == "OK")
            {
                // ĳ���� ����, �κ�� �̵�
                errText.SetActive(false);
                PhotonNetwork.JoinLobby();
                chatClient.GetComponent<ChatClient>().ConnectToChatServer();
                SceneManager.LoadScene("lobby_test");

            }
            else if (GetStringDataValue(accountVal[0],"Msg:") == "INGAME")
            {
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "������ �������Դϴ�. �������� �õ��մϴ�.";
                chatClient.GetComponent<ChatClient>().ConnectToChatServer();
                PhotonNetwork.JoinRoom(GetStringDataValue(accountVal[0],"room_name:"));

            }
            else if (GetStringDataValue(accountVal[0],"Msg:") == "Need Character")
            {
                // ĳ���� �̺���, ���� �ʿ�
                makeCharWindow.SetActive(true);
            }
            else
            {
                // �α��� ����
                _doLogin = false;
                errText.SetActive(true);
                errText.GetComponent<Text>().text = GetStringDataValue(accountVal[0],"Msg:");
                idInput.GetComponent<InputField>().text = "";
                pwInput.GetComponent<InputField>().text = "";
            }
        }
    }
    
    string GetStringDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|")) value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
