using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_control : MonoBehaviour
{
    public float rotatespeed = 10.0f;
    public float zoomspeed = 10.0f;

    private Camera maincam;
    // Start is called before the first frame update
    void Start()
    {
        maincam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ZoomCam();
        RotateCam();
    }
    void ZoomCam()
    {
        float now_Dis = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomspeed;
        if(now_Dis != 0)
        {
            maincam.fieldOfView += now_Dis;
        }
    }
    void RotateCam()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = Input.GetAxis("Mouse X") * rotatespeed;
        rot.y = Input.GetAxis("Mouse Y") * rotatespeed;
        Quaternion temp = Quaternion.Euler(rot);
        temp.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation,temp,2f);

    }
}
