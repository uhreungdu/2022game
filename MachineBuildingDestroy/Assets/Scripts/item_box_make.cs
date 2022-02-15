using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_box_make : MonoBehaviour
{
    //아이템 타입을 나타내는 enum
    //1:무기, 2:투척, 3:소모, 4:설치
    public enum item_type{
        weapon,splash,potion,obstacles
    }
    public item_type now_type;
    public bool effect_On = false;
    // Start is called before the first frame update
    [SerializeField] private float noiseStrength = 0.25f;
    [SerializeField] private float objectHeight = 1.0f;
    public float Time_temp;
    public bool effect_switch;
    public float move_height;
    private Material material;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    void Start()
    {
        effect_switch = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(effect_On == true)
        {
            if(move_height <= 2)
            {
                if(Mathf.Sin(Time_temp) >= 0.99f)
                {
                    move_height += 2;
                    effect_switch = true;
                }
                else
                {
                    Time_temp += Time.deltaTime * Mathf.PI * 0.5f;
                    move_height += Mathf.Sin(Time_temp)* 2;
                    SetHeight(move_height);
                    move_height = 0;
                }
            }
            transform.Rotate(Vector3.up * 20 * Time.deltaTime);
        }
    }
    public void decide_type(){
        int type_rand = Random.Range(0,3);
        switch(type_rand){
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
