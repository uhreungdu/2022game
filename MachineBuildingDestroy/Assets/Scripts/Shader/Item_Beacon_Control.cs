using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Item_Beacon_Control : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameManager Gmanager;
    public GameObject box;
    public GameObject box_obj;
    public bool Item_get;
    [SerializeField]
    private int CreateTime = 10;
    void Start()
    {
        Gmanager = GameManager.GetInstance();
        //box_obj.GetComponent<item_box_make>().effect_On = true;
        Item_get = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if(box_obj == null && Gmanager.EManager.itembox_Create == true)
        {
            if (Item_get == true)
            {
                int itemkindLength = System.Enum.GetValues(typeof(item_box_make.item_type)).Length;
                int rand = Random.Range(1, itemkindLength);
                rand = 5;
                photonView.RPC("CreateItem", RpcTarget.MasterClient, rand);
            }
        }

        if (Gmanager.EManager.itembox_Create == false)
        {
            Item_get = true;
        }
    }
    
    [PunRPC]
    void CreateItem(int type)
    {
        box = Resources.Load<GameObject>("item_box");
        box_obj = PhotonNetwork.InstantiateRoomObject(box.name, 
            gameObject.transform.position+new Vector3(0,0.7f,0), 
            new Quaternion(0, 0, 0, 0));
        box_obj.transform.SetParent(gameObject.transform);
        //box_obj.transform.Translate(gameObject.transform.position);
        box_obj.GetComponent<item_box_make>().decide_type(type);
        Item_get = false;
    }
}
