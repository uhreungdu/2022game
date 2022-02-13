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
    void Start()
    {
        effect_On = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(effect_On == true)
        {
            //decide_type();
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
            Destroy(gameObject);
        }
    }

}
