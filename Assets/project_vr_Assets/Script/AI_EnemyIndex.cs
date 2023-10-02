using UnityEngine;
using UnityEngine.AI;


public class AI_EnemyIndex : MonoBehaviour
{
    public NavMeshAgent nav;//네브매쉬 
    private Transform Player;//타겟 즉 타워
    public AnimationClip Atk_anime;//공격 애니메이션 시간 가져오기 위해 사용 애니메이터에서 클립 가져오는걸 잘몰라서..
    public HP_M hp;//체력 매니져
    public float EnemyHP = 100;//몬스터 체력 설정
    public GameObject target;//공격할대상
    Tower_fire T_f_M;//타워에 값 줄라고 사용

    public float Atk = 1f;// 공격력
    public float Atk_ex = 1f;// 공격력의 기존 값 (버프가 끝나고 원래 상태로 돌아갈떄 쓰임)
    public float ATK_UP = 5f;//버프를 받으면 증가하는 공격력의 양
    public float ATK_delay = 3f;//공격 딜레이 
    float ATK_deal_re_save = 0f;//공격딜레이 초기화용 변수
    bool Atk_ON = false;//공격 스위칭용 변수
    public float Speed = 3f;//이속
    public float Speed_up = 5f;//스피드 증가
    bool Enemy_dmg_01;//상태이상용 변수
    bool Atk_anime_check = false;//애니메이션 전환용 불형태의 변수
    float time_sturn = 0;//스턴용 
    bool Arrow_sturn = false;//3.0f 스턴 구분용
    bool Arrow_sturn_2 = false;//6.0f 스턴 구분용
    public float deadtime;

    public float nukBack_B = 0.5f;

    Animator ani;//애니메이터
    public bool bt = false;

    AudioSource adi_s;
    public AudioClip Atk_0;
    public AudioClip Atk_1;
    public AudioClip dead_0;
    public AudioClip dead_1;
    public AudioClip Hit_0;
    public AudioClip Hit_1;

    bool sound_change = false;//1 false 2 true
    Vector3 pos;

    public bool attack = true; //공격받는중
    public bool stiff = false; //경직


    [Header("받고 있는 스킬의 종류")]
    public Skill s;
    public SkillValue sv;
    /*
    [Space(10f)]
    [Header("적 움직임 설정")]
    [Header("수평")]
    [SerializeField] bool horizontal = false;   //수평
    [SerializeField] float hspeed = 1f;      //수평 움직임 속도
    [SerializeField] float hdistance = 1f;  //수평 움직임 거리
    [Space(10f)]
    [Header("수직")]
    [SerializeField] bool verticality = false;   //수직
    [SerializeField] float vspeed = 1f;      //수평 움직임 속도
    [SerializeField] float vdistance = 1f;  //수평 움직임 거리
    [Header("전진모드")]
    [SerializeField] bool frontmode = false;   //전진
    [SerializeField] float fmspeed = 1f;      //전진 모드 움직임 속도*/

    //

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<빌드 순서는 AWAke>Start>update 순이다.>>>>>>>>>>>>>>>>>>>>>>>>

    // Start is called before the first frame update

    private void Awake()
    {
        adi_s = GetComponent<AudioSource>();
        //Debug.Log(EnemyHP);
        Atk_ON = false;
        this.ani = GetComponent<Animator>();//가져오기
        Atk_ex = Atk;//값을 동일하게
        Atk = Atk_ex;//동일하게
        hp = GetComponent<HP_M>();
        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();
            this.nav = GetComponent<NavMeshAgent>();//네브메쉬 지정.
                                                    //this.nav.destination = Player.transform.position;//네브매쉬의 목적지는 플레이어(타워)가 있는 위치이다.
                                                    //this.nav.speed = Speed;//몬스터의 이동속도는 1f이다
            this.nav.autoBraking = false;//autoBraking은 대상이 목적지에 가까워지면 속도가 감소 되는것이다.
                                         //그러한것들을 막기위해 false로 지정해준다.
        }

