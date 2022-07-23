using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public GameManager gManager;
    public bool isPressed;

    public Image itemImage;
    public Sprite[] itemImages;

    public GameObject attackButton;

    private void Awake()
    {
        gManager=GameManager.GetInstance();
        if (Application.platform != RuntimePlatform.Android)
        {
            gameObject.SetActive(false);
        }
    }

    public void ButtonUp()
    {
        isPressed = false;
        GetComponent<Button>().interactable = false;
    }
    
    public void ButtonDown()
    {
        var item_type = gManager.player_stat.Item_num;
        switch (item_type)
        {
            case item_box_make.item_type.obstacles:
            case item_box_make.item_type.potion:
            case item_box_make.item_type.Gun:
            case item_box_make.item_type.Hammer:
                attackButton.GetComponent<AttackButton>().ChangeButtonImage(item_type);
                break;
            default: 
                break;
        }
        isPressed = true;
    }
    
    private void FixedUpdate()
    {
        var item_type = gManager.player_stat.Item_num;
        itemImage.sprite = itemImages[(int)item_type];
       
    }
}
