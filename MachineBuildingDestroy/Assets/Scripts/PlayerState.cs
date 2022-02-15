using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : LivingEntity
{
    public int team { get; private set; }
    public int point { get; private set; }
    // Start is called before the first frame update
    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    void Start()
    {
        playerAnimator = GetComponent<Animator>();          // ���� �ȵ�
        playerAudioPlayer = GetComponent<AudioSource>();    // ���� �ȵ�
        base.OnEnable();
    }
    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        team = Random.Range(0, 2);
        point = 0;
    }

    public void GainPoint(int Point)
    {
        point += Point;
    }

    public void ResetPoint()
    {
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
