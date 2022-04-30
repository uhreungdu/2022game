using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashAttack : PlayerAttack
{
    // Start is called before the first frame update
    public PlayerImpact _playerImpact;
    public BoxCollider _HandBoxCollider;
    public GameObject LHandGameObject;
    private Thirdpersonmove _thirdpersonmove;
    void Start()
    {
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _attackName = "기본공격";
        _lastColliderActiveTime = 0.4f; // 공격 유지 시간
        _aftercastAttack = 0.8f;
        SetAffterCast(0);
        _damage = 20;
        
        _hitBoxColliders.Add(_HandBoxCollider);
    }

    void Update()
    {
        HandTransform();
        AfterCastRecovery();
        ActiveLAttack();
    }
    
    public void ActiveLAttack()
    {
        if (_hitBoxColliders[0].enabled && ActiveColliderCheck())
        {
            return;
        }
        else if (_hitBoxColliders[0].enabled)
        {
            SetDashAttackCollision(0);
        }
    }

    public void SetDashAttackCollision(int set)
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
    
    public void DashAttackMovement()
    {
        Transform rootTransform = transform.root;
        _playerImpact.AddImpact(rootTransform.forward, 150);
    }
    
    public void SetDashAttackAffterCast(int set)
    {
        base.SetAffterCast(set);      // 애니메이션 이벤트에서 이래야 받음
    }
}
