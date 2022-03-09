using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomButton : MonoBehaviour
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

    public void CreateRoom()
    {
        gManager.SetExRoomName(_account.GetPlayerNickname()+"의 방" + Random.Range(0, 9999));
        gManager.SetInRoomName(gManager.GetExRoomName() + 
                               _account.GetPlayerNickname() + 
                               System.DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss"));

        StartCoroutine(WebRequest());
    }
    IEnumerator WebRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("iname", "\""+gManager.GetInRoomName()+"\"" );
        form.AddField("ename", "\"" + gManager.GetExRoomName() + "\"");
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
            Debug.Log("Form upload complete!");
            PhotonNetwork.JoinOrCreateRoom(gManager.GetInRoomName(), new RoomOptions { MaxPlayers = 6 }, null);
            GameObject.Find("DelRoomButton").GetComponent<Button>().interactable = true;
            GameObject.Find("SetNameButton").GetComponent<Button>().interactable = false;
            GetComponent<Button>().interactable = false;
            // 시연을 위해 무조건 리스트가 갱신되게 넣은것
            StartCoroutine(gManager.GetRoomList());
        }
    }
}
