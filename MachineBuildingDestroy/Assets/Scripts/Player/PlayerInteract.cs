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
    private float lastTime; // ������ �������� �� ����
    private bool neargoalpost;

    public GameObject _GameUIObject;
    private Slider _Progressbar;
    void Start()
    {
        lastTime = -1;
        gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        gManager = GameManager.GetInstance();
        _GameUIObject = GameObject.Find("Canvas");
        
        for (int i = 0; i < _GameUIObject.transform.childCount; ++i)
        {
            if (_GameUIObject.transform.GetChild(i).name == "Goalprogression")
            {
                _Progressbar = _GameUIObject.transform.GetChild(i).GetComponent<Slider>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
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
                playerState.ResetPoint();
            }
        }
        else
        {
            lastTime = -1;
            _Progressbar.gameObject.SetActive(false);
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goalpost")
        {
            neargoalpost = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Goalpost")
        {
            neargoalpost = false;
        }
    }
}
