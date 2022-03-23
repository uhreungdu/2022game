using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Photon.Pun;

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
        public List<Tile> Tiles = new List<Tile>();        // json���� ���� ���� �ļ�
        public void Print()
        {
            for (int i = 0; i < Tiles.Count; ++i)
            {
                //Debug.Log("kind = " + Tiles[i].kind);
                //Debug.Log("position = " + Tiles[i].position);
            }
        }
    }

    private Maptile maptile = new Maptile();
    private List<GameObject> Buildings = new List<GameObject>();
    private List<GameObject> Planes = new List<GameObject>();

    public GameObject planepref;
    public GameObject goalpostpref;
    public GameObject itempref;
    public GameObject tankpref;
    public GameObject arcadepref;

    private float x = 20;
    private float z = 20;
    
    [Tooltip("True = 온라인모드, False = 로컬모드")]
    public bool Online = false;


    // Start is called before the first frame update
    void Start()
    {
        //string path = Application.dataPath + "/Maps" + "/Map.csv";
        //string[] map = System.IO.File.ReadAllLines(path);

        //if (map.Length > 0)
        //{

        //}
        RandomMap();
        string jsonData = ObjectToJson(maptile);
        Debug.Log(jsonData);
        CreateJsonFile(Application.dataPath, "maptileClass", jsonData);
        
        var jtc2 = LoadJsonFile<Maptile>(Application.dataPath, "maptileClass");
        maptile = jtc2;
        // jtc2.Print();

        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = null;
            switch (maptile.Tiles[i].kind)
            {
                case 0:
                    tilepref = planepref;
                    break;
                case 1:
                    tilepref = goalpostpref;
                    break;
                case 2:
                    tilepref = itempref;
                    break;
                case 3:
                    tilepref = tankpref;
                    break;
                case 4:
                    tilepref = arcadepref;
                    break;
            }
            GameObject temp = Instantiate(tilepref, maptile.Tiles[i].position, tilepref.transform.rotation);
            temp.name = tilepref.name + i;
            temp.transform.SetParent(this.transform);
        }

        //string jsonData = ObjectToJson(maptile);
        
        //Debug.Log(jsonData);
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
            position.z = Random.RandomRange(-offset * (z / 2) + offset, offset * (z / 2) - offset);
            
            Tile tile = new Tile();
            tile.kind = Random.Range(2, 4 + 1);
            if (tile.kind == 2)
            {
                position.y = 0.15f;
            }
            else
                position.y = Random.RandomRange(20, 50);

            tile.position = position;
            maptile.Tiles.Add(tile);
            
            // GameObject tilepref = null;
            // switch (Random.Range(2, 3 + 1))
            // {
            //     case 0:
            //         tilepref = planepref;
            //         break;
            //     case 1:
            //         tilepref = goalpostpref;
            //         break;
            //     case 2:
            //         tilepref = tankpref;
            //         break;
            //     case 3:
            //         tilepref = arcadepref;
            //         break;
            // }

            // GameObject buliding = Instantiate(tilepref, position, bulidingpref.transform.rotation);
            // Buildings.Add(buliding);
            // buliding.transform.SetParent(this.transform);

        }
    }

    public void CreateNetworkMap()
    {
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
                    tilepref = goalpostpref;
                    break;
                case 2:
                    tilepref = itempref;
                    break;
                case 3:
                    tilepref = tankpref;
                    break;
                case 4:
                    tilepref = arcadepref;
                    break;
            }
            GameObject temp = PhotonNetwork.InstantiateRoomObject(tilepref.name, maptile.Tiles[i].position, tilepref.transform.rotation);
            temp.name = tilepref.name + i;
            temp.transform.SetParent(this.transform);
        }

        string jsonData = ObjectToJson(maptile);
    }
}