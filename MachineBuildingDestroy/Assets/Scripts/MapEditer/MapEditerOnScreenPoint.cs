using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class MapEditerOnScreenPoint : MonoBehaviour
{
    public MapEditerManager mapEditerManager;
    public Map map;
    public BoxCollider boxCollider;

    public Material RadMaterial;
    public Material Blue;

    // Start is called before the first frame update
    void Start()
    {
        mapEditerManager = MapEditerManager.GetInstance();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        ScreenObjectCreative();
        UpdateScreenPoint();
        MouseClick();
        BuildingChange();
    }

    Vector3 GetPoint()
    {
        Vector3 MouseScreenPoint = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.x.ReadValue(), 0);
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.x.ReadValue(),
            Mouse.current.position.y.ReadValue(), Camera.main.transform.position.y));
        // Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Camera.main.pixelHeight-Input.mousePosition.y, -Camera.main.nearClipPlane));
        // point.z += point.y * 0.62348f;
        // point.y -= point.y;
        point.y = 0;
        Debug.Log("표기 : " + point.ToString("N3"));
        return point;
    }

    void ScreenObjectCreative()
    {
        GameObject tilepref = null;
        if (!mapEditerManager.SaveMode && gameObject.transform.childCount < 1)
        {
            switch (mapEditerManager.Prefnum)
            {
                case 0:
                    tilepref = map.planepref;
                    break;
                case 1:
                    tilepref = map.goalpostpref;
                    break;
                case 2:
                    tilepref = map.itempref;
                    break;
                case 3:
                    tilepref = map.tankpref;
                    break;
                case 4:
                    tilepref = map.arcadepref;
                    break;
            }

            if (tilepref != null)
            {
                GameObject temp = Instantiate(tilepref, GetPoint(), tilepref.transform.rotation);
                temp.transform.parent = gameObject.transform;
            }
        }
    }

    void UpdateScreenPoint()
    {
        if (mapEditerManager.SaveMode)
        {
            if (gameObject.transform.childCount >= 1)
                Destroy(transform.GetChild(0).gameObject);
        }
        else if (gameObject.transform.childCount >= 1)
        {
            Vector3 Point = GetPoint();
            int x = (int) GetPoint().x / 10;
            int z = (int) GetPoint().z / 10;
            if (Point.x <= 0)
            {
                Point.x = -5 + x * 10;
            }
            else
            {
                Point.x = 5 + x * 10;
            }

            if (Point.z <= 0)
            {
                Point.z = -5 + z * 10;
            }
            else
            {
                Point.z = 5 + z * 10;
            }

            transform.position = Point;
            transform.GetChild(0).position = Point;
        }
    }

    void MouseClick()
    {
        if (!mapEditerManager.SaveMode && gameObject.transform.childCount >= 1)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                if (!InstallCheck())
                {
                    Map.Tile tile = new Map.Tile();
                    tile.kind = mapEditerManager.Prefnum;
                    tile.position = transform.GetChild(0).position;
                    map.maptile.Tiles.Add(tile);
                    transform.GetChild(0).parent = map.gameObject.transform;
                }
            }

            if (Mouse.current.rightButton.isPressed)
            {
                boxCollider.enabled = true;
            }
            else 
                boxCollider.enabled = false;
        }
    }

    void BuildingChange()
    {
        if (Keyboard.current.digit1Key.isPressed)
        {
            PrefnumSet(0);
        }

        if (Keyboard.current.digit2Key.isPressed)
        {
            PrefnumSet(1);
        }

        if (Keyboard.current.digit3Key.isPressed)
        {
            PrefnumSet(2);
        }

        if (Keyboard.current.digit4Key.isPressed)
        {
            PrefnumSet(3);
        }

        if (Keyboard.current.digit5Key.isPressed)
        {
            PrefnumSet(4);
        }
    }

    void PrefnumSet(int num)
    {
        if (mapEditerManager.Prefnum != num)
        {
            if (gameObject.transform.childCount >= 1)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            mapEditerManager.Prefnum = num;
        }
    }

    bool InstallCheck()
    {
        foreach (var allTile in map.maptile.Tiles)
        {
            Vector3 comp1 = new Vector3((int) allTile.position.x, 0,
                (int) allTile.position.z);
            Vector3 comp2 = new Vector3((int) transform.GetChild(0).position.x,
                0, (int) transform.GetChild(0).position.z);
            if (mapEditerManager.Prefnum == 0 && allTile.kind == 0 && comp1 == comp2)
            {
                return true;
            }
            else if (mapEditerManager.Prefnum != 0 && allTile.kind > 0 && comp1 == comp2)
            {
                return true;
            }
        }
        return false;
    }
    
    bool InstallCheckRemove()
    {
        foreach (var allTile in map.maptile.Tiles)
        {
            Vector3 comp1 = new Vector3((int) allTile.position.x, 0,
                (int) allTile.position.z);
            Vector3 comp2 = new Vector3((int) transform.GetChild(0).position.x,
                0, (int) transform.GetChild(0).position.z);
            if (mapEditerManager.Prefnum == 0 && allTile.kind == 0 && comp1 == comp2)
            {
                map.maptile.Tiles.Remove(allTile);
                return true;
            }
            else if (mapEditerManager.Prefnum != 0 && allTile.kind > 0 && comp1 == comp2)
            {
                map.maptile.Tiles.Remove(allTile);
                return true;
            }
        }
        return false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (InstallCheck() && other.transform.parent.name != "Pointer")
        {
            InstallCheckRemove();
            Destroy(other.gameObject);
        }
    }
    
}