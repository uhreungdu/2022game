using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public int Durability = 10;

    public PlayerState _PlayerState;

    public AudioClip HitClip;
    
    private AttackButton _attackButton;
    // Update is called once per frame
    void Start()
    {
        _PlayerState = transform.root.GetComponent<PlayerState>();
        HitClip = Resources.Load<AudioClip>("Sounds/Hammer");
        if (transform.root.GetComponent<PhotonView>().IsMine && Application.platform == RuntimePlatform.Android)
            _attackButton = GameObject.Find("AttackButton").GetComponent<AttackButton>();
    }
    
    private void Awake()
    {
        Durability = 10;
    }
    
    void Update()
    {
        if (Durability <= 0)
        {
            _PlayerState.nowEquip = false;
            _PlayerState.Item = item_box_make.item_type.no_item;
            if (transform.root.GetComponent<PhotonView>().IsMine && Application.platform == RuntimePlatform.Android)
            {
                // 공격버튼 원래대로 바꾸기
                _attackButton.ChangeButtonImage(item_box_make.item_type.no_item);
            }
            gameObject.SetActive(false);
        }
    }
}
