using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class Item_Beacon_Control : MonoBehaviourPun
{
    // Start is called before the first frame update
    public GameManager Gmanager;
    public int min;
    public int past_min;
    public float sec;
    public GameObject box;
    public GameObject box_obj;
    public bool have_box;
    [SerializeField]
    private int CreateTime = 10;
    void Start()
    {
        Gmanager = GameManager.GetInstance();
        //box_obj.GetComponent<item_box_make>().effect_On = true;
        have_box = true;
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
            // Master Client가 아니면 아무것도 하지않습니다.
            if (!PhotonNetwork.IsMasterClient) return;
            
            // Master Client라면 CreateItem(int)함수를 자신 포함 방 안에있는 모두에게 실행하라고 명령합니다.
            int rand = Random.Range(0, 3);
            photonView.RPC("CreateItem", RpcTarget.All, rand);
            //CreateItem(rand);
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
        box_obj.GetComponent<item_box_make>().decide_type(type);
        have_box = false;
        past_min = Gmanager.now_timer.min;
    }
}
