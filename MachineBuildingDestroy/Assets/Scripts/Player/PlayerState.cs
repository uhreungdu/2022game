using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class PlayerState : LivingEntity,IPunObservable
{
    public int team { get; private set; }
    public int point { get; private set; }
    
    public bool isAimming{ get;  set; }
    public bool nowEquip{ get;  set; }

    private Vector3 reSpawnTransform;

    public item_box_make.item_type Item { get; private set; }
    
    // Start is called before the first frame update
    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public GameManager gManager;

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator playerAnimator; // �÷��̾��� �ִϸ�����
    private CharacterController _characterController;
    public Dmgs_Status P_Dm;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();          // ���� �ȵ�
        playerAudioPlayer = GetComponent<AudioSource>();    // ���� �ȵ�
        _characterController = GetComponent<CharacterController>();
        var info = GameObject.Find("Myroominfo");
        if (info != null)
        {
            team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
            Destroy(info);
        }
        gManager = GameManager.GetInstance();
        gManager.addTeamcount(team);

        reSpawnTransform = transform.position;

        reSpawnTransform.x = ReSpawnTransformSet(reSpawnTransform.x);
        reSpawnTransform.y = ReSpawnTransformSet(reSpawnTransform.y);
        reSpawnTransform.z = ReSpawnTransformSet(reSpawnTransform.z);

        Item = item_box_make.item_type.potion;
        P_Dm = new Dmgs_Status();
        P_Dm.Set_St(20f,0f,1f);
        base.OnEnable();
    }
    
    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable() ���� (���� �ʱ�ȭ)
        // onDeath += DieAction;
        point = 0;
    }

    public float ReSpawnTransformSet(float value)
    {
        if (value <= -1)
        {
            value /= 10;
            value = (int) value;
            value *= 10;
            value -= 5;
        }
        else if (value >= 1)
        {
            value /= (int)10;
            value = (int) value;
            value *= 10;
            value += 5;
        }
        else
            value = 0;
        return value;
    }

    [PunRPC]
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
        playerAnimator.SetTrigger("Stiffen");
    }

    public void NetworkOnDamage(float damage)
    {
        //OnDamage(damage);
        photonView.RPC("OnDamage", RpcTarget.Others, damage);
    }

    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();
        playerAnimator.SetTrigger("Dead");
        Invoke("Respawn", 10f);
    }

    public void Respawn()
    {
        _characterController.enabled = false;
        transform.position = reSpawnTransform + new Vector3(Random.Range(-4, 4), 0.0f, Random.Range(-4, 4));
        _characterController.enabled = true;
        playerAnimator.Rebind();
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
        //Item = dItemType;
        photonView.RPC("SetItemRPC", RpcTarget.All, dItemType);
    }

    [PunRPC]
    public void SetItemRPC(item_box_make.item_type dItemType)
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
            stream.SendNext(health);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            team = (int) stream.ReceiveNext();
            Item = (item_box_make.item_type) stream.ReceiveNext();
            point = (int) stream.ReceiveNext();
            health = (float)stream.ReceiveNext();
        }
    }

    public void update_stat()
    {
        if (photonView.IsMine)
        {
            gManager.player_stat.setting(health,Item);
            print("정보 넘겨줌");
        }
    }
}
