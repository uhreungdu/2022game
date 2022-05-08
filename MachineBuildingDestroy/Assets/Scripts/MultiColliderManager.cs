using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiColliderManager : MonoBehaviour
{
    public class CheckAttack
    {
        public float time;
        public PlayerAttack PlayerAttacks;
    }
    // Start is called before the first frame update
    public List<CheckAttack> _checkAttack;
    public void Update()
    {
        foreach (var attack in _checkAttack)
        {
            attack.time += Time.time;
        }
        _checkAttack.RemoveAll(x => x.time >= 0.05);
    }
}
