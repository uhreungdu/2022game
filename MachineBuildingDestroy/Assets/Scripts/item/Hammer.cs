using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public int Durability = 10;

    public PlayerState _PlayerState;
    // Update is called once per frame
    void Update()
    {
        if (Durability <= 0)
        {
            gameObject.SetActive(false);
            _PlayerState.nowEquip = false;
            _PlayerState.Item = item_box_make.item_type.no_item;
        }
    }
}
