using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using ExitGames.Client.Photon;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static LobbyManager instance;
    public GameObject roomlist;

    public static LobbyManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<LobbyManager>();
            if (instance == null)
            {
                GameObject container = new GameObject("LobbyManager");
                instance = container.AddComponent<LobbyManager>();
            }
        }
        return instance;
    }

    void Awake()
    {

    }
    void Start()
    {
        StartCoroutine(GetRoomList());
    }

    // Update is called once per frame
    void Update()
    {
    }
    


    public IEnumerator GetRoomList()
    {
        WWWForm form = new WWWForm();

        UnityWebRequest www = UnityWebRequest.Post("http://121.139.87.70/get_room_list.php", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string results = www.downloadHandler.text;
            roomlist.GetComponent<RoomList>().SetRoomList(results.Split(';'));
            roomlist.GetComponent<RoomList>().SetRoomBlocks();
        }
    }
}
