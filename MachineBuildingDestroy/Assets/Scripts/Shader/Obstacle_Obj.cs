using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_Obj : LivingEntity
{
    // Start is called before the first frame update
    public List<GameObject> obstacles_list;
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            obstacles_list.Add(gameObject.transform.GetChild(i).gameObject);
            obstacles_list[i].GetComponent<Dissolve_Control>().dissolve_switch = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Destory_Obs()
    {
        for (int i = 0; i < obstacles_list.Count; i++)
        {
            if (obstacles_list[i] != null)
            {
                obstacles_list[i].GetComponent<Dissolve_Control>().dissolve_switch = false;
            }
        }
    }
}
