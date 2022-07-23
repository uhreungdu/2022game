using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public bool isPressed;
    
    public Image attackImage;
    public Sprite[] itemImages;
    
    private void Awake()
    {
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

    public void ChangeButtonImage(item_box_make.item_type value)
    {
        attackImage.sprite = itemImages[(int)value];
    }
}
