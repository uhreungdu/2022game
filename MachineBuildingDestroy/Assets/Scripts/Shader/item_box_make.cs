using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class item_box_make : MonoBehaviour
{
    //아이템 타입을 나타내는 enum
    //0:무기, 1:투척, 2:소모, 3:설치
    // 에너지파 추가
    // UI 추가바람
    public enum item_type{
        no_item,
        Buff,
        potion,
        obstacles,
        Hammer,
        EnergyWave,
        Gun
    }
    
    public item_type now_type {get; private set;}
    public bool effect_On = true;
    // Start is called before the first frame update
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;
    public float Time_temp;
    public bool effect_switch;
    public float move_height;
    private Material material;
    public GameManager GManager;
    public GameObject Spark;
    public GameObject inbox;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    void Start()
    {
        effect_switch = false;
        GManager = GameManager.GetInstance();
        move_height = -1f;
        SetHeight(move_height);
        Spark.SetActive(false);
        inbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(effect_On == true)
        {
            if(GManager.EManager.itembox_Create == false)
            {
                Spark.SetActive(false);
                inbox.SetActive(false);
                effect_switch = false;
                move_height -= Time.deltaTime * 2;
                // Debug.Log(move_height);
                SetHeight(move_height);
                if (move_height <= -1f)
                {
                    if (PhotonNetwork.IsMasterClient)
                        PhotonNetwork.Destroy(gameObject);
                }
            }
            else
            {
                if(move_height <= 1.5f)
                {
                    move_height += Time.deltaTime * 2;
                    SetHeight(move_height);
                }
                else
                {
                    Spark.SetActive(true);
                    inbox.SetActive(true);
                    effect_switch = true;
                }
            }
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        }
    }
    
    // 2022-04-23 변경
    // 양현석
    // 내부 변경
    // 스위칭 없애고 똑같은 코드로 돌아가도록 수정
    // 봤으면 지우기
    
    public void decide_type(int type)
    {
        now_type = (item_type)type;
    }
    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
             if(effect_switch == true)
             {
                 //if (PhotonNetwork.IsMasterClient)
                 {
                     //PhotonNetwork.Destroy(gameObject);
                 }
                
                 Destroy(gameObject);
             }
         }
    }
    public void SetHeight(float height)
    {
        material.SetFloat("CutoffHeight",height);
        material.SetFloat("Nosie_strength",noiseStrength);
    }

}
