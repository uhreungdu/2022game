using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;
// using static item_box_make.item_type;

public class PlayerState : LivingEntity, IPunObservable
{
    public int team { get; private set; }
    public int point { get; private set; }
    
    public bool isAimming{ get; set; }
    public bool nowEquip{ get; set; }

    public float _aftercastAttack { get; set; }
    public float _lastAttackTime { get; set; }
    public bool aftercast { get; set; } // 움직일 수 없는 시간
    
    public bool stiffen { get; private set; }
    public bool falldown { get; private set; }

    private Vector3 reSpawnTransform;

    public item_box_make.item_type Item { get; private set; }
    
    // Start is called before the first frame update
    public AudioClip deathClip; // ��� �Ҹ�
    public AudioClip hitClip; // �ǰ� �Ҹ�
    public GameManager gManager;
    public GameObject _AttackGameObject;
    public GameObject nameOnhead;

    private AudioSource playerAudioPlayer; // �÷��̾� �Ҹ� �����
    private Animator _animator; // �÷��̾��� �ִϸ�����
    private PlayerAnimator _playerAnimator; // �÷��̾��� �ִϸ�����
    private CharacterController _characterController;
    public Dmgs_Status P_Dm;
    

    void Start()
    {
        _animator = GetComponent<Animator>();          // ���� �ȵ�
        playerAudioPlayer = GetComponent<AudioSource>();    // ���� �ȵ�
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        var info = GameObject.Find("Myroominfo");
        if (info != null)
        {
            team = Convert.ToInt32(info.GetComponent<MyInRoomInfo>().MySlotNum > 2);
            Destroy(info);
        }
        gManager = GameManager.GetInstance();
        gManager.addTeamcount(team);
        
        Item = item_box_make.item_type.obstacles;
        P_Dm = new Dmgs_Status();
        P_Dm.Set_St(20f,0f,1f);
        reSpawnTransform = new Vector3(ReSpawnTransformSet(transform.position.x), 
            ReSpawnTransformSet(transform.position.y), 
            ReSpawnTransformSet(transform.position.z));
        photonView.RPC("SetOnHeadName",RpcTarget.All,PhotonNetwork.NickName);
        
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
        _playerAnimator.lastStiffenTime = Time.time;
        _playerAnimator.lastFalldownTime = Time.time;
        BoxCollider[] _attackboxColliders = _AttackGameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider child in _attackboxColliders)
        { 
            child.enabled = false;
        }
    }

    public void NetworkOnDamage(float damage)
    {
        //OnDamage(damage);
        photonView.RPC("OnDamage", RpcTarget.Others, damage);
    }

    public override void Die() {
        // LivingEntity의 Die()를 실행하여 기본 사망 처리 실행
        base.Die();
        _animator.SetTrigger("Dead");
        Invoke("Respawn", 10f);
    }

    public void Respawn()
    {
        _characterController.enabled = false;
        transform.position = reSpawnTransform + new Vector3(Random.Range(-4, 4), 0.0f, Random.Range(-4, 4));
        _characterController.enabled = true;
        dead = false;
        health = startingHealth;
        _animator.Rebind();
    }
    
    public void SetStiffen(int set)
    {
        if (set >= 1)
            stiffen = true;
        else if (set < 1)
            stiffen = false;
    }
    public void SetFalldown(int set)
    {
        if (set >= 1)
            falldown = true;
        else if (set < 1)
            falldown = false;
    }

    public bool IsCrowdControl()
    {
        if (stiffen)
            return true;
        if (falldown)
            return true;
        return false;
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

    [PunRPC]
    public void SetOnHeadName(string value)
    {
        nameOnhead.GetComponent<TextMesh>().text = value;
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
            //print("정보 넘겨줌");
        }
    }
}
