using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MapEditerCamInput : MonoBehaviour
{
    private PlayerInput _gamePlayerInput;
    public MapEditerManager _mapEditerManager;
    public MapEditerOnScreenPoint _mapEditerOnScreenPoint;
    public Map _map;
    public Vector3 BeforeMousePoint = new Vector3(-1, -1, -1);
    private int Allprefcount = 7;
    
    // private Joystick _joystick;

    void Start()
    {
        // 에디터 상에서 체크할려면 WindowsEditor로 해야됨
        // if (Application.platform == RuntimePlatform.Android)
        //     _joystick = GameObject.Find("Joystickback").GetComponent<Joystick>();
        _gamePlayerInput = GetComponent<PlayerInput>();
    }

    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값
    
    public float _zoom { get; private set; } // 감지된 회전 입력값

    public Vector3 _direction { get; private set; }
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
        // // move에 관한 입력 감지
        // move = Input.GetAxis(moveAxisName);
        // _gamePlayerInput.
        // // rotate에 관한 입력 감지
        // rotate = Input.GetAxis(rotateAxisName);
        
        // if (Application.platform == RuntimePlatform.Android)
        // {
        //     move = _joystick.moveVector.y;
        //     rotate = _joystick.moveVector.x;
        // }
    }

    public void ResetDirection()
    {
        _direction = Vector3.zero;
    }

    public void OnMove(InputValue value)
    {
        Vector3 valueVector3 = new Vector3(value.Get<Vector2>().x, value.Get<Vector2>().y, 0);
        Vector3 NextMouseVector3 = Vector3.zero;
        if (BeforeMousePoint == new Vector3(-1, -1, -1))
        {
            BeforeMousePoint =
                new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
            return;
        }
        else
        {
            NextMouseVector3 = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
        }
        BeforeMousePoint = Camera.main.ScreenToWorldPoint(new Vector3(BeforeMousePoint.x,
            BeforeMousePoint.y, Camera.main.transform.position.y));
        NextMouseVector3 = Camera.main.ScreenToWorldPoint(new Vector3(NextMouseVector3.x,
            NextMouseVector3.y, Camera.main.transform.position.y));
        Vector3 directionVector3 = NextMouseVector3 - BeforeMousePoint;
        if (Mouse.current.middleButton.isPressed)
        {
            _direction += new Vector3(-directionVector3.x, 0, -directionVector3.z);
        }
        BeforeMousePoint =
            new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
    }
    
    public void OnZoom(InputValue value)
    {
        _zoom = value.Get<float>();
        if (_zoom > 1)
            _zoom /= _zoom;
        else if(_zoom < -1)
            _zoom /= -_zoom;
    }
    
    public void OnSwitchBuilding(InputValue value)
    {
        if (value.isPressed)
            _mapEditerOnScreenPoint.PrefnumSet((_mapEditerManager.Prefnum + 1) % (_map.Prefs.Length));
    }
    
    public void OnRotate(InputValue value)
    {
        if (value.isPressed)
            _mapEditerOnScreenPoint._rotate = (_mapEditerOnScreenPoint._rotate + 90) % 360;
    }
}
