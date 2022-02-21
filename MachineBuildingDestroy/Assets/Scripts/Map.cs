using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private List<GameObject> Buildings = new List<GameObject>(); // 생성된 적들을 담는 리스트

    public GameObject planepref;
    public GameObject bulidingpref;

    private float x = 20;
    private float z = 20;

    // Start is called before the first frame update
    void Start()
    {
        //string path = Application.dataPath + "/Map.csv";
        //string[] map = System.IO.File.ReadAllLines(path);

        //if (map.Length > 0)
        //{

        //}
        float offset = 10;
        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < z; ++j)
            {
                Vector3 position = new Vector3((i * offset) - (offset * ((x - 1) / 2)), 0, (j * offset) - (offset * ((z - 1) / 2)));
                GameObject temp = Instantiate(planepref, position, planepref.transform.rotation);
                temp.transform.SetParent(this.transform);
            }
        }
        for (int i = 0; i < 100; ++i)
        {
            Vector3 position = Vector3.zero;
            position.x = Random.RandomRange(-offset * (x / 2) + offset, offset * (x / 2) - offset);
            position.y = Random.RandomRange(20, 50);
            position.z = Random.RandomRange(-offset * (z / 2) + offset, offset * (z / 2) - offset);

            GameObject buliding = Instantiate(bulidingpref, position, bulidingpref.transform.rotation);
            Buildings.Add(buliding);
            buliding.transform.SetParent(this.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
