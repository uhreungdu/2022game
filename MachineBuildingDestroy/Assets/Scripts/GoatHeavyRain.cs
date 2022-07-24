using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class GoatHeavyRain : MonoBehaviour
{
    public struct GoatHeavyRainInfo
    {
        public Vector3 StartPosition;
        public Vector3 EndPosition;
        public Vector3 Path;
    }

    public Map map;
    public GameObject GoatHeavyRainPref;
    private List<GoatHeavyRainInfo> GoatHeavyRainInfos = new List<GoatHeavyRainInfo>();
    private bool GoatHeavyRainDelay;
    private GameManager _gameManager;

    public void Start()
    {
        if (map == null)
        {
            // 부하가 많은 작업 손으로 집어넣지 않았을 경우 방지
            map = GameObject.Find("Map").GetComponent<Map>();
        }
        if (GoatHeavyRainPref == null)
            GoatHeavyRainPref = Resources.Load<GameObject>("Effect/GoatHeavyRain");
        GoatHeavyRainDelay = false;
        _gameManager = GameManager.GetInstance();
    }

    public void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        if (GoatHeavyRainDelay == false && _gameManager.EManager.goatheavyrain_Create)
            StartCoroutine(HeavyRainLoop());
        // DebugGoatHeavyRainInfo();
    }

    IEnumerator HeavyRainLoop()
    {
        GoatHeavyRainDelay = true;
        GoatHeavyRainInfos.Add(GoatHeavyRainInfoCreate());
        yield return new WaitForSeconds(1.5f);
        GoatHeavyRainDelay = false;
    }

    // 궤적 한개 생성
    GoatHeavyRainInfo GoatHeavyRainInfoCreate()
    {
        GoatHeavyRainInfo returnGoatHeavyRainInfo = new GoatHeavyRainInfo();
        RaycastHit raycastHit;
        int safety = 0; // while문 안전장치
        while (safety < 100)
        {
            returnGoatHeavyRainInfo.StartPosition = new Vector3(Random.Range(map.MinimumX - 30, map.MaximumX + 30), 100,
                Random.Range(map.MinimumZ - 30, map.MaximumZ + 30));
            returnGoatHeavyRainInfo.EndPosition = new Vector3(Random.Range(map.MinimumX, map.MaximumX), 0,
                Random.Range(map.MinimumZ, map.MaximumZ));
            returnGoatHeavyRainInfo.Path =
                (returnGoatHeavyRainInfo.EndPosition - returnGoatHeavyRainInfo.StartPosition).normalized;
            var ray = new Ray(returnGoatHeavyRainInfo.StartPosition, returnGoatHeavyRainInfo.Path);
            if (Physics.Raycast(ray,
                    (returnGoatHeavyRainInfo.EndPosition - returnGoatHeavyRainInfo.StartPosition).magnitude))
            {
                break;
            }
            safety++;
        }
        
        GameObject GoatHeavyRainObj = PhotonNetwork.InstantiateRoomObject("Effect/GoatHeavyRain",
            returnGoatHeavyRainInfo.StartPosition, Quaternion.LookRotation(returnGoatHeavyRainInfo.Path));
        GoatHeavyRainObject GoatHeavyRainObject = GoatHeavyRainObj.GetComponent<GoatHeavyRainObject>();
        GoatHeavyRainObject._goatHeavyRainInfo = returnGoatHeavyRainInfo;
        
        GameObject DangerzoneObj = PhotonNetwork.InstantiateRoomObject("Effect/Dangerzone",
            returnGoatHeavyRainInfo.EndPosition, Quaternion.identity);
        GoatHeavyRainObject.Dangerzone = DangerzoneObj;
        
        return returnGoatHeavyRainInfo;
    }

    void DebugGoatHeavyRainInfo()
    {
        foreach (var Info in GoatHeavyRainInfos)
        {
            Debug.DrawRay(Info.StartPosition,
                Info.Path * (Info.EndPosition - Info.StartPosition).magnitude, 
                Color.red);
        }
    }
}