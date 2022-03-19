using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditerCamInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름

    private Joystick joystick;

    void Start()
    {
        // 에디터 상에서 체크할려면 WindowsEditor로 해야됨
        if (Application.platform == RuntimePlatform.Android)
            joystick = GameObject.Find("Joystickback").GetComponent<Joystick>();
    }

    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값
    void Update()
    {
        // 게임오버 상태에서는 사용자 입력을 감지하지 않는다
        //if (GameManager.GetInstance() != null)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    return;
        //}
        // move에 관한 입력 감지
        move = Input.GetAxis(moveAxisName);
        // rotate에 관한 입력 감지
        rotate = Input.GetAxis(rotateAxisName);
        if (Application.platform == RuntimePlatform.Android)
        {
            move = joystick.moveVector.y;
            rotate = joystick.moveVector.x;
        }
    }
}
