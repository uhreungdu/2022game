using System;
using UnityEngine;
using Photon.Pun;

public class Dmgs_Status
{
    // Start is called before the first frame update
    public float Atk_Stats { get; private set; }
    public float Weapon_Stat { get; private set; }
    public float Item_Coefficient { get; private set; }

    public void Set_St(float atk, float Wea, float Itme)
    {
        Atk_Stats = atk;
        Weapon_Stat = Wea;
        Item_Coefficient = Itme;
    }
    public float Damge_formula()
    {
        float temp = (Atk_Stats + Weapon_Stat) * Item_Coefficient;
        return temp;
    }
    public void set_Atk(float atk)
    {
        Atk_Stats = atk;
    }
    public void set_Wea(float wea)
    {
        Weapon_Stat = wea;
    }
    public void set_Ite(float ite)
    {
        Item_Coefficient = ite;
    }
    
}
