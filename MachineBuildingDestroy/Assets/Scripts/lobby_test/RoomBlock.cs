using System.Collections;
using System.Collections.Generic;
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

    private LobbyManager _lobbyManager;
    private Account _account;

    // Start is called before the first frame update
    void Start()
    {
        _lobbyManager = LobbyManager.GetInstance();
        _account = Account.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
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
            GetComponent<Button>().interactable = _nowP != _maxP;
        }
        else
        {
            transform.Find("Text").gameObject.GetComponent<Text>().text = "";
            GetComponent<Button>().interactable = false;
        }
    }

    public void EnterRoom()
    {
        StartCoroutine(WebRequest());
    }

    IEnumerator WebRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("iname", "\"" + _iname + "\"");
        form.AddField("Pname", "\"" + _account.GetPlayerNickname() + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/player_join_room.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            Debug.Log(results);
            _lobbyManager.SetInRoomName(_iname);
            _lobbyManager.SetExRoomName(_ename);
            PhotonNetwork.JoinRoom(_iname);
            //SceneManager.LoadScene("SampleScene");
        }
    }
}
