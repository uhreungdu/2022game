using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class RcvEvent : MonoBehaviourPun
{
    [SerializeField]
    private GameManager gManager;
    // Start is called before the first frame update
    void Start()
    {
        //gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnEvent(EventData Evdata)
    {
        // 사용자지정 Event가 아니면 return합니다.
        if (Evdata.Code > System.Enum.GetValues(typeof(NetworkManager.EventCode)).Length) return;

        ParameterDictionary parameters = Evdata.Parameters;
        object[] data = (object[])Evdata.CustomData;
        switch (Evdata.Code)
        {
            case (byte)NetworkManager.EventCode.Test:
                for (int i = 0; i < data.Length; i++)
                    Debug.Log(data[i]);
                break;
            case (byte)NetworkManager.EventCode.SpawnPlayer:
                transform.GetComponent<NetworkManager>().SpawnPlayer();
                break;
            case (byte) NetworkManager.EventCode.StartGame:
            {
                var info = GameObject.Find("Myroominfo");
                int team = -2;
                if (info != null)
                {
                    team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
                }

                transform.GetComponent<NetworkManager>().SetTeamNumOnServerEvent(PhotonNetwork.NickName, team);
                break;
            }
            case (byte) NetworkManager.EventCode.RespawnForReconnect:
            {
                if ((string) parameters[0] != PhotonNetwork.NickName) break;
                StartCoroutine(SpawnPlayerForReconnect((int) parameters[1]));
                //transform.GetComponent<NetworkManager>().SpawnPlayer((int) parameters[1]);
                break;
            }
            case (byte) NetworkManager.EventCode.CreateBuildingFromServer:
            {
                if (!PhotonNetwork.IsMasterClient) break;
                var prefabName = (string) parameters[0];
                var position = new Vector3((float) parameters[1], (float) parameters[2] + 30f, (float) parameters[3]);
                var rotation = new Quaternion((float) parameters[4], (float) parameters[5], (float) parameters[6],
                    (float) parameters[7]);
                PhotonNetwork.InstantiateRoomObject(prefabName, position, rotation);
                break;
            }
            case (byte) NetworkManager.EventCode.DestroyBuildingFromServer:
            {
                if (!PhotonNetwork.IsMasterClient) break;
                var objectList = GameObject.FindGameObjectsWithTag("Wall");
                foreach (var target in objectList)
                {
                    var view = target.GetComponent<PhotonView>();
                    if (view == null) continue;
                    if (view.ViewID != (int) parameters[0]) continue;
                    PhotonNetwork.Destroy(target);
                }

                break;
            }
            case (byte) NetworkManager.EventCode.HideBuildingFragments:
            {
                var objectList = GameObject.FindGameObjectsWithTag("Wall");
                foreach (var target in objectList)
                {
                    var view = target.GetComponent<PhotonView>();
                    if (view == null) continue;
                    if (view.ViewID != (int) parameters[0]) continue;
                    target.GetComponent<BulidingObject>().HideBuilding();
                }

                break;
            }
        }
    }

    IEnumerator SpawnPlayerForReconnect(int team)
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(5f);
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "SampleScene")
            {
                transform.GetComponent<NetworkManager>().SpawnPlayer(team);
                break;
            }
        }
    }
    
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
