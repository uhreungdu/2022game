using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Photon.Pun;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    public List<Maptile> MapList;
    private List<GameObject> Buildings = new List<GameObject>();
    private List<GameObject> Planes = new List<GameObject>();
    public InputField FileNameInput;

    public float MaximumX = -Int32.MaxValue;
    public float MaximumZ = -Int32.MaxValue;
    public float MinimumX = Int32.MaxValue;
    public float MinimumZ = Int32.MaxValue;

    public GameObject[] Prefs;

    private float x = 20;
    private float z = 20;
    
    [Tooltip("True = 온라인모드, False = 로컬모드")]
    public bool Online = false;
    [Tooltip("True = 맵에디터모드, False = 플레이모드")]
    public bool Other = false;



    // Start is called before the first frame update
    void Start()
    {
        if (!Other)
        {
            if (Online)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    CreateNetworkMap();
                    MeasurementMap();
                    GameObject.Find("NetworkManager").GetComponent<NetworkManager>().SpawnPlayerEvent();
                }
            }
            else
            {
                MapLoad("1");
            }
        }
        else
        {
            MapList = LoadListJsonFile<Maptile>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Other)
        {
            GameObject g = GameObject.Find("GameManager");
            if (g != null)
                Destroy(g);
        }
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
    
    static T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = null;
        string jsonData;
        try
        {
            fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            jsonData = Encoding.UTF8.GetString(data);
            return JsonUtility.FromJson<T>(jsonData);
        }
        catch (FileNotFoundException ioEx)
        {
            print(ioEx.Message);
            throw new FileNotFoundException(@"없는 파일을 읽으려 했습니다. ",ioEx);
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    public static List<T> LoadListJsonFile<T>()
    {
        String FolderName = Application.dataPath + "/" + "Map";
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
        List<T> maptiles = new List<T>();
        if (di.Exists)
        {
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                string extension = ".json";
                if (File.Extension.ToLower().CompareTo(extension) == 0)
                {
                    String FileNameOnly = File.Name.Substring(0, File.Name.Length - extension.Length);
                    String FullFileName = File.FullName;
                    String loadPath = FolderName;
                    T loadJsonFile = LoadJsonFile<T>(loadPath, FileNameOnly);
                    maptiles.Add(loadJsonFile);
                    print(FullFileName + " " + FileNameOnly);
                }
            }
        }
        return maptiles;
    }
    
    public static List<string> LoadNameFile()
    {
        String FolderName = Application.dataPath + "/" + "Map";
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
        List<string> NameList = new List<string>();
        if (di.Exists)
        {
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                string extension = ".json";
                if (File.Extension.ToLower().CompareTo(extension) == 0)
                {
                    String FileNameOnly = File.Name.Substring(0, File.Name.Length - extension.Length);
                    String FullFileName = File.FullName;
                    String loadPath = FolderName;
                    NameList.Add(FileNameOnly);
                    print(FullFileName + " " + FileNameOnly);
                }
            }
        }
        return NameList;
    }
    public void LoadMapFile(string loadPath, string fileName)
    {
        maptile = LoadJsonFile<Maptile>(loadPath, fileName);
    }

    public void LoadMapList()
    {
        MapList = LoadListJsonFile<Maptile>();
    }
    
    public static List<Maptile> ReturnLoadMapList()
    {
        List<Maptile> maptiles = LoadListJsonFile<Maptile>();
        return maptiles;
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
        }
    }

    public void MapSave()
    {
        string jsonData = ObjectToJson(maptile);
        string mapname = null;
        if (FileNameInput.text != null)
        {
            mapname = FileNameInput.text;
        }
        else
        {
            FileNameInput.text = "SampleMap";
        }
        LoadMapList();
        foreach (var maptile in MapList)
        {
            
        }
        Debug.Log(jsonData);
        CreateJsonFile(Application.dataPath + "/" + "Map", mapname, jsonData);
    }

    public GameObject SetTilepref(int kind)
    {
        GameObject obj;
        if (kind == -1)
            return null;
        return Prefs[kind];
    }

    public void MapLoad(string loadPath = "maptileClass")
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
        var jtc2 = LoadJsonFile<Maptile>(Application.dataPath + "/" + "Map", loadPath);
        maptile = jtc2;

        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = SetTilepref(maptile.Tiles[i].kind);
            GameObject temp = Instantiate(tilepref, maptile.Tiles[i].position, maptile.Tiles[i].rotate);
            if (Other)
            {
                temp.transform.parent = transform;
                Rigidbody temprigidbody = temp.GetComponent<Rigidbody>();
                if (temprigidbody != null)
                    temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Destroy(temp.GetComponentInChildren<BulidingObject>());
                Destroy(temp.GetComponentInChildren<LandMarkObject>());
            }
        }
    }

    public void ReLoadMap()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (var child in allChildren)
        {
            if (child.gameObject != gameObject)
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = SetTilepref(maptile.Tiles[i].kind);
            GameObject temp = Instantiate(tilepref, maptile.Tiles[i].position, maptile.Tiles[i].rotate);
            if (Other)
            {
                temp.transform.parent = transform;
                Rigidbody temprigidbody = temp.GetComponent<Rigidbody>();
                if (temprigidbody != null)
                    temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                Destroy(temp.GetComponentInChildren<BulidingObject>());
                Destroy(temp.GetComponentInChildren<LandMarkObject>());
            }
        }
    }

    public void CreateNetworkMap()
    {
        MyInRoomInfo myInRoomInfo = MyInRoomInfo.GetInstance();
        var jtc2 = LoadJsonFile<Maptile>(Application.dataPath + "/" + "Map", myInRoomInfo.MapName);
        maptile = jtc2;
        // jtc2.Print();

        for (int i = 0; i < maptile.Tiles.Count; ++i)
        {
            GameObject tilepref = SetTilepref(maptile.Tiles[i].kind);
            GameObject temp = PhotonNetwork.InstantiateRoomObject(tilepref.name, maptile.Tiles[i].position, maptile.Tiles[i].rotate);
        }
    }

    public void MeasurementMap()
    {
        foreach (var tile in maptile.Tiles)
        {
            if (tile.position.x > MaximumX)
                MaximumX = tile.position.x;
            if (tile.position.z > MaximumZ)
                MaximumZ = tile.position.z;
            if (tile.position.x < MinimumX)
                MinimumX = tile.position.x;
            if (tile.position.z < MinimumZ)
                MinimumZ = tile.position.z;
        }
        print($"MaximumX = {MaximumX}, MaximumZ = {MaximumZ}, MinimumX = {MinimumX}, MinimumZ = {MinimumZ}");
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
