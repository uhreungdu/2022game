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
        if(other.gameObject.tag == "Player")
        {
            gManager.setScore(0,1);
            Destroy(gameObject);
        }
    }
}
