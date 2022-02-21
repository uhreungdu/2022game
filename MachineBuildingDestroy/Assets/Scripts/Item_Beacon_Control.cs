using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Beacon_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager Gmanager;
    public int min;
    public int past_min;
    public float sec;
    public GameObject box;
    public GameObject box_obj;
    public bool have_box;
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
            if(Gmanager.now_timer.sec >= 30)
            {
                box_obj.GetComponent<item_box_make>().effect_On = true;
                past_min = Gmanager.now_timer.min;
            }
            
        }
        else if(box_obj == null && Gmanager.now_timer.sec >= 30)
        {
            if(have_box == true)
            {
                box = Resources.Load<GameObject>("item_box");
                box_obj = Instantiate(box);
                box_obj.transform.SetParent(gameObject.transform);
                box_obj.transform.Translate(gameObject.transform.position);
                box_obj.GetComponent<item_box_make>().decide_type();
                have_box = false;
                past_min = Gmanager.now_timer.min;
            }
            else
            {
                if(past_min != Gmanager.now_timer.min)
                {
                    have_box = true;
                }
            }
        }
        else
        {
            
        }
    }
}
