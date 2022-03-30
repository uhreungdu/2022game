using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Obstacle_Obj : LivingEntity, IPunObservable
{
    // Start is called before the first frame update
    public List<GameObject> obstacles_list;
    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            obstacles_list.Add(gameObject.transform.GetChild(i).gameObject);
            obstacles_list[i].GetComponent<Dissolve_Control>().dissolve_switch = true;
        }
        onDeath += Destory_Obs;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }

    public void Destory_Obs()
    {
        for (int i = 0; i < obstacles_list.Count; i++)
        {
            if (obstacles_list[i] != null)
            {
                obstacles_list[i].GetComponent<Dissolve_Control>().dissolve_switch = false;
            }
        }
    }
    [PunRPC]
    public override void OnDamage(float damage)
    {
        base.OnDamage(damage);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트이면 쓰기 부분이 실행됩니다.
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        // 리모트 오브젝트이면 읽기 부분이 실행됩니다.
        else
        {
            health = (float) stream.ReceiveNext();
            OnDamage(0);
        }
    }
}
