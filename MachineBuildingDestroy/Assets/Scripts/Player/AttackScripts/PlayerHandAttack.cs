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
    private BoxCollider _lHandBoxCollider;
    private BoxCollider _rHandBoxCollider;
    void Start()
    {
        _playerState = transform.GetComponent<PlayerState>();
        _playerImpact = transform.GetComponent<PlayerImpact>();
        _attackName = "기본공격";
        _timeBetAttack = 0.3f; // 공격 간격
        _activeAttackTime = 0f; // 공격 유지 시간
        _lastAttackTime = 0f; // 공격을 마지막에 한 시점
        SetKeepActiveAttack(0);
        _damage = 20;
        
        _lHandBoxCollider = GameObject.Find("Bip001 L Hand").GetComponent<BoxCollider>();
        _hitBoxColliders.Add(_lHandBoxCollider);
        _rHandBoxCollider = GameObject.Find("Bip001 R Hand").GetComponent<BoxCollider>();
        _hitBoxColliders.Add(_rHandBoxCollider);
    }
    
    public void SetLHandCollision(int set)
    {
        if (set > 0)
            _hitBoxColliders[0].enabled = true;
        else if (set <= 0)
            _hitBoxColliders[0].enabled = false;
    }
    
    public void SetRHandCollision(int set)
    {
        if (set > 0)
            _hitBoxColliders[1].enabled = true;
        else if (set <= 0)
            _hitBoxColliders[1].enabled = false;
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
}
