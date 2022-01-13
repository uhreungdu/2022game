using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�
    public string fireButtonName = "Fire1"; // �߻縦 ���� �Է� ��ư �̸�
    public string JumpButtonName = "Jump"; // �߻縦 ���� �Է� ��ư �̸�
    public string DashButtonName = "Dash"; // �߻縦 ���� �Է� ��ư �̸�

    // �� �Ҵ��� ���ο����� ����
    public float move { get; private set; } // ������ ������ �Է°�
    public float rotate { get; private set; } // ������ ȸ�� �Է°�
    public bool fire { get; private set; } // ������ �߻� �Է°�
    public bool jump { get; private set; } // ������ �߻� �Է°�
    public bool dash { get; private set; } // ������ �߻� �Է°�
    // Update is called once per frame
    void Update()
    {
        // ���ӿ��� ���¿����� ����� �Է��� �������� �ʴ´�
        //if (GameManager.GetInstance() != null)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    return;
        //}
        // move�� ���� �Է� ����
        move = Input.GetAxis(moveAxisName);
        // rotate�� ���� �Է� ����
        rotate = Input.GetAxis(rotateAxisName);
        // fire�� ���� �Է� ����
        fire = Input.GetButton(fireButtonName);

        jump = Input.GetButton(JumpButtonName);
        dash = Input.GetButton(DashButtonName);
    }
}
