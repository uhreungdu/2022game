using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditerOnScreenPoint : MonoBehaviour
{
    public MapEditerManager mapEditerManager;
    public Map map;
    // Start is called before the first frame update
    void Start()
    {
        mapEditerManager = MapEditerManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        ScreenObjectCreative();
        UpdateScreenPoint();
    }

    Vector3 GetPoint()
    {   
        Vector3 MouseScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
             Input.mousePosition.y, Camera.main.transform.position.y));
        // Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Camera.main.pixelHeight-Input.mousePosition.y, -Camera.main.nearClipPlane));
        // point.z += point.y * 0.62348f;
        // point.y -= point.y;
        Vector3 point1 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2,
            Screen.height / 2, -Camera.main.transform.position.z));
        Vector3 point2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2,
            Screen.height / 2, -Camera.main.transform.position.z));
        float tempx = point1.x - point2.x;
        float tempy = point1.y - point2.y;
        float tempz = point1.z - point2.z;
        Vector3 temp = new Vector3(tempx, tempy, tempz);
        Debug.Log("x둘 비율 : " + tempx / tempy);
        Debug.Log("z둘 비율 : " + tempz / tempy);
        Debug.Log("표기 : " + point.ToString("N3"));
        return point;
    }

    void ScreenObjectCreative()
    {
        GameObject tilepref = null;
        if (gameObject.transform.childCount < 1)
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
            GameObject temp = Instantiate(tilepref, GetPoint(), tilepref.transform.rotation);
            temp.transform.parent = gameObject.transform;
        }
    }
    
    void UpdateScreenPoint()
    {
        if (gameObject.transform.childCount >= 1)
        {
            transform.GetChild(0).position = GetPoint();
        }
    }
}
