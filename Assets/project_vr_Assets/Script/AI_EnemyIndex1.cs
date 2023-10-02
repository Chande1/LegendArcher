using UnityEngine;
using UnityEngine.AI;


public class AI_EnemyIndex1 : MonoBehaviour
{
    NavMeshAgent nav;//네브매쉬 
    private Transform Player;//타겟 즉 타워

    public HP_M hp;//체력 매니져
    public float EnemyHP = 100;//몬스터 체력 설정
    public GameObject target;//공격할대상

    public float Atk = 1;// 공격력
    public float Atk_ex = 1;// 공격력의 기존 값 (버프가 끝나고 원래 상태로 돌아갈떄 쓰임)
    public float ATK_UP = 5f;//버프를 받으면 증가하는 공격력의 양
    public float ATK_delay = 3f;//공격 딜레이 
    public float ATK_deal_re_save = 3f;//공격딜레이 초기화용 변수
    bool Atk_ON = false;//공격 스위칭용 변수
    public float Speed=3f;//이속
    public float Speed_up = 5f;//스피드 증가
    bool Enemy_dmg_01;//상태이상용 변수
    bool Atk_anime_check = false;//애니메이션 전환용 불형태의 변수
    float time_sturn=0;//스턴용 
    bool Arrow_sturn=false;//3.0f 스턴 구분용
    bool Arrow_sturn_2 = false;//6.0f 스턴 구분용

    Animator ani;//애니메이터
   
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<빌드 순서는 AWAke>Start>update 순이다.>>>>>>>>>>>>>>>>>>>>>>>>

    // Start is called before the first frame update
    private void Awake()
    {
        //Debug.Log(EnemyHP);
        Atk_ON = false;
        this.ani = GetComponent<Animator>();//가져오기
        Atk_ex = Atk;//값을 동일하게
        Atk = Atk_ex;//동일하게
        hp = GetComponent<HP_M>();
    }
    private void Start()
    {
    }
    private void FixedUpdate()
    {
        hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기
        if (time_sturn>0)
        {
            time_sturn -= Time.deltaTime;
            this.ani.StopPlayback();//애니메이션이 멈춘다
        }

        if (time_sturn<=0)
        {
            anime_ON();//애니메이션 출용 용도

            /*Vector3 relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;*/
            if (Atk_ON == true)
            {

                if (nav.remainingDistance <= nav.stoppingDistance)//타겟과의 거리가 2보다 작으면 
                {
                    Atk_anime_check = true;//공격 애니메이션 발동
                    ATK_delay -= Time.deltaTime;//공격 딜레이를 줄여준다 
                    if (ATK_delay <= 0)// 공격 딜레이가 0보다 아래가 되면
                    {

                        ATK_delay = ATK_deal_re_save;//다시 초기화
                        Attacking();//공격 함수를 발동
                    }
                }

                if (target.GetComponent<HP_M>().Hp <= 0)
                {
                    Atk_ON = false;//공격을 false로 지정
                    this.nav.destination = Player.transform.position;//그 타워의 hp를 0으로 만들면 목적지를 다시 플레이어(타워)로 바꾼다.
                    Atk_anime_check = false;//떄리는 모션 비활성화
                }

            }

            /*
            if (hp.Hp == 0)
            {
                Destroy(this.gameObject);//hp가 0이면 게임 오브젝트 삭제
            }
            */
        }
    }
    // Update is called once per frame
    void Update()//매 프레임 마다 가동되는것
    {
        hp.HP_save(EnemyHP);//hp매니저에 몬스터 체력 넣기
        Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();
        this.nav = GetComponent<NavMeshAgent>();//네브메쉬 지정.
        this.nav.destination = Player.transform.position;//네브매쉬의 목적지는 플레이어(타워)가 있는 위치이다.
        this.nav.speed = Speed;//몬스터의 이동속도는 1f이다
        this.nav.autoBraking = false;//autoBraking은 대상이 목적지에 가까워지면 속도가 감소 되는것이다.
        //그러한것들을 막기위해 false로 지정해준다.
        this.nav.stoppingDistance = 1f;//네브매쉬의 멈출거리


    }



    private void anime_ON()//애니메이션 출연 용도
    {

        if(Atk_anime_check==true)//공격 애니메이션
        {
            ani.SetBool("Attack",true);//공격하는 애니메이션 발동
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

            Atk_ON = true;
        }
        /*
        if(other.gameObject.CompareTag("Bullet"))// 플레이어의 공격 태그 넣어야함
          {
          hp.dmg_HP(5f);//대미지 받을 수치하고
          }
        */
        if (other.gameObject.CompareTag("Bullet"))
        {
            EnemyHP -= 5f;

            if (EnemyHP <= 0)
            {
                WaveManager wavemanager = GameObject.Find("GameManager").GetComponent<WaveManager>();
                wavemanager.KillScore++;

                ani.SetBool("die", true);

                //몬스터의 HP가 0이거나 0이더크면 비활성화시킨다.
                this.gameObject.SetActive(false); 
                //Destroy(this.gameObject, 3f);
            }


        }

        if (other.gameObject.CompareTag("ice_Arrow"))
        {
            Arrow_sturn = true; 
            if(Arrow_sturn_2==true)
            {
                time_sturn += 6.0f;
                Arrow_sturn_2 = false;
            }else
            {
                time_sturn = 3.0f;
                Arrow_sturn_2 = false;
            }

        }
        if(other.gameObject.CompareTag("ice_Arrow_2"))
        {
            Arrow_sturn_2 = true;
            if (Arrow_sturn == true)
            {
                time_sturn += 3.0f;
                Arrow_sturn= false;
            }
            else
            {
                time_sturn += 6.0f;
                Arrow_sturn = false;
            }

        }

    }
    void Attacking()//몬스터가 공격하는중이면 
    {
    
        target.GetComponent<HP_M>().dmg_HP(Atk);//그 오브젝트의 hp 매니져에 공격력만큼  체력에 뺀다.
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");
      
        if (other.gameObject.CompareTag("Skill_damage up")|| other.gameObject.CompareTag("Skill_damge up_02"))//버프 장판에서 나갈 경우
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
        }
        if(other.gameObject.CompareTag("Skill_move_speed up")|| other.gameObject.CompareTag("skill_move_speed up_02"))//이속 증가 버프에서 빠져나오면
        {
            if(nav.speed!= Speed)
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
        }
        if (other.gameObject.CompareTag("Skill_move_speed up") || other.gameObject.CompareTag("skill_move_speed up_02"))//이속 증가 버프를 받으면
        {
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

}
