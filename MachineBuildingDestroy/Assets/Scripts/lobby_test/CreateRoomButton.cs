using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomButton : MonoBehaviour
{
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

    public void CreateRoom()
    {
        gManager.SetExRoomName(_account.GetPlayerNickname()+"ÀÇ ¹æ" + Random.Range(0, 9999));
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
            PhotonNetwork.JoinOrCreateRoom(gManager.GetInRoomName(), new RoomOptions { MaxPlayers = 6 }, null);
        }
    }
}
