using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerState playerState;
    public GameManager gManager;
    // Start is called before the first frame update


    public float timeBet = 3f; // ���� ����
    private float lastTime; // ������ �������� �� ����
    private bool neargoalpost;

    void Start()
    {
        lastTime = -1;
        neargoalpost = false;
        playerInput = GetComponentInParent<PlayerInput>();
        playerState = GetComponentInParent<PlayerState>();
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.Interaction && neargoalpost)
        {
            if (lastTime < 0)
                lastTime = Time.time;
            if (Time.time >= lastTime + timeBet)
            {
                lastTime = -1;
                gManager.setScore(playerState.team, playerState.point);
                playerState.ResetPoint();
            }
        }
        else
        {
            lastTime = -1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (other.tag == "Goalpost")
        {
            neargoalpost = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if (other.tag == "Goalpost")
        {
            neargoalpost = false;
        }
    }
    
}
