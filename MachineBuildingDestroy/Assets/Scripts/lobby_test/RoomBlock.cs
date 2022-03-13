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
    public string iname;
    public string ename;
    public int nowP;
    public int maxP;

    private LobbyManager gManager;
    private Account _account;

    // Start is called before the first frame update
    void Start()
    {
        gManager = LobbyManager.GetInstance();
        _account = Account.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVariables(string internal_name, string external_name, int nowPlayerNum, int maxPlayerNum)
    {
        iname = internal_name;
        ename = external_name;
        nowP = nowPlayerNum;   
        maxP = maxPlayerNum;
        SetText();
    }

    void SetText()
    {
        if (iname != "")
        {
            transform.Find("Text").gameObject.GetComponent<Text>().text =
                ename + "\nÀÎ¿ø: " + nowP + "/" + maxP;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            transform.Find("Text").gameObject.GetComponent<Text>().text = "";
            GetComponent<Button>().interactable = false;
        }
        if(GameObject.Find("CreateRoomButton").GetComponent<Button>().interactable == false)
        {
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
        form.AddField("iname", "\"" + iname + "\"");
        form.AddField("Pname", "\"" + _account.GetPlayerNickname() + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/player_join_room.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            gManager.SetInRoomName(iname);
            gManager.SetExRoomName(ename);
            PhotonNetwork.JoinRoom(iname);
            SceneManager.LoadScene("SampleScene");
        }
    }
}
