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
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _attackName = "기본공격";
        SetAffterCast(0);
        _lastColliderActiveTime = 0.23f; // 공격 유지 시간
        _aftercastAttack = 0.3f;
        _damage = 10;
        
        _hitBoxColliders.Add(_lHandBoxCollider);
        _hitBoxColliders.Add(_rHandBoxCollider);
    }

    private void Update()
    {
        HandTransform();
        AfterCastRecovery();
        ActiveLAttack();
        ActiveRAttack();
    }
    public void ActiveLAttack()
    {
        if (_hitBoxColliders[0].enabled && ActiveColliderCheck())
        {
            return;
        }
        else if (_hitBoxColliders[0].enabled)
        {
            SetLHandCollision(0);
        }
    }
    
    public void ActiveRAttack()
    {
        if (_hitBoxColliders[1].enabled && ActiveColliderCheck())
        {
            return;
        }
        else if (_hitBoxColliders[1].enabled)
        {
            SetRHandCollision(0);
        }
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
            _lHandBoxCollider.enabled = true;
            _lastColliderOnTime = Time.time;
        }
        else if (set <= 0)
        {
            _lHandBoxCollider.enabled = false;
            // print("왼손 지속시간" + (_lastLColliderOffTime - _lastLColliderOnTime) + "초");
        }
    }

    public void SetRHandCollision(int set)
    {
        if (set > 0)
        {
            _rHandBoxCollider.enabled = true;
            _lastColliderOnTime = Time.time;
        }
        else if (set <= 0)
        {
            _rHandBoxCollider.enabled = false;
        }
    }

    public void AttackMovement(int combo)
    {
        Transform rootTransform = transform.root;
        
        switch (combo)
        {
            case 0:
                _playerImpact.AddImpact(rootTransform.forward, 40);
                break;
            
            case 1:
                _playerImpact.AddImpact(rootTransform.forward, 40);
                break;
            
            case 2:
                _playerImpact.AddImpact(rootTransform.forward, 40);
                break;
        }
    }

    public void SetHandAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }
}
