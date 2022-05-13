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
        // 로그인 창 비활성화
        _doLogin = true;
        GetComponent<Button>().interactable = false;
        idInput.GetComponent<InputField>().interactable = false;
        pwInput.GetComponent<InputField>().interactable = false;
        
        // 로그인 요청
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
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(id, nickname);
                PhotonNetwork.JoinLobby();
                SceneManager.LoadScene("lobby_test");
                break;
            }
            case 1:
            {
                // 로그인 오류
                _doLogin = false;
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "ID PW ERR";
                idInput.GetComponent<InputField>().text = "";
                pwInput.GetComponent<InputField>().text = "";
                break;
            }
            case 2:
            {
                // 캐릭터 미보유, 설정 필요
                makeCharWindow.SetActive(true);
                break;
            }
            case 3:
            {
                // 로그인 오류
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
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "게임이 진행중입니다. 재접속을 시도합니다.";
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(id, nickname);
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
            // 로그인 창 활성화
            _doLogin = false;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            accountVal = results.Split(';');
            // 로그인 창 활성화
            GetComponent<Button>().interactable = true;
            idInput.GetComponent<InputField>().interactable = true;
            pwInput.GetComponent<InputField>().interactable = true;
            if (GetStringDataValue(accountVal[0],"Msg:") == "OK")
            {
                // 캐릭터 보유, 로비씬 이동
                errText.SetActive(false);
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(
                    GetStringDataValue(accountVal[0],"account_id:"),
                    GetStringDataValue(accountVal[0],"character_name:"));
                PhotonNetwork.JoinLobby();
                chatClient.GetComponent<ChatClient>().ConnectToChatServer();
                SceneManager.LoadScene("lobby_test");

            }
            else if (GetStringDataValue(accountVal[0],"Msg:") == "INGAME")
            {
                errText.SetActive(true);
                errText.GetComponent<Text>().text = "게임이 진행중입니다. 재접속을 시도합니다.";
                GameObject.Find("Account").GetComponent<Account>().WriteAccount(
                    GetStringDataValue(accountVal[0],"account_id:"),
                    GetStringDataValue(accountVal[0],"character_name:"));
                chatClient.GetComponent<ChatClient>().ConnectToChatServer();
                PhotonNetwork.JoinRoom(GetStringDataValue(accountVal[0],"room_name:"));

            }
            else if (GetStringDataValue(accountVal[0],"Msg:") == "Need Character")
            {
                // 캐릭터 미보유, 설정 필요
                makeCharWindow.SetActive(true);
            }
            else
            {
                // 로그인 오류
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
