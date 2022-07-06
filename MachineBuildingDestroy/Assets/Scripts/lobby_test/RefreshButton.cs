using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    private LobbyManager _lobbyManager;
    private RoomList _roomList;

    // Start is called before the first frame update
    void Start()
    {
        _lobbyManager = LobbyManager.GetInstance();
        _roomList = GameObject.Find("RoomList").GetComponent<RoomList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        _roomList.CleanRoomList();
        _lobbyManager.GetRoomList();
    }
}
