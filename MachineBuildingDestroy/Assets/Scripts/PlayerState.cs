using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : LivingEntity
{
    public int team { get; private set; }
    // Start is called before the first frame update
    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    void Start()
    {
        //playerAnimator = GetComponent<Animator>();          // ���� �ȵ�
        //playerAudioPlayer = GetComponent<AudioSource>();    // ���� �ȵ�
        base.OnEnable();
    }
    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        team = Random.Range(0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
