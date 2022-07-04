using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightEffect_Control : MonoBehaviour
{
    public List<GameObject> effects = new List<GameObject>();
    public List<Effect_control> particles = new List<Effect_control>();

    public GameManager gmanager;
    public bool once;

    public float time_delay;
    // Start is called before the first frame update
    void Start()
    {
        gmanager = GameManager.GetInstance();
        for (int i = 0; i < effects.Count; ++i)
        {
            if (effects[i] != null)
            {
                particles.Add(effects[i].GetComponent<Effect_control>());
                effects[i].SetActive(false);
                particles[i].Stop_Particles();
            }
        }

        once = true;

    }
    
    // Update is called once per frame
    void Update()
    {
        if ((int)gmanager.EManager.Ntimer / 65.5f > 0 && once == true)
        {
            readyEevent();
            once = false;
        }

    }

    IEnumerator effect_count(int num, float time)
    {
        yield return new WaitForSeconds(time);
        effects[num].SetActive(true);
        particles[num].Play_Particles();
    }

    public void readyEevent()
    {
        for (int i = 0; i < particles.Count; ++i)
        {
            if (i < 3)
            {
                time_delay += 10f;
            }
            else if (i < 6)
            {
                time_delay += 2f;
            }
            else if (i < 9)
            {
                time_delay += 1.5f;
            }
            else if (i < 12)
            {
                time_delay += 1f;
            }
            else
            {
                time_delay += 0.5f;
            }
            StartCoroutine(effect_count(i, time_delay));
        }
    }
    
}
