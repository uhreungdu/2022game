using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    public LobbyManager gManager;

    // Start is called before the first frame update
    void Start()
    {
        gManager = LobbyManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        StartCoroutine(gManager.GetRoomList());
    }
}
