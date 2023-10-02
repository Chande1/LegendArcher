using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class throw_Enemy : MonoBehaviour
{
    //투척 몬스터용 스크립트
    public AnimationClip Atk_anime;//공격애니메이션
    public GameObject bullet;//던질 총알
    public GameObject hand;//총알이 나갈 위치
    UI_manager ui;
    public HP_M hp;//체력 매니져
    public float EnemyHP = 100;//몬스터 체력 설정
    private Transform Player;//타겟 즉 타워
    public float Atk = 1;// 공격력
    float EnemyHp_S = 0;

    Animator Ani;

    float ATK_delay = 3f;//공격 딜레이 
    float ATK_deal_re_save;//공격딜레이 초기화용 변수
    bool Atk_ON = false;//공격 스위칭용 변수
    bool Active_bt = false;
    public float Time_to_Active = 1f;//골렘이 일어나기 까지 걸리는 시간
    float Time_to_Active_Count_1 = 0;
    float Time_to_Active_Count_2 = 0;
    float Time_to_Active_Count_3 = 0;
    float Time_to_Active_Count_4 = 0;
    float Count = 0f;

    public GameObject target;//공격할대상

    float time_sturn = 6;//스턴용 
    bool Arrow_sturn = false;//3.0f 스턴 구분용
    bool Arrow_sturn_2 = false;//6.0f 스턴 구분용
    AudioSource adi;
    public AudioClip dead;
    public AudioClip revive;
    public AudioClip throw_st_1;//1
    public AudioClip throw_st_2;//2
    bool ATK_ad_c = false;//false 1 트루면 2
    public bool stiff = false; //경직

    [Header("받고 있는 스킬의 종류")]
    public Skill s;
    public SkillValue sv;

    private void Awake()
    {
        if (UI_manager.stage1difficulty == 1)//nomal
        {
            Time_to_Active_Count_1 = 40;
            Time_to_Active_Count_2 = 40;
            Time_to_Active_Count_3 = 30;
            Time_to_Active_Count_4 = 30;
        }
        if (UI_manager.stage1difficulty == 2)//hard
        {
            Time_to_Active_Count_1 = 35;
            Time_to_Active_Count_2 = 32;
            Time_to_Active_Count_3 = 27;
            Time_to_Active_Count_4 = 27;

        }
        adi = GetComponent<AudioSource>();
        this.Ani = GetComponent<Animator>();
        hp = GetComponent<HP_M>();//체력 매니저

        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//플레이어의 위치는 캐슬이라는 태그를 가진 오브젝트에게서 가져온다.  
            target = Player.gameObject;//목표 설정(떄릴거)
        }
       
        ATK_delay = 5.9f;//대기모션 시간+ 공격시간
        ATK_deal_re_save = Atk_anime.length;
        EnemyHp_S = EnemyHP;
    }
    private void Start()
    {
        hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기

        if (UI_manager.stage1difficulty == 1)//nomal
        {
            Time_to_Active = 50;
        }
        if (UI_manager.stage1difficulty == 2)//hard
        {
            Time_to_Active = 40;
        }
    }
    // Update is called once per frame

    void FixedUpdate()
    {
        if (UI_manager.stage1difficulty != 0)
        {



            if (EnemyHP <= 0)//죽음
            {
                adi.clip = dead;//현재 이 오브젝트는 데드 클립을 담고있어요
                adi.Play();
                Ani.SetBool("On", false);
                Ani.SetBool("isdie", true);
                Ani.SetBool("idel", false);
                Active_bt = false;
                if (Count == 0)
                    Time_to_Active = Time_to_Active_Count_1;//시간 초기화
                else if (Count == 1)
                    Time_to_Active = Time_to_Active_Count_2;//시간 초기화
                else if (Count >= 2)
                    Time_to_Active = Time_to_Active_Count_3;//시간 초기화 

                EnemyHP = EnemyHp_S;//초기화
                Count++;
                //Invoke("ks", 3f);
            }
            if (stiff == true)
            {
                Ani.speed = 0f;
            }



            hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기


            if (Time_to_Active >= 0 && Active_bt == false)
            {
                Time_to_Active -= Time.deltaTime;
            }
            else
            if (Time_to_Active <= 0 && Active_bt == false)
            {
                ATK_delay = 6f;
                Active_bt = true;
                Ani.SetBool("isdie", false);
                Ani.SetBool("Atk_On", false);
                Ani.SetBool("On", true);
                adi.clip = revive;
                adi.Play();
            }
            if (stiff == false)
            {
                Ani.speed = 1f;
                if (Active_bt == true&&target)
                {
                    Ani.SetBool("idel", true);
                    Vector3 relativePos = target.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = rotation;

                    Ani.SetBool("Atk_On", true);
                    ATK_delay -= 0.02f;//공격 딜레이를 줄여준다 
                    if (ATK_delay <= 0 && EnemyHP > 0)// 공격 딜레이가 0보다 아래가 되면
                    {
                        if (ATK_ad_c == false)
                        {
                            adi.clip = throw_st_1;
                            ATK_ad_c = true;

                        }
                        else
                        {
                            ATK_ad_c = false;
                            adi.clip = throw_st_2;

                        }
                        adi.Play();
                        Attacking();//공격 함수를 발동
                    }
                }
                else
                {

                    Ani.SetBool("Atk_On", false);
                }
            }

        }
    }


    void Attacking()
    {
        Instantiate(bullet, hand.gameObject.transform.position, Quaternion.identity);//총알 내보내기
        ATK_delay = ATK_deal_re_save;//다시 초기화
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //EnemyHP -= 5f;


        }

    }
    public void Damaging(float _damage)
    {
        EnemyHP -= _damage;
    }

    public void SkillSetting(Skill _skill)
    {
        s = _skill;
        if (_skill == null)
        {
            sv = SkillValue.Non;
        }
        else
        {
            sv = _skill.value;
        }
    }

    public void ks()
    {
        WaveManager wavemanager = GameObject.Find("UI_Manager").GetComponent<WaveManager>();
        wavemanager.KillScore++;
    }



}
