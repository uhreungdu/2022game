using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class cointrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public CharacterController characterController;
    private float rotateSpeed = 10;
    void Start()
    {
        gManager = GameManager.GetInstance();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= quaternion.Euler(0, Time.deltaTime * rotateSpeed, 0);
    }
}
