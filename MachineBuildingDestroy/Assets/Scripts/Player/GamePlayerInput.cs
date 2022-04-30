using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축 이름
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축 이름
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼 이름
    public string JumpButtonName = "Jump"; // 점프를 위한 입력 버튼 이름
    public string DashButtonName = "Dash"; // 대쉬를 위한 입력 버튼 이름
    public string InteractionButtonName = "Interaction"; // 상호작용를 위한 입력 버튼 이름
    public string ItemButtonName = "Item"; // 상호작용를 위한 입력 버튼 이름

    private Joystick _joystick;
    private AttackButton _atkButton;
    private JumpButton _jmpButton;
    private DashButton _dshButton;
    private InterractButton _itrButton;
    private ItemButton _itmButton;
    private GameManager _gameManager;
    private PlayerInput _playerInput;

    void Start()
    {
        // 에디터 상에서 체크할려면 WindowsEditor로 해야됨
        if (Application.platform == RuntimePlatform.Android)
        {
            _joystick = GameObject.Find("Joystickback").GetComponent<Joystick>();
            _atkButton = GameObject.Find("AttackButton").GetComponent<AttackButton>();
            _jmpButton = GameObject.Find("JumpButton").GetComponent<JumpButton>();
            _dshButton = GameObject.Find("DashButton").GetComponent<DashButton>();
            _itrButton = GameObject.Find("InteractButton").GetComponent<InterractButton>();
            _itmButton = GameObject.Find("ItemButton").GetComponent<ItemButton>();
        }
        _gameManager = GameManager.GetInstance();
    }

    // 값 할당은 내부에서만 가능
    public float move { get; private set; } // 감지된 움직임 입력값
    public float rotate { get; private set; } // 감지된 회전 입력값
    public bool fire { get; private set; } // 감지된 발사 입력값
    public bool fireKeyDown { get; private set; }
    public bool jump { get; private set; } // 감지된 발사 입력값
    public bool dash { get; private set; } // 감지된 발사 입력값
    public bool Interaction { get; private set; } // 감지된 발사 입력값
    public bool item { get; private set; }
    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            move = _joystick.moveVector.y;
            rotate = _joystick.moveVector.x;
            fire = _atkButton.isPressed;
            jump = _jmpButton.isPressed;
            dash = _dshButton.isPressed;
            Interaction = _itrButton.isPressed;
            item = _itmButton.isPressed;
        }
        else
        {
            // move에 관한 입력 감지
            move = Input.GetAxis(moveAxisName);
            // rotate에 관한 입력 감지
            rotate = Input.GetAxis(rotateAxisName);
            // fire에 관한 입력 감지
            if (fire == true)
                fireKeyDown = true;
            else
                fireKeyDown = false;
            fire = Input.GetButton(fireButtonName);
            jump = Input.GetButton(JumpButtonName);
            dash = Input.GetButton(DashButtonName);
            Interaction = Input.GetButton(InteractionButtonName);
            item = Input.GetButton(ItemButtonName);
        }
        if (_gameManager.gameStart == false || _gameManager.EManager.gameSet)
        {
            // move에 관한 입력 감지
            move = 0;
            // rotate에 관한 입력 감지
            rotate = 0;
            // fire에 관한 입력 감지
            fire = false;
            jump = false;
            dash = false;
            Interaction = false;
            item = false;
        }
    }

//     public void OnMove(InputValue value)
//     {
//         Vector2 movement = value.Get<Vector2>();
//         move = movement.x;
//         rotate = movement.y;
//     }
//     
//     public void OnFire(InputValue value)
//     {
//         fire = value.Get<bool>();
//     }
//     public void OnJump(InputValue value)
//     {
//         jump = value.Get<bool>();
//     }
//     public void OnDash(InputValue value)
//     {
//         dash = value.Get<bool>();
//     }
//     public void OnInteraction(InputValue value)
//     {
//         Interaction = value.Get<bool>();
//     }
 }
