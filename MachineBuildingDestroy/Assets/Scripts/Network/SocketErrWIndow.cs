using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketErrWIndow : MonoBehaviour
{
    private void Start()
    {
        LoginDBConnection.GetInstance().socketErrWindow = this.gameObject;
        gameObject.SetActive(false);
    }
}
