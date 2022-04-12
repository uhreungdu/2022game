using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Manager : MonoBehaviour
{
    public GameObject Child_effect1;
    public GameObject Child_effect2;
    public GameObject effect_acitve;
    // Start is called before the first frame update
    void Start()
    {
        Child_effect1 = gameObject.transform.GetChild(0).gameObject;
        Child_effect2 = gameObject.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Child_effect1.GetComponent<ParticleSystem>().isPlaying &&
            !Child_effect2.GetComponent<ParticleSystem>().isPlaying)
        {
            effect_acitve = Resources.Load<GameObject>("New_Heal_Effect");
            Instantiate(effect_acitve, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
