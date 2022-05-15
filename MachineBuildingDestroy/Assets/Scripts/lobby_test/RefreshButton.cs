using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    private LobbyManager _lobbyManager;

    // Start is called before the first frame update
    void Start()
    {
        _lobbyManager = LobbyManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        _lobbyManager.GetRoomList();
    }
}
