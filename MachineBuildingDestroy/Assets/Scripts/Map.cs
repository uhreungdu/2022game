using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private List<GameObject> walls = new List<GameObject>(); // 생성된 적들을 담는 리스트

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
        //offset = plane.GetComponent<RectTransform>().rect.width;
        float offset = 10;
        Vector3 position = Vector3.zero;
        GameObject temp = Instantiate(planepref, position, planepref.transform.rotation);
        temp.transform.localScale += new Vector3(x - 1, 0, z - 1);
        for (int i = 0; i < 100; ++i)
        {
            position.x = Random.RandomRange(-offset * (x / 2) + offset, offset * (x / 2) - offset);
            position.y = Random.RandomRange(20, 50);
            position.z = Random.RandomRange(-offset * (z / 2) + offset, offset * (z / 2) - offset);

            GameObject wall = Instantiate(bulidingpref, position, bulidingpref.transform.rotation);
            walls.Add(wall);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
