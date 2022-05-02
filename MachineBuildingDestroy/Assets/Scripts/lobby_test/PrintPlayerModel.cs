using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PrintPlayerModel : MonoBehaviour
{
    public GameObject[] models = new GameObject[6];
    public int nowModelNum = 0;

    public void RenewPlayerModel(int num)
    {
        nowModelNum = num;
        if (transform.childCount == 0)
        {
            var model = Instantiate(models[nowModelNum]);
            model.transform.parent = transform;
            model.transform.position = transform.position;
            
        }
    }

    private void Awake()
    {
        RenewPlayerModel(0);
    }
}
