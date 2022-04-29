using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Text_Display : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public Text text_H;
    public Text text_Item;
    public item_box_make.item_type item_typ;
    void Start()
    {
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        text_H.text = "HP: "+gManager.player_stat.health;
        item_typ = gManager.player_stat.Item_num;
        switch (item_typ)
        {
            case item_box_make.item_type.no_item:
                text_Item.text = " ";
                break;
            case item_box_make.item_type.potion:
                text_Item.text = "Potion";
                break;
            case item_box_make.item_type.obstacles:
                text_Item.text = "obstacles";
                break;
            case item_box_make.item_type.Buff:
                text_Item.text = "Buff";
                break;
            case item_box_make.item_type.Hammer:
                text_Item.text = "Hammer";
                break;
            
                    
        }
    }
}
