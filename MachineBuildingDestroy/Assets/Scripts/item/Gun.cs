using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public int Durability = 50;

    public PlayerState _PlayerState;
    // Update is called once per frame

    void Start()
    {
        _PlayerState = transform.root.GetComponent<PlayerState>();
    }
    void Update()
    {
        if (Durability <= 0)
        {
            _PlayerState.nowEquip = false;
            _PlayerState.Item = item_box_make.item_type.no_item;
            gameObject.SetActive(false);
        }
    }
}