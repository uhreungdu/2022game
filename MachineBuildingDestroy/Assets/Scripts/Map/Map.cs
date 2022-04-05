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
        public Quaternion rotate;
    }
    public class Maptile
    {
        public List<Tile> Tiles = new List<Tile>();        // json���� ���� ���� �ļ�
        public void Print()
        {
            for (int i = 0; i < Tiles.Count; ++i)
            {
                Debug.Log("kind = " + Tiles[i].kind);
                Debug.Log("position = " + Tiles[i].position);
            }
        }
    }

    enum tileKind
    {
        plane,
        goalpost,
        item,
        tank,
        arcade,
        team1Spawner,
        team2Spawner
    };

    public Maptile maptile = new Maptile();
    private List<GameObject> Buildings = new List<GameObject>();
    private List<GameObject> Planes = new List<GameObject>();

    public GameObject mapGameObject;

    public GameObject planepref;
    public GameObject goalpostpref;
    public GameObject itempref;
    public GameObject tankpref;
    public GameObject arcadepref;
    public GameObject team1Spawner;
    public GameObject team2Spawner;

    private float x = 20;
    private float z = 20;
    
    [Tooltip("True = 온라인모드, False = 로컬모드")]
    public bool Online = false;
    [Tooltip("True = 맵에디터모드, False = 플레이모드")]
    public bool MapEditer = false;



    // Start is called before the first frame update
    void Start()
    {
        //string path = Application.dataPath + "/Maps" + "/Map.csv";
        //string[] map = System.IO.File.ReadAllLines(path);

        //if (map.Length > 0)
        //{

        // //}
        // string jsonData = ObjectToJson(maptile);
        // Debug.Log(jsonData);
        // CreateJsonFile(Application.dataPath, "maptileClass", jsonData);
        if (!MapEditer)
        {
            if (Online)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    CreateNetworkMap();
                    GameObject.Find("NetworkManager").GetComponent<NetworkManager>().SpawnPlayerEvent();
                }
            }
            else
                MapLoad();

            
        }
        // localMAP not USE NetworkGame
        /*
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
        */
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
                // GameObject temp = Instantiate(planepref, position, planepref.transform.rotation);
                // temp.transform.SetParent(this.transform);
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

    public void MapSave()
    {
        string jsonData = ObjectToJson(maptile);
        Debug.Log(jsonData);
        CreateJsonFile(Application.dataPath, "maptileClass", jsonData);
    }

    public GameObject SetTilepref(int kind)
    {
        GameObject obj;
        switch (kind)
        {
            case 0:
                return planepref;
            case 1:
                return goalpostpref;
            case 2:
                return itempref;
            case 3:
                return tankpref;
            case 4:
                return arcadepref;
            case 5:
                return team1Spawner;
            case 6:
                return team2Spawner;
        }

        return null;
    }

    public void MapLoad()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (var child in allChildren)
        {
            if (child.gameObject != gameObject)
            {
                Destroy(child.gameObject);
            }
        }
        if (maptile.Tiles.Count >= 1)
            maptile.Tiles.Clear();
        var jtc2 = LoadJsonFile<Maptile>(Application.dataPath, "maptileClass");
        maptile = jtc2;

        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = SetTilepref(maptile.Tiles[i].kind);
            GameObject temp = Instantiate(tilepref, maptile.Tiles[i].position, maptile.Tiles[i].rotate);
            temp.name = tilepref.name + i;
            temp.transform.parent = transform;
        }

    }

    public void CreateNetworkMap()
    {
        var jtc2 = LoadJsonFile<Maptile>(Application.dataPath, "maptileClass");
        maptile = jtc2;
        jtc2.Print();

        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = SetTilepref(maptile.Tiles[i].kind);
            GameObject temp = PhotonNetwork.InstantiateRoomObject(tilepref.name, maptile.Tiles[i].position, maptile.Tiles[i].rotate);
        }
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(maptile);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            maptile = (Maptile)stream.ReceiveNext();
        }
    }
}
