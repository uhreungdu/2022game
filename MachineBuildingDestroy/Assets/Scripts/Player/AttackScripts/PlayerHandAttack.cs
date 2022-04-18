using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEditor;
using UnityEngine;

// 오브젝트에 직접 들어가는 공격 스크립트.
// 애니메이션 이벤트로 작동시키기 위한 요소들
public class PlayerHandAttack : PlayerAttack
{
    // Start is called before the first frame update
    public PlayerState _playerState;
    public PlayerImpact _playerImpact;
    public BoxCollider _lHandBoxCollider;
    public BoxCollider _rHandBoxCollider;
    public GameObject LHandGameObject;
    public GameObject RHandGameObject;
    
    protected float _lastLColliderOnTime; // 콜라이더가 켜진 시점
    protected float _lastLColliderOffTime; // 공격을 마지막에 한 시점
    protected float _lastRColliderOnTime; // 콜라이더가 켜진 시점
    protected float _lastRColliderOffTime; // 공격을 마지막에 한 시점
    void Start()
    {
        _playerState = transform.GetComponent<PlayerState>();
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _attackName = "기본공격";
        _aftercastAttack = 0.3f; // 공격 간격
        _activeAttackTime = 0f; // 공격 유지 시간
        _lastAttackTime = 0f; // 공격을 마지막에 한 시점
        SetAffterCast(0);
        _damage = 10;
        
        _hitBoxColliders.Add(_lHandBoxCollider);
        _hitBoxColliders.Add(_rHandBoxCollider);
    }

    private void Update()
    {
        HandTransform();
        AfterCaseRecovery();
    }

    private void HandTransform()
    {
        Vector3 WorldLHandPosition = LHandGameObject.transform.position;
        Vector3 WorldRHandPosition = RHandGameObject.transform.position;
        _lHandBoxCollider.transform.position = WorldLHandPosition;
        _rHandBoxCollider.transform.position = WorldRHandPosition;
    }

    public void SetLHandCollision(int set)
    {
        if (set > 0)
        {
            _hitBoxColliders[0].enabled = true;
            _lastLColliderOnTime = Time.time;
        }
        else if (set <= 0)
        {
            _hitBoxColliders[0].enabled = false;
            _lastLColliderOffTime = Time.time;
            print("왼손 지속시간" + (_lastLColliderOffTime - _lastLColliderOnTime) + "초");
        }
    }

    public void SetRHandCollision(int set)
    {
        if (set > 0)
        {
            _hitBoxColliders[1].enabled = true;
            _lastRColliderOnTime = Time.time;
        }
        else if (set <= 0)
        {
            _hitBoxColliders[1].enabled = false;
            _lastRColliderOffTime = Time.time;
            print("오른손 지속시간" + (_lastRColliderOffTime - _lastRColliderOnTime) + "초");
        }
    }

    public void AttackMovement(int combo)
    {
        Transform rootTransform = transform.root;
        
        switch (combo)
        {
            case 0:
                _playerImpact.AddImpact(rootTransform.forward, 10);
                break;
            
            case 1:
                _playerImpact.AddImpact(rootTransform.forward, 10);
                break;
            
            case 2:
                _playerImpact.AddImpact(rootTransform.forward, 15);
                break;
        }
    }

    public void SetAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }
}
