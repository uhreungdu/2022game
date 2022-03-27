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
        if(box_obj != null)
        {
            box_obj.GetComponent<item_box_make>().effect_On = true;
        }
        else if(box_obj == null && Gmanager.EManager.itembox_Create == true)
        {
            // Master Client�� �ƴϸ� �ƹ��͵� �����ʽ��ϴ�.
            if (!PhotonNetwork.IsMasterClient) return;

            if (Item_get == true)
            {
                // Master Client��� CreateItem(int)�Լ��� �ڽ� ���� �� �ȿ��ִ� ��ο��� �����϶�� ����մϴ�.
                int rand = Random.Range(0, 3 + 1);
                photonView.RPC("CreateItem", RpcTarget.All, rand);
                //CreateItem(rand);
            }
        }

        if (Gmanager.EManager.itembox_Create == false)
        {
            Item_get = true;
        }
        //Debug.Log(Gmanager.EManager.itembox_Create);
    }
    
    [PunRPC]
    void CreateItem(int type)
    {
        box = Resources.Load<GameObject>("item_box");
        box_obj = Instantiate(box);
        box_obj.transform.SetParent(gameObject.transform);
        box_obj.transform.Translate(gameObject.transform.position);
        box_obj.GetComponent<item_box_make>().decide_type(3);
        Item_get = false;
    }
}
