using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Hammer_Effect : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject effect_obj;
    public GameObject Tail_obj;
    public VisualEffect Hit_VFX;
    public BoxCollider box_col;
    void Start()
    {
        Hit_VFX = effect_obj.GetComponent<VisualEffect>();
        box_col = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (box_col.enabled == true)
        {
            Tail_obj.SetActive(true);
        }
        else
        {
            Tail_obj.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"||other.tag == "Wall" || other.tag == "Obstcle_Item")
        {
            if (other.gameObject != transform.root.gameObject)
            {
                Vector3 centerpo = (other.ClosestPoint(transform.position) + transform.position)/2f;
                effect_obj.transform.position = centerpo;
                Hit_VFX.Play();
            }
        }
    }
}
