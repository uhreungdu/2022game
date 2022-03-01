using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class DelRoomButton : MonoBehaviour
{
    public LobbyManager gManager;
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
        form.AddField("Pname", "\"" + gManager.GetName() + "\"");

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
            GameObject.Find("SetNameButton").GetComponent<Button>().interactable = true;
            GetComponent<Button>().interactable = false;
        }
    }
}
