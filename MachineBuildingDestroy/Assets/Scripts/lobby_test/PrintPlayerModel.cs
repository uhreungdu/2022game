using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PrintPlayerModel : MonoBehaviour
{
    public GameObject[] models = new GameObject[6];


    public void RenewPlayerModel(int num)
    {
        if (transform.childCount == 0)
        {
            var model = Instantiate(models[num]);
            model.transform.parent = transform;
            model.transform.position = transform.position;
            model.transform.localScale = new Vector3(350, 350, 350);
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
            var model = Instantiate(models[num]);
            model.transform.parent = transform;
            model.transform.position = transform.position;
            model.transform.localScale = new Vector3(350, 350, 350);
        }
    }

}
