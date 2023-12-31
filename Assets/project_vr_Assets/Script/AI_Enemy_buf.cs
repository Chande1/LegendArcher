using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//네브매쉬 사용할려면 필요함

public class AI_Enemy_buf : MonoBehaviour
{
    //버프 몬스터용 스크립트
    public NavMeshAgent nav;//네브매쉬 
    public AnimationClip Atk_anime;//공격애니메이션
    public HP_M hp;//체력 매니져
    public float EnemyHP = 100;//몬스터 체력 설정

    Tower_fire T_F_M;//보조 타워
    private Transform Player;//타겟 즉 타워
    public GameObject target;//공격할대상
    AudioSource audio_s;
    public AudioClip ATk1;
    public AudioClip ATk2;
    public AudioClip dead1;
    public AudioClip dead2;
    public AudioClip hit1;
    public AudioClip hit2;
    int Rm = 0;
    public float Atk = 1;// 공격력
    public float Atk_ex = 1;// 공격력의 기존 값 (버프가 끝나고 원래 상태로 돌아갈떄 쓰임)
    public float ATK_UP = 5f;//버프를 받으면 증가하는 공격력의 양
    public float ATK_delay = 3f;//공격 딜레이 
    float ATK_deal_re_save = 0f;//공격딜레이 초기화용 변수
    public float Atk_length = 2f;//공격 사거리
    bool Atk_ON = false;//공격 스위칭용 변수
    public float Speed = 2f;//이속
    public float Speed_up = 5f;//스피드 증가 버프를 받으면 증가하는 양
    public float Speed_A = 0;
    public float nukBack_B = 0.5f;
    public bool attack = true; //공격받는중
    public bool stiff = false; //경직
    public float dead_Time;

    [Header("받고 있는 스킬의 종류")]
    public Skill s;
    public SkillValue sv;


    Animator ani;//애니메이터

