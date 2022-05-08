using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
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
    public bool aftercast { get; set; } // ???직일 ?�� ?��?�� ?���?
    
    public bool stiffen { get; private set; }
    public bool falldown { get; private set; }

    private Vector3 reSpawnTransform;

    public item_box_make.item_type Item { get; set; }
    
    // Start is called before the first frame update
    public AudioClip deathClip;
    public AudioClip hitClip;
    public GameManager gManager;
    public GameObject _AttackGameObject;
    public GameObject nameOnhead;

    private AudioSource playerAudioPlayer;
    private Animator _animator;
    private PlayerAnimator _playerAnimator;
    private CharacterController _characterController;
    public Dmgs_Status P_Dm;
    public GameObject Dead_Effect;
    public Thirdpersonmove _Thirdpersonmove;

    public GameObject coinprefab;
    [SerializeField]
    private float explosionForce;
    private GameObject _networkManager;

    void Start()
    {
        _animator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        _characterController = GetComponent<CharacterController>();
        _playerAnimator = GetComponent<PlayerAnimator>();
        _Thirdpersonmove = GetComponent<Thirdpersonmove>();
        var info = MyInRoomInfo.GetInstance();
        if (info != null)
        {
            team = info.GetComponent<MyInRoomInfo>().mySlotNum % 2;
            //Destroy(info);
        }
        gManager = GameManager.GetInstance();
        gManager.addTeamcount(team);
        
        Item = item_box_make.item_type.Hammer;
        
        P_Dm = new Dmgs_Status();
        P_Dm.Set_St(20f,0f,1f);
        reSpawnTransform = new Vector3(ReSpawnTransformSet(transform.position.x), 
            ReSpawnTransformSet(transform.position.y), 
            ReSpawnTransformSet(transform.position.z));
        Dead_Effect.SetActive(false);
        if (photonView.IsMine)
        {
            photonView.RPC("SetOnHeadName", RpcTarget.All, PhotonNetwork.NickName);
        }

        _networkManager = GameObject.Find("NetworkManager");
        base.OnEnable();
    }
    
    protected override void OnEnable()
    {
        // LivingEntity�� OnEnable()
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

    public void NetworkOtherAnimatorControl(String str, bool b)
    {
        //OnDamage(damage);
        photonView.RPC("AnimatorControl", RpcTarget.AllViaServer, str, b);
    }
    
    [PunRPC]
    public void AnimatorControl(String str, bool b)
    {
        _animator.SetBool(str, b);
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
        photonView.RPC("OnDamage", RpcTarget.AllViaServer, damage);
    }
 
    public override void Die() {
        // LivingEntity?�� Die()�? ?��?��?��?�� 기본 ?���? 처리 ?��?��
        base.Die();
        _animator.SetTrigger("Dead");
        Dead_Effect.SetActive(true);
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        myInRoomInfo.Infomations[myInRoomInfo.mySlotNum].TotalDeath++;
        Invoke("Respawn", 10f);
    }

    public void Respawn()
    {
        _characterController.enabled = false;
        transform.position = reSpawnTransform + new Vector3(Random.Range(-4, 4), 0.0f, Random.Range(-4, 4));
        _characterController.enabled = true;
        _Thirdpersonmove.yvelocity = 0.0f;
        dead = false;
        health = startingHealth;
        Dead_Effect.SetActive(false);
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
        if (_animator.GetBool("Stiffen"))
            return true;
        if (_animator.GetBool("Falldown"))
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

    private void OnDestroy()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.PlayerList.Length < 2) return;
        if (gManager.EManager.gameSet) return;
        DropCoins(point, transform.position);
    }
    
    private void DropCoins(int num, Vector3 pos)
    {
        for (int i = 0; i < num; ++i)
        {
            float radian = (Random.Range(0, 360)) * Mathf.PI / 180;
            float radius = Random.value * 3.0f;
            Vector3 coinPosition = transform.position;
            coinPosition.x = coinPosition.x + (radius * Mathf.Cos(radian));
            coinPosition.z = coinPosition.z + (radius * Mathf.Sin(radian));
            coinPosition.y = coinPosition.y + 10f;
            GameObject coin =
                PhotonNetwork.InstantiateRoomObject(coinprefab.name, coinPosition, coinprefab.transform.rotation);
            Vector3 explosionPosition = transform.position;
            coin.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPosition, 10f, explosionForce / 2);
            coin.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPosition, 10f);
        }
    } 

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 ?��브젝?��?���? ?���? �?분이 ?��?��?��?��?��.
        if (stream.IsWriting)
        {
            stream.SendNext(team);
            stream.SendNext(Item);
            stream.SendNext(point);
            stream.SendNext(health);
        }
        // 리모?�� ?��브젝?��?���? ?���? �?분이 ?��?��?��?��?��.
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
            gManager.player_stat.setting(health,Item,point,startingHealth);
            //print("정보 넘겨줌");
        }
    }

    private void OnApplicationQuit()
    {
    }
}
