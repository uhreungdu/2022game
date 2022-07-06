using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_image_Display : MonoBehaviour
{
    public GameManager gManager;

    public Image item_icon;

    public Sprite image;

    public Sprite HammerImage;
    public Sprite PotionImage;
    public Sprite BuffImage;
    public Sprite obstacles;
    public Sprite GunImage;
    public item_box_make.item_type item_typ;
    // Start is called before the first frame update
    void Start()
    {
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        item_typ = gManager.player_stat.Item_num;
        switch (item_typ)
        {
            case item_box_make.item_type.no_item:
                item_icon.sprite = image;
                break;
            case item_box_make.item_type.potion:
                item_icon.sprite = PotionImage;
                break;
            case item_box_make.item_type.obstacles:
                item_icon.sprite = obstacles;
                break;
            case item_box_make.item_type.Buff:
                item_icon.sprite = BuffImage;
                break;
            case item_box_make.item_type.Hammer:
                item_icon.sprite = HammerImage;
                break;
            case item_box_make.item_type.Gun:
                item_icon.sprite = GunImage;
                break;
                    
        }
    }
}
