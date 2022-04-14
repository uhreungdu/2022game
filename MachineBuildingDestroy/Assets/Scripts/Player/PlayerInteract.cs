using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInteract : MonoBehaviour
{
    [FormerlySerializedAs("playerInput")] public GamePlayerInput gamePlayerInput;
    public PlayerState playerState;
    public GameManager gManager;
    // Start is called before the first frame update


    public float timeBet = 3f; // ���� ����
    private float lastTime; // ������ �������� �� ����
    private bool neargoalpost;

    void Start()
    {
        lastTime = -1;
        gamePlayerInput = GetComponentInParent<GamePlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePlayerInput.Interaction && neargoalpost)
        {
            if (lastTime < 0)
                lastTime = Time.time;
            if (Time.time >= lastTime + timeBet)
            {
                lastTime = -1;
                gManager.addScore(playerState.team, playerState.point);
                playerState.ResetPoint();
            }
        }
        else
        {
            lastTime = -1;
        }
        playerState.update_stat();
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
