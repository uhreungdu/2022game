using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Beacon_Control : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager Gmanager;
    public int min;
    public float sec;
    public GameObject box;
    public bool have_box;
    void Start()
    {
        Gmanager = GameManager.GetInstance();
        box.GetComponent<item_box_make>().decide_type();
    }

    // Update is called once per frame
    void Update()
    {
        box.GetComponent<item_box_make>().effect_On = true;
    }
}
