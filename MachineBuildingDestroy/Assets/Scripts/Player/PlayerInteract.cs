using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    [FormerlySerializedAs("playerInput")] public GamePlayerInput gamePlayerInput;
    public PlayerState playerState;
    public GameManager gManager;
    // Start is called before the first frame update
    
    public float timeBet = 3f; // ���� ����
    public LayerMask _LayerMask;
    private float lastTime; // ������ �������� �� ����
    private bool neargoalpost;
    
    public UImanager _UImanager;
    private Slider _Progressbar;
    void Start()
    {
        lastTime = -1;
        gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        gManager = GameManager.GetInstance();
        _UImanager = UImanager.GetInstance();
        
        for (int i = 0; i < _UImanager.Canvas.transform.childCount; ++i)
        {
            if (_UImanager.Canvas.transform.GetChild(i).name == "Goalprogression")
            {
                _Progressbar = _UImanager.Canvas.transform.GetChild(i).GetComponent<Slider>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + new Vector3(0f, 2.5f, 0f), 10, _LayerMask);
        if (colliders.Length >= 1)
            neargoalpost = true;
        else
            neargoalpost = false;
        if (gamePlayerInput.Interaction && neargoalpost)
        {
            if (lastTime < 0)
            {
                lastTime = Time.time;
                _Progressbar.gameObject.SetActive(true);
            }
            if (Time.time >= lastTime + timeBet)
            {
                gManager.addScore(playerState.team, playerState.point);
                MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
                myInRoomInfo.Infomations[myInRoomInfo.mySlotNum].TotalGetPoint += playerState.point;
                playerState.ResetPoint();
                _Progressbar.gameObject.SetActive(false);
            }
        }
        else
        {
            _Progressbar.gameObject.SetActive(false);
            lastTime = -1;
        }

        ProgressbarUpdate();
        playerState.update_stat();
    }

    private void ProgressbarUpdate()
    {
        if (_Progressbar.gameObject.activeSelf)
        {
            _Progressbar.value = (Time.time - lastTime) / 3.0f;
        }
        else
            _Progressbar.value = 0;
    }
    
    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.tag == "Goalpost")
    //     {
    //         neargoalpost = true;
    //     }
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.tag == "Goalpost")
    //     {
    //         neargoalpost = false;
    //     }
    // }
}
