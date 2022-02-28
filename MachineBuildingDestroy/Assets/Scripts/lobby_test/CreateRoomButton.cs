using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomButton : MonoBehaviour
{
    public LobbyManager gManager;

    string exroomName;
    string inroomName;

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
        exroomName = "Å×½ºÆ®" + Random.Range(0, 9999);
        inroomName = exroomName + gManager.GetName() + System.DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss");

        StartCoroutine(WebRequest());
    }
    IEnumerator WebRequest()
    {
        WWWForm form = new WWWForm();
        form.AddField("iname", "\""+inroomName+"\"" );
        form.AddField("ename", "\"" + exroomName + "\"");
        form.AddField("nowPnum", 1);
        form.AddField("maxPnum", 6);
        form.AddField("Pname", "\"" + gManager.GetName() + "\"");

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/room_make.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            PhotonNetwork.JoinOrCreateRoom(inroomName, new RoomOptions { MaxPlayers = 6 }, null);
        }
    }
}
