using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_box_make : MonoBehaviour
{
    //아이템 타입을 나타내는 enum
    //0:무기, 1:투척, 2:소모, 3:설치
    public enum item_type{
        weapon,splash,potion,obstacles
    }
    public item_type now_type {get; private set;}
    public bool effect_On = false;
    // Start is called before the first frame update
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;
    public float Time_temp;
    public bool effect_switch;
    public float move_height;
    private Material material;
    public GameManager GManager;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(effect_On == true)
        {
            if(GManager.EManager.itembox_Create == false)
            {
                effect_switch = false;
                move_height -= Time.deltaTime * 2;
                Debug.Log(move_height);
                SetHeight(move_height);
                if (move_height <= -1f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                if(move_height <= 1.5)
                {
                    move_height += Time.deltaTime * 2;
                    SetHeight(move_height);
                }
                else
                {
                    effect_switch = true;
                }
            }
            transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        }
    }
    public void decide_type(int type){
        switch(type){
            case 0:
                now_type = item_type.weapon;
                break;
            case 1:
                now_type = item_type.splash;
                break;
            case 2:
                now_type = item_type.potion;
                break;
            case 3:
                now_type = item_type.obstacles;
                break;
        }
    }
    void OnTriggerEnter(Collider col){
        if(col.tag == "Player"){
             if(effect_switch == true)
             {
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
