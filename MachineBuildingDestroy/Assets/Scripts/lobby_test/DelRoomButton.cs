using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class DelRoomButton : MonoBehaviour
{
    private LobbyManager gManager;
    private Account _account;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().interactable = false;
        gManager = LobbyManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeleteRoom()
    {
        StartCoroutine(WebRequest());
    }

    IEnumerator WebRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("iname", "\"" + gManager.GetInRoomName() + "\"");
        form.AddField("Pname", "\"" + _account.GetPlayerNickname() + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/player_exit_room.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            PhotonNetwork.LeaveRoom();
            GameObject.Find("CreateRoomButton").GetComponent<Button>().interactable = true;
            GetComponent<Button>().interactable = false;
            // 시연을 위해 무조건 리스트가 갱신되게 넣은것
            StartCoroutine(gManager.GetRoomList());
        }
    }
}
