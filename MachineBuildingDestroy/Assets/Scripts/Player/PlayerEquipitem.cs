using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class PlayerEquipitem : MonoBehaviourPun
{
    public GameObject _ItemObject;
    private PlayerState _playerState;
    public Transform _RFingerTransform;
    
    public bool BuffOn;
    public float buff_Time;
    public GameObject ItemObj;
    public GameObject Wall_Obstcle_Frame;
    public GameObject BuffObj;
    public GameObject HammerObj;
    public Rigidbody item_Rigid;
    public Quaternion parent_qut;
    
    private float timeBetHeal = 0.5f; // �� ����
    private float activeHealTime = 0f; // �� ���� �ð�
    private float LastHealTime = 0f; // ������ �������� �� ����

    void Start()
    {
        _playerState = GetComponent<PlayerState>();
        GameObject buffobj = Resources.Load<GameObject>("Buff_Effect");
        BuffObj = Instantiate(buffobj);
        BuffObj.transform.SetParent(gameObject.transform);
        Vector3 tpos = gameObject.transform.position + Vector3.up;
        BuffObj.transform.position = tpos;
        BuffObj.SetActive(false);
        buff_Time = 10f;
    }

    private void Update()
    {
        
    }
    
    public void Equip_item()
    {
        if (_playerState.Item == item_box_make.item_type.potion && _playerState.nowEquip == false)
        {
            //getobj = Resources.Load<GameObject>("potion");
            ItemObj = PhotonNetwork.Instantiate("potion", new Vector3(0, 0, 0), Quaternion.identity);
            ItemObj.transform.SetParent(gameObject.transform, true);
            Vector3 tpos = _RFingerTransform.transform.position;
            ItemObj.transform.Translate(tpos);
            item_Rigid = ItemObj.GetComponent<Rigidbody>();
            ItemObj.GetComponent<PotionState>().SetState("init");
            _playerState.nowEquip = true;
        }

        else if (_playerState.Item == item_box_make.item_type.obstacles && _playerState.nowEquip == false)
        {
            Wall_Obstcle_Frame = Resources.Load<GameObject>("Wall_Obstcle_Frame");
            ItemObj = Instantiate(Wall_Obstcle_Frame);
            ItemObj.transform.SetParent(gameObject.transform, true);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward * 5f) + Vector3.up;
            ItemObj.transform.Translate(tpos);
            Quaternion temp_Q = quaternion.identity;
            ItemObj.transform.localRotation = Quaternion.identity;
            _playerState.nowEquip = true;
        }
        
        else if (_playerState.Item == item_box_make.item_type.Hammer && _playerState.nowEquip == false)
        {
            HammerObj.SetActive(true);
            _playerState.nowEquip = true;
        }

        else if (_playerState.Item == item_box_make.item_type.Buff && BuffOn == false)
        {
            BuffOn = true;
        }
    }
    
    public void Throw_item()
    {
        if (ItemObj == null)
            return;
        if (_playerState.Item == item_box_make.item_type.potion)
        {
            ItemObj.transform.parent = null;
            ItemObj.GetComponent<PotionState>().SetState("throw");
            Vector3 throw_Angle;
            throw_Angle = gameObject.transform.forward * 10f;
            throw_Angle.y = 5f;
            item_Rigid.AddForce(throw_Angle, ForceMode.Impulse);
            _playerState.nowEquip = false;
            //던지고 나면 아이템 사라짐
            // _playerState.Item = item_box_make.item_type.no_item;
        }

        if (_playerState.Item == item_box_make.item_type.obstacles)
        {
            Quaternion old_rot = gameObject.transform.rotation;
            Debug.Log(old_rot);
            Destroy(ItemObj.gameObject);
            ItemObj.transform.parent = null;
            //getobj = Resources.Load<GameObject>("Wall_Obstcle_Objs");
            //ItemObj = Instantiate(getobj);
            Vector3 tpos = gameObject.transform.position + (gameObject.transform.forward * 5f);
            //ItemObj.transform.Translate(tpos);
            //ItemObj.transform.rotation = new Quaternion(old_rot.x, old_rot.y, old_rot.z, old_rot.w);
            ItemObj = PhotonNetwork.Instantiate("Wall_Obstcle_Objs", tpos,
                new Quaternion(old_rot.x, old_rot.y, old_rot.z, old_rot.w));
            ItemObj = null;
            _playerState.nowEquip = false;
            //던지고 나면 아이템 사라짐
            // _playerState.Item = item_box_make.item_type.no_item;
        }
    }

    public void Receive_Heal()
    {
        if (Time.time >= LastHealTime + timeBetHeal)
        {
            if (_playerState.health + 20 >= _playerState.startingHealth)
            {
                _playerState.RestoreHealth(20);
                LastHealTime = Time.time;
            }
            else
            {
                float remain_heal = 100 - _playerState.health;
                _playerState.RestoreHealth(remain_heal);
                LastHealTime = Time.time;
            }
        }
    }

    public void BuffCheck()
    {
        if (BuffOn)
        {
            _playerState.P_Dm.set_Ite(1.5f);
            buff_Time -= Time.deltaTime;
        }
        else
        {
            _playerState.P_Dm.set_Ite(1.0f);
        }

        if (buff_Time <= 0 && BuffOn == true)
        {
            BuffOn = false;
            buff_Time = 10f;
        }

        BuffObj.SetActive(BuffOn);
        //print(BuffOn);
    }
}
