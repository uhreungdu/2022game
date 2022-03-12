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
            int rand = Random.Range(0, 3);
            CreateItem(rand);
        }
        Debug.Log(Gmanager.EManager.itembox_Create);
    }

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

    public void OnEvent(EventData Evdata)
    {
        byte eventCode = Evdata.Code;
        Debug.Log("EVENTCALLITEM");
        // 아이템 생성
        if (eventCode == 2)
        {
            object[] data = (object[])Evdata.CustomData;
            if ((bool)data[1])
            {
                CreateItem((int)data[0]);
            }
        }
    }

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
}