        ATK_deal_re_save = ATK_delay;
        //this.nav.stoppingDistance = 4.5f;//네브매쉬의 멈출거리
    }
    private void Start()
    {
        adi_s = GetComponent<AudioSource>();
        for (int i = 0; i < 2; i++)
        {
            if (GameObject.FindGameObjectWithTag("Tower_AI"))    //버프 타워가 없을 경우 대비
            {
                T_f_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                T_f_M.GM_IN(gameObject);//자기 정보를 타워한테 일단 준다
            }

        }

        hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기
        //this.nav.speed = Speed;//몬스터의 이동속도는 1f이다

    }

    private void Update()
    {
        //Debug.Log(stiff);
        if (stiff == false)          //경직이 아니라면
        {
            ani.speed = 1f;
            if (EnemyHP > 0)
            {
                if (ani.GetBool("ATK") != false)
                    ani.SetBool("ATK", false);

                if (nav.destination != null)
                    this.nav.destination = Player.transform.position;//네브매쉬의 목적지는 플레이어(타워)가 있는 위치이다.
            }


            if (time_sturn > 0)
            {
                time_sturn -= Time.deltaTime;
                this.ani.StopPlayback();//애니메이션이 멈춘다
            }


            if (time_sturn <= 0)
            {
                anime_ON();//애니메이션 출용 용도
                hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기
                this.nav.speed = Speed;//몬스터의 이동속도는 1f이다

                if (Atk_ON == true)
                {
                    if (nav.remainingDistance <= nav.stoppingDistance&&target)//타겟과의 거리가 2보다 작으면 
                    {
                        Vector3 relativePos = target.transform.position - transform.position;
                        Quaternion rotation = Quaternion.LookRotation(relativePos);
                        transform.rotation = rotation;

                        ani.speed /= ATK_deal_re_save;
                        ATK_delay -= Time.deltaTime;//공격 딜레이를 줄여준다
                        Atk_anime_check = true;//공격 애니메이션 발동

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
                        Atk_anime_check = false;//떄리는 모션 비활성화
                    }

                }

            }

        }
        else if (stiff == true)
        {
            if (EnemyHP > 0)
                ani.speed = 0f;
            else
            {
                ani.speed = 1f;
            }

            nav.ResetPath();

        }

        if (this.EnemyHP <= 0&&attack)
        {
            for (int i = 0; i < 2; i++)
            {
                if (GameObject.FindGameObjectWithTag("Tower_AI"))    //버프 타워가 없을 경우 대비
                {
                    T_f_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                    T_f_M.Gm_Out(this.gameObject);//자기 정보를 타워한테 일단 준다
                }

            }
            //T_f_M.Gm_Out(this.gameObject);
            this.gameObject.tag = "Untagged";
            this.nav.speed = 0;//몬스터의 이동속도는 1f이다
            Invoke("ks", 1f);
            nav.ResetPath();
            ani.SetBool("die", true);
            Destroy(this.gameObject, deadtime);
            attack = false;
        }
    }


    void Audio_play()
    {
        //공격
        if (sound_change == false && EnemyHP > 0 && ATK_delay <= 0)
        {
            adi_s.clip = Atk_0;
            sound_change = true;
            Debug.Log("in1");
        }
        if ((sound_change == true && EnemyHP > 0) && ATK_delay <= 0)
        {
            sound_change = false;
            adi_s.clip = Atk_1;
            Debug.Log("in2");
        }
        //죽었을떄
        if (sound_change == false && EnemyHP <= 0)
        {
            adi_s.clip = dead_0;
            sound_change = true;
        }
        if (sound_change == true && EnemyHP <= 0)
        {
            adi_s.clip = dead_1;
            sound_change = true;
        }

        adi_s.Play();
    }


    private void anime_ON()//애니메이션 출연 용도
    {

        if (Atk_anime_check == true)//공격 애니메이션
        {
            ani.SetBool("Attack", true);//공격하는 애니메이션 발동

        }

        if (Atk_anime_check == false)//공격중이 아니라면 
        {
            ani.SetBool("Attack", false);// 걷는 애니메이션 발동
        }

    }

    private void OnTriggerEnter(Collider other)//스킬들이나 타워 관련 스크립트용 
    {
        //Debug.Log("OnTriggerEnter");
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//플레이어의 타워를 타겟으로 삼는다.
            //ATK_deal_re_save = Atk_anime.length;
            //ATK_delay = Atk_anime.length;
            Atk_ON = true;
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
        /*
        if(other.gameObject.CompareTag("Bullet"))// 플레이어의 공격 태그 넣어야함
          {
          hp.dmg_HP(5f);//대미지 받을 수치하고
          }
        */
        //안사용 되고있음 현재 스턴용도
        //if (other.gameObject.CompareTag("ice_Arrow"))
        //{
        //    Arrow_sturn = true;
        //    if (Arrow_sturn_2 == true)
        //    {
        //        time_sturn += 6.0f;
        //        Arrow_sturn_2 = false;
        //    }
        //    else
        //    {
        //        time_sturn = 3.0f;
        //        Arrow_sturn_2 = false;
        //    }

        //}
        //if (other.gameObject.CompareTag("ice_Arrow_2"))
        //{
        //    Arrow_sturn_2 = true;
        //    if (Arrow_sturn == true)
        //    {
        //        time_sturn += 3.0f;
        //        Arrow_sturn = false;
        //    }
        //    else
        //    {
        //        time_sturn += 6.0f;
        //        Arrow_sturn = false;
        //    }

        //}

    }
    void Attacking()//몬스터가 공격하는중이면 
    {
        target.GetComponent<HP_M>().dmg_HP(Atk);//그 오브젝트의 hp 매니져에 공격력만큼  체력에 뺀다.
        Audio_play();
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");

        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))//버프 장판에서 나갈 경우
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
            if (nav.speed != Speed)
                nav.speed -= Speed_up;
        }


    }
    private void OnTriggerStay(Collider other)//닿고있으면
    {

        // 버프 스킬을 굳이 스테이 에다가 한번더 넣는 이유는 || 떄문이다.
        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))// 버프 장판 안에 있으면 대미지 증가
        {
            if (Atk == Atk_ex)// ATK가 처음 정해진 공격력이랑 같으면
                Atk += ATK_UP;//공격력 증가
            if (nav.speed == Speed)//네브매쉬의 속도가 변수 스피드랑 같으면
                nav.speed += Speed_up;//스피드업을 네브매쉬의 스피드에 더해준다
        }
        //if (other.gameObject.CompareTag("Tower_type_1"))
        //{
        //    nav.SetDestination(other.gameObject.transform.position);//공격하는 타워가 근처에 있으면 타겟을 그 타워로 바꾼다.
        //    target = other.gameObject;//타겟은 Tower_type_1의 태그를 가진 오브젝트이다
        //    Atk_ON = true;
        //}
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//플레이어의 타워를 타겟으로 삼는다.
            Atk_ON = true;
        }


    }

    public void Damaging(float _damage)
    {
        EnemyHP -= _damage;//0<hp
        if (EnemyHP > 0)
        {
            if (sound_change == false)
            {
                adi_s.clip = Hit_0;
                sound_change = true;
            }
            else
            {
                adi_s.clip = Hit_1;
                sound_change = false;
            }
        }
        Audio_play();
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

    /*public void monstermoving()
    {
        this.nav.speed = Speed;
    }*/

}
