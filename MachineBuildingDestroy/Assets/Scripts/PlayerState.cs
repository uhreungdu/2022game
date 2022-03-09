using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : LivingEntity
{
    public int team { get; private set; }
    public int point { get; private set; }
    // Start is called before the first frame update
    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public GameManager gManager;

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    void Start()
    {
        playerAnimator = GetComponent<Animator>();          // 지금 안됨
        playerAudioPlayer = GetComponent<AudioSource>();    // 지금 안됨
        team = Random.Range(0, 2);
        gManager = GameManager.GetInstance();

        gManager.addTeamcount(team);
        base.OnEnable();
    }
    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        onDeath += DieAction;
        point = 0;
    }
    public void DieAction()
    {
        gameObject.SetActive(false);
    }

    public void AddPoint(int Point)
    {
        point += Point;
    }

    public void ResetPoint()
    {
        point = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