    // Start is called before the first frame update
    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        Speed_A = Speed;//스피도의 기존값을 저장하기 위해
        hp = GetComponent<HP_M>();//체력 매니저
        ani = GetComponent<Animator>();
        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//플레이어의 위치는 캐슬이라는 태그를 가진 오브젝트에게서 가져온다.
            this.nav = GetComponent<NavMeshAgent>();//네브메쉬 지정.
            this.nav.destination = Player.transform.position;//네브매쉬의 목적지는 플레이어(타워)가 있는 위치이다.
            this.nav.autoBraking = false;//autoBraking은 대상이 목적지에 가까워지면 속도가 감소 되는것이다.
                                         //this.nav.stoppingDistance = Atk_length;//네브매쉬의 멈출거리
            this.nav.speed = Speed;//몬스터의 이동속도는 1f이다s
        }
      

    }
    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            if(GameObject.FindGameObjectWithTag("Tower_AI"))
            {
                T_F_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                T_F_M.GM_IN(gameObject);//자기 정보를 타워한테 일단 준다
            }
          
        }
        hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기
        T_F_M.GM_IN(this.gameObject);//자기 넣기
        ATK_deal_re_save = ATK_delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        hp.HP_save(EnemyHP);

        if(this.nav)
            this.nav.speed = Speed;//몬스터의 이동속도는 1f이다s
        if (stiff == false&&Player)          //경직이 아니라면
        {
            if (nav.destination != null)
                this.nav.destination = Player.transform.position;//그 타워의 hp를 0으로 만들면 목적지를 다시 플레이어(타워)로 바꾼다.  




            ani.SetBool("Atk", false);// 공격 애니메이션 

            if (Atk_ON == true)
            {


                if (nav.remainingDistance <= nav.stoppingDistance&&target)//타겟과의 거리가 2보다 작으면 
                {
                    Vector3 relativePos = target.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = rotation;
                    ani.speed /= ATK_deal_re_save;
                    ani.SetBool("Atk", true);// 공격 애니메이션 발동
                    ATK_delay -= Time.deltaTime;//공격 딜레이를 줄여준다 
                    if (ATK_delay <= 0)// 공격 딜레이가 0보다 아래가 되면
                    {


                        Attacking();//공격 함수를 발동
                        ATK_delay = ATK_deal_re_save;//다시 초기화
                    }
                }

                if (target.GetComponent<HP_M>().Hp <= 0)
                {
                    Atk_ON = false;//공격을 false로 지정
                    this.nav.destination = Player.transform.position;//그 타워의 hp를 0으로 만들면 목적지를 다시 플레이어(타워)로 바꾼다.  
                }

            }

            ani.speed = 1f;
        }
        else
        if (stiff == true)
        {
            nav.ResetPath();
            if (EnemyHP > 0)
                ani.speed = 0f;
            else
            {
                ani.speed = 1f;
            }

        }
        if (EnemyHP <= 0&&attack)
        {

            T_F_M.Gm_Out(this.gameObject);
            Invoke("ks", 1f);
            nav.ResetPath();
            this.gameObject.tag = "Untagged";
            ani.speed = 1f;
            ani.SetBool("die", true);
            Destroy(this.gameObject, dead_Time);
            attack = false;
        }

    }

    void Audio_play()
    {
        Rm = Random.Range(0, 2);
        //공격
        if (Rm == 0 && EnemyHP > 0 && ATK_delay < 0)
        {
            audio_s.clip = ATk1;

        }
        else if ((Rm == 1 && EnemyHP > 0) && ATK_delay < 0)
        {

            audio_s.clip = ATk2;
        }
        //죽었을떄
        if (Rm == 0 && EnemyHP <= 0)
        {
            audio_s.clip = dead1;

        }
        if (Rm == 1 && EnemyHP <= 0)
        {
            audio_s.clip = dead2;

        }

        audio_s.Play();
    }
    void Attacking()//몬스터가 공격하는중이면 
    {
        target.GetComponent<HP_M>().dmg_HP(Atk);//그 오브젝트의 hp 매니져에 공격력만큼  체력에 뺀다.
        Audio_play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//플레이어의 타워를 타겟으로 삼는다.
            Atk_ON = true;
            ATK_deal_re_save = Atk_anime.length;
            ATK_delay = Atk_anime.length;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            this.nav.speed = 0;
            Vector3 VFirst;//맞은거
            Vector3 VLast;// 자신
            VLast = other.gameObject.transform.position;
            VFirst = gameObject.transform.position;

            Debug.Log(VLast);
            if (VLast.z > VFirst.z)
            {
                this.gameObject.transform.Translate(new Vector3(0, 0, -nukBack_B));
            }
            else if (VLast.z == VFirst.z)
            {

            }
            else
            {
                this.gameObject.transform.Translate(new Vector3(0, 0, nukBack_B));
            }

            if (VLast.x > VFirst.x)
            {
                this.gameObject.transform.Translate(new Vector3(-nukBack_B, 0, 0));
            }
            else if (VLast.x == VFirst.x)
            {

            }
            else
            {
                this.gameObject.transform.Translate(new Vector3(nukBack_B, 0, 0));
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))// 버프 장판 안에 있으면 대미지 증가
        {
            if (Atk == Atk_ex)// ATK가 처음 정해진 공격력이랑 같으면
                Atk += ATK_UP;//공격력 증가
            if (nav.speed == Speed)//네브매쉬의 속도가 변수 스피드랑 같으면
                nav.speed += Speed_up;//스피드업을 네브매쉬의 스피드에 더해준다
            // nav.speed 는 대상이 목적지에 가는데 걸리는 속도를 조절하는것이다.
        }
        if (other.gameObject.CompareTag("Tower_type_1"))
        {
            nav.SetDestination(other.gameObject.transform.position);//공격하는 타워가 근처에 있으면 타겟을 그 타워로 바꾼다.
            target = other.gameObject;//타겟은 Tower_type_1의 태그를 가진 오브젝트이다
            Atk_ON = true;
        }
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//플레이어의 타워를 타겟으로 삼는다.
            Atk_ON = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        /*
        if (this.gameObject.CompareTag("AI_buf") && other.gameObject.CompareTag("skill_move_speed up_02"))
        {
            if (nav.speed != Speed)//스피드가 같지않으면
                nav.speed -= Speed_up;//스피드를 뺴준다.
            // nav.speed 는 대상이 목적지에 가는데 걸리는 속도를 조절하는것이다.
        }

        if (this.gameObject.CompareTag("AI_buf") && other.gameObject.CompareTag("Skill_damge up_02"))
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;

        }

        if (this.gameObject.CompareTag("AI_buf_2") && other.gameObject.CompareTag("skill_move_speed up"))
        {
            if (nav.speed != Speed)
                nav.speed -= Speed_up;
            // nav.speed 는 대상이 목적지에 가는데 걸리는 속도를 조절하는것이다.
        }

        if (this.gameObject.CompareTag("AI_buf_2") && other.gameObject.CompareTag("Skill_damge up"))
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
        }
        */
        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))//버프 장판에서 나갈 경우
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
            if (nav.speed != Speed)
                nav.speed -= Speed_up;
        }
    }


    public void Damaging(float _damage)
    {
        EnemyHP -= _damage;
        if (EnemyHP > 0)
        {
            Rm = Random.Range(0, 2);
            if (Rm == 0)
            {
                audio_s.clip = hit1;
                audio_s.Play();
            }
            else
            {
                audio_s.clip = hit2;
                audio_s.Play();
            }

        }
        else
        {
            Audio_play();
        }
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

    public bool GetAtk_ON()// 공격하는 중인지 외부에서 확인하는 함수
    {
        if (Atk_ON)
            return true;
        else
            return false;
    }
}
