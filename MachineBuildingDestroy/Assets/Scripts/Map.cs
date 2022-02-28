using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Map : MonoBehaviour
{
    [System.Serializable]
    public class Tile
    {
        public int kind;
        public Vector3 position;
    }
    public class Maptile
    {
        public List<Tile> Tiles = new List<Tile>();        // jsonÀ¸·Î ¹­±â À§ÇÑ ²Ä¼ö
        public void Print()
        {
            for (int i = 0; i < Tiles.Count; ++i)
            {
                Debug.Log("kind = " + Tiles[i].kind);
                Debug.Log("position = " + Tiles[i].position);
            }
        }
    }

    private Maptile maptile = new Maptile();
    private List<GameObject> Buildings = new List<GameObject>();
    private List<GameObject> Planes = new List<GameObject>();

    public GameObject planepref;
    public GameObject bulidingpref;

    private float x = 20;
    private float z = 20;


    // Start is called before the first frame update
    void Start()
    {
        //string path = Application.dataPath + "/Maps" + "/Map.csv";
        //string[] map = System.IO.File.ReadAllLines(path);

        //if (map.Length > 0)
        //{

        //}
        var jtc2 = LoadJsonFile<Maptile>(Application.dataPath, "maptileClass");
        maptile = jtc2;
        jtc2.Print();

        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = null;
            switch (maptile.Tiles[i].kind)
            {
                case 0:
                    tilepref = planepref;
                    break;
                case 1:
                    tilepref = bulidingpref;
                    break;
                case 2:
                    tilepref = bulidingpref;
                    break;
                case 3:
                    tilepref = bulidingpref;
                    break;
            }
            GameObject temp = Instantiate(tilepref, maptile.Tiles[i].position, tilepref.transform.rotation);
            temp.transform.SetParent(this.transform);
        }

        string jsonData = ObjectToJson(maptile);
        Debug.Log(jsonData);
        //CreateJsonFile(Application.dataPath, "maptileClass", jsonData);

    }

    // Update is called once per frame
    void Update()
    {

    }

    string ObjectToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }
    T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    void RandomMap()
    {
        float offset = 10;
        for (int i = 0; i < x; ++i)
        {
            for (int j = 0; j < z; ++j)
            {
                Vector3 position = new Vector3((i * offset) - (offset * ((x - 1) / 2)), 0, (j * offset) - (offset * ((z - 1) / 2)));
                GameObject temp = Instantiate(planepref, position, planepref.transform.rotation);
                temp.transform.SetParent(this.transform);
                Tile tile = new Tile();
                tile.kind = 0;
                tile.position = position;
                maptile.Tiles.Add(tile);
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

            Tile tile = new Tile();
            tile.kind = 1;
            tile.position = position;
            maptile.Tiles.Add(tile);
        }
    }
}
