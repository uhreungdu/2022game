using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGunAttack : PlayerAttack
{
    // Start is called before the first frame update
    public PlayerImpact _playerImpact;
    public GameObject RHandGameObject;
    public GameObject GunObject;
    private Thirdpersonmove _thirdpersonmove;
    
    public List<GameObject> _GunBullets;
    public GameObject _gunBulletGameObject;

    public Gun _Gun;
    
    public bool isDelay = false;
    void Start()
    {
        base.Start();
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _attackName = "총공격";
        _lastColliderActiveTime = 0.4f; // 공격 유지 시간
        _aftercastAttack = 0.8f;
        SetAffterCast(0);
        _damage = 10;
        _Gun = GunObject.GetComponent<Gun>();
        
        _GunBullets = new List<GameObject>();
    }

    void Update()
    {
        HandTransform();
        AfterCastRecovery();
        //ActiveAttack();
    }
    
    public void ActiveAttack()
    {
        if (_hitBoxColliders[0].enabled && ActiveColliderCheck() && !_playerState.IsCrowdControl() && !_thirdpersonmove.IsGrounded() && !_playerState.dead)
        {
            if (_playerState._Currentstatus == PlayerState.Currentstatus.Idle)
                _playerState._Currentstatus = PlayerState.Currentstatus.Attack;
        }
        else if (_hitBoxColliders[0].enabled)
        {
            if (_playerState._Currentstatus == PlayerState.Currentstatus.Attack)
                _playerState._Currentstatus = PlayerState.Currentstatus.Idle;
            _hitBoxColliders[0].enabled = false;
        }
    }
    
    // public void SetHandCollision(int set)
    // {
    //     if (set > 0)
    //     {
    //         _hitBoxColliders[0].enabled = true;
    //         SetActiveAttack();
    //     }
    //     else if (set <= 0)
    //     {
    //         _hitBoxColliders[0].enabled = false;
    //     }
    // }

    private void HandTransform()
    {
        Vector3 WorldRHandPosition = RHandGameObject.transform.position;
    }
    
    public void JumpAttackMovement()
    {
        Transform rootTransform = transform.root;
        _playerImpact.AddImpact(rootTransform.forward, 200);
    }
    
    public void SetJumpAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }

    public IEnumerator ShootBullet()
    {
        //GameObject BulletgameObject = PhotonNetwork.Instantiate(_gunBulletGameObject.name, GunObject.transform.position + -GunObject.transform.right, _gunBulletGameObject.transform.rotation);
        GameObject BulletgameObject = Instantiate(_gunBulletGameObject,
            GunObject.transform.position + -GunObject.transform.right, _gunBulletGameObject.transform.rotation);
        GunBullet BulletgameObjectGunBullet = BulletgameObject.GetComponent<GunBullet>();
        BulletgameObjectGunBullet.ShootPosition = GunObject.transform.position;
        Vector3 returnForwardVector = transform.forward;
        returnForwardVector = Quaternion.Euler(Random.Range(0.0f, 5.0f), Random.Range(0.0f, 5.0f), 0) * transform.forward;
        BulletgameObjectGunBullet.ForwardVector3 = returnForwardVector;
        BulletgameObjectGunBullet.ShootNickName = _playerState.NickName;
        _Gun.Durability -= 1;
        yield return new WaitForSeconds(0.025f);
        isDelay = false;
    }
}
