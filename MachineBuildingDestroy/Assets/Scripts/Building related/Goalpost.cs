using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalpost : MonoBehaviourPun
{
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (box_obj == null && Gmanager.EManager.itembox_Create == true)
        {
            if (Item_get == true)
            {
                int itemkindLength = System.Enum.GetValues(typeof(item_box_make.item_type)).Length;
                int rand = Random.Range(1, itemkindLength);
                rand = 5;
                photonView.RPC("CreateItem", RpcTarget.MasterClient, rand);
            }
        }
    }

    if (Gmanager.EManager.itembox_Create == false)
{
    Item_get = true;
}
    private void OnTriggerEnter(Collider other)
    {

    }
}
