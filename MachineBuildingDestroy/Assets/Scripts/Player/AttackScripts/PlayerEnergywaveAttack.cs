using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergywaveAttack : PlayerAttack
{
    // Start is called before the first frame update
    public PlayerImpact _playerImpact;
    public BoxCollider _rHandBoxCollider;
    public GameObject LHandGameObject;
    public GameObject RHandGameObject;
    private Thirdpersonmove _thirdpersonmove;
    void Start()
    {
        base.Start();
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _thirdpersonmove = GetComponent<Thirdpersonmove>();
        _attackName = "기본공격";
        _lastColliderActiveTime = 0.2f; // 공격 유지 시간
        _aftercastAttack = 1.0f;
        SetAffterCast(0);
        _damage = 25;
        
        _hitBoxColliders.Add(_rHandBoxCollider);
    }

    void Update()
    {
        HandTransform();
        AfterCastRecovery();
        ActiveAttack();
    }
    public void ActiveAttack()
    {
        if (_hitBoxColliders[0].enabled && ActiveColliderCheck() && !_playerState.IsCrowdControl() && !_thirdpersonmove.IsGrounded())
        {
            return;
        }
        else if (_hitBoxColliders[0].enabled)
        {
            _hitBoxColliders[0].enabled = false;
        }
    }
    
    public void SetHandCollision(int set)
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
        Vector3 WorldRHandPosition = RHandGameObject.transform.position;
        _rHandBoxCollider.transform.position = WorldRHandPosition;
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
}
