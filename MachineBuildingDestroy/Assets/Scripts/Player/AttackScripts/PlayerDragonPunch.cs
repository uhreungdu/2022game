using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerDragonPunch : PlayerAttack
{
    // Start is called before the first frame update
    public PlayerImpact _playerImpact;
    public BoxCollider _HandBoxCollider;
    public GameObject LHandGameObject;
    private Thirdpersonmove _thirdpersonmove;
    void Start()
    {
        base.Start();
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _attackName = "기본공격";
        _lastColliderActiveTime = 0.4f; // 공격 유지 시간
        _aftercastAttack = 0.6f;
        SetAffterCast(0);
        _damage = 30;
        //_coolTime = 10;
        _coolTime = 0;
        _lastUsedTime = -999f;
        _AttackAudioClips.Add(Resources.Load<AudioClip>("Sounds/Punch"));
        
        _hitBoxColliders.Add(_HandBoxCollider);
    }

    void Update()
    {
        //HandTransform();
        AfterCastRecovery();
        ActiveRAttack();
    }
    
    public void ActiveRAttack()
    {
        if (_hitBoxColliders[0].enabled && ActiveColliderCheck() && !_playerState.IsCrowdControl() && !_playerState.dead)
        {
            if (_playerState._Currentstatus == PlayerState.Currentstatus.Idle)
                _playerState._Currentstatus = PlayerState.Currentstatus.SupergardAttack;
        }
        else if (_hitBoxColliders[0].enabled)
        {
            if (_playerState._Currentstatus == PlayerState.Currentstatus.SupergardAttack)
                _playerState._Currentstatus = PlayerState.Currentstatus.Idle;
            SetDragonPunchCollision(0);
        }
    }

    public void SetDragonPunchCollision(int set)
    {
        if (set > 0)
        {
            _hitBoxColliders[0].enabled = true;
            SetActiveAttack();
        }
        else if (set <= 0)
        {
            _hitBoxColliders[0].enabled = false;
        }
    }

    private void HandTransform()
    {
        Vector3 WorldRHandPosition = LHandGameObject.transform.position;
        _HandBoxCollider.transform.position = WorldRHandPosition;
    }
    
    public void DragonPunchMovement()
    {
        Transform rootTransform = transform.root;
        _thirdpersonmove.yvelocity = 1.5f;
        //_playerImpact.AddImpact(rootTransform.up, 500);
        _playerImpact.AddImpact(rootTransform.forward, 150);
        //photonView.RPC("Setyvelocity", RpcTarget.AllViaServer);
    }
    
    [PunRPC]
    public void Setyvelocity(float vel)
    {
        _thirdpersonmove.yvelocity = vel;
    }

    
    public void SetDragonPunchAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }
}
