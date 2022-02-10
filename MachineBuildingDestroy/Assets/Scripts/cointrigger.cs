using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cointrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gManager;
    void Start()
    {
        gManager = GameManager.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        int team;
        if (other.gameObject.tag == "Player")
        {
            PlayerState playerstate = other.gameObject.GetComponentInParent<PlayerState>();

            Debug.Log("ÆÀ : ");
            gManager.setScore(playerstate.team, 1);
            Destroy(gameObject);
        }
    }
}
