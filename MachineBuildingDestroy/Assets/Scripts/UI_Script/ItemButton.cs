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
    }
    
    public void ButtonDown()
    {
        isPressed = true;
    }
    
    private void FixedUpdate()
    {
        var item_type = gManager.player_stat.Item_num;
        itemImage.sprite = itemImages[(int)item_type];
       
    }
}
