using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
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
                ename + "\n인원: " + nowP + "/" + maxP;
            GetComponent<Button>().interactable = true;
        }
        else
        {
            transform.Find("Text").gameObject.GetComponent<Text>().text = "";
            GetComponent<Button>().interactable = false;
        }
        if(_account.GetPlayerNickname() == "")
        {
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
            PhotonNetwork.JoinRoom(iname);
            GameObject.Find("DelRoomButton").GetComponent<Button>().interactable = true;
            GameObject.Find("SetNameButton").GetComponent<Button>().interactable = false;
            GetComponent<Button>().interactable = false;
            // 시연을 위해 무조건 리스트가 갱신되게 넣은것
            StartCoroutine(gManager.GetRoomList());
            string results = www.downloadHandler.text;
            Debug.Log(results);

            gManager.SetInRoomName(iname);
            gManager.SetExRoomName(ename);
        }
    }
}
