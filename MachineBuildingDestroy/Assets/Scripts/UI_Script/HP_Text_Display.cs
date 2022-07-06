using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Text_Display : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public Text text_H;
    public Image Hp_Bar;
    public Material HP_BarMat;
    public Text Coin_text;
    public item_box_make.item_type item_typ;
    void Start()
    {
        gManager = GameManager.GetInstance();
        HP_BarMat = Hp_Bar.material;
    }

    // Update is called once per frame
    void Update()
    {
        text_H.text = "HP: "+gManager.player_stat.health;
        Hp_Bar.fillAmount = gManager.player_stat.health / gManager.player_stat.MaxHealth;
        item_typ = gManager.player_stat.Item_num;
        Coin_text.text = ""+gManager.player_stat.Coin_Count;
        Glitch_Effect_On(gManager.player_stat.UIGiltch);
    }

    void Glitch_Effect_On(bool onoffswitch)
    {
        if (onoffswitch)
        {
            HP_BarMat.SetFloat("_NoiseScale",250f);
            HP_BarMat.SetFloat("_Timenoise",10f);
        }
        else
        {
            HP_BarMat.SetFloat("_NoiseScale",0f);
            HP_BarMat.SetFloat("_Timenoise",0f);
        }
    }
}
