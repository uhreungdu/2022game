using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerState : LivingEntity,IPunObservable
{
    public int team { get; private set; }
    public int point { get; private set; }

    public item_box_make.item_type Item { get; private set; }
    
    // Start is called before the first frame update
    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public GameManager gManager;

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����

    void Start()
    {
        playerAnimator = GetComponent<Animator>();          // ���� �ȵ�
        playerAudioPlayer = GetComponent<AudioSource>();    // ���� �ȵ�
        var info = GameObject.Find("Myroominfo");
        if (info != null)
        {
            team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
            Destroy(info);
        }
        gManager = GameManager.GetInstance();

        gManager.addTeamcount(team);
        Item = item_box_make.item_type.potion;
        base.OnEnable();
    }
    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        // onDeath += DieAction;
        point = 0;
    }

    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        playerAnimator.SetTrigger("Stiffen");
    }

    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();
        playerAnimator.SetTrigger("Dead");
    }
    public void DieAction()
    {
        gameObject.SetActive(false);
    }

    public void AddPoint(int Point)
    {
        point += Point;
    }
    
    
    public void SetItem(item_box_make.item_type dItemType)
    {
        Item = dItemType;
    }

    public void ResetPoint()
    {
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(team);
            stream.SendNext(Item);
            stream.SendNext(point);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            team = (int) stream.ReceiveNext();
            Item = (item_box_make.item_type) stream.ReceiveNext();
            point = (int) stream.ReceiveNext();
        }
    }
}
