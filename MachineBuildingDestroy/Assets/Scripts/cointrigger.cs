using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cointrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    public CharacterController characterController;
    public float pushPower = 2.0F;
    void Start()
    {
        gManager = GameManager.GetInstance();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    void OnTriggerEnter(Collider other)
    {
        int team;
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerState>().AddPoint(1);
            Destroy(gameObject);
        }
    }
}
