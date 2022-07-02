using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public int Durability = 10;

    public PlayerState _PlayerState;
    // Update is called once per frame
    void Start()
    {
        _PlayerState = transform.root.GetComponent<PlayerState>();
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
            gameObject.SetActive(false);
        }
    }
}
