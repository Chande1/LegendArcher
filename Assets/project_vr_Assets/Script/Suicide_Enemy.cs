using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Suicide_Enemy : MonoBehaviour
{
    public HP_M hp;//체력 매니저
    public float EnemyHP = 5f;
    public float Speed = 1f;
    public NavMeshAgent nav;
    public Transform Target;
    public float Atk = 2f;
    bool Atk_Time = false;
    float Anime_Time = 1f;//애니메이션 시간에 따라 조정해야함 원래5f
    public GameObject boomPrefeb;
    Animator ani;
    int Rm_int = 0;
    public float Speed_up = 5f;//스피드 증가
    public float Atk_ex = 1f;// 공격력의 기존 값 (버프가 끝나고 원래 상태로 돌아갈떄 쓰임)
    public float ATK_UP = 5f;//버프를 받으면 증가하는 공격력의 양
    bool Atk_ON = false;//공격 스위칭용 변수
    public bool attack = true; //공격받는중
    public bool stiff = false; //경직
    public float nukBack_B = 0.5f;
    AudioSource adi_s;
    public AudioClip spawn;
    public AudioClip spawn_1;
    public AudioClip ATK;
    public AudioClip dead;
    public AudioClip dead1;
    [Header("받고 있는 스킬의 종류")]
    public Skill s;
    public SkillValue sv;
    int A = 0;
    public float dead_Time = 0f;



    private void Awake()
    {
        adi_s = GetComponent<AudioSource>();
        Rm_int = Random.Range(0, 2);//0,1
        if (Rm_int > 0)
        {
            adi_s.clip = spawn;
        }
        else
        {
            adi_s.clip = spawn_1;
        }

        ani = this.GetComponent<Animator>();
        if(GameObject.FindWithTag("PlayerTower"))
        {
            Target = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();
            nav = this.GetComponent<NavMeshAgent>();
            hp = this.gameObject.GetComponent<HP_M>();
            this.nav.speed = Speed;//몬스터의 이동속도는 1f이다
        }
      
        hp.HP_save(EnemyHP);//체력 설정
    }


    private void Start()
    {
        adi_s.Play();
    }

    void Update()
    {

        hp.HP_save(EnemyHP);//매순간 체력을 가져오기 위해
        if (stiff == false)          //경직이 아니라면
        {
            ani.speed = 1f;

            this.nav.speed = Speed;//몬스터의 이동속도는 1f이다
            if (Atk_Time)
                Anime_Time -= Time.deltaTime;

            //nav.SetDestination(Target.transform.position);
            if (nav.destination != null)
                this.nav.destination = Target.transform.position;//네브매쉬의 목적지는 플레이어(타워)가 있는 위치이다.
            if (Anime_Time <= 0 || EnemyHP <= 0)
            {

                if (Anime_Time < 0)
                {
                    Attack();//공격하고 죽기
                    EnemyHP = 0;//hp=0으로
                    Anime_Time = 6000f;

                    //Instantiate(boom, this.gameObject.transform.position, Quaternion.identity);//폭팔 이펙트
                }

                //죽는 애니메이션
                //Destroy(this.gameObject, 4f);//애니메이션 시간 끝나고 N초후 삭제



            }

        }
        if (EnemyHP <= 0&&attack)
        {
            Invoke("ks", 1f);
            nav.ResetPath();
            this.gameObject.tag = "Untagged";
            ani.SetBool("Die", true);
            Destroy(this.gameObject, dead_Time);
            attack = false;
        }





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
    }

    void Attack()
    {
        Atk_ON = true;
        adi_s.clip = ATK;
        adi_s.Play();
        Target.GetComponent<HP_M>().dmg_HP(Atk);//그 오브젝트의 hp 매니져에 공격력만큼  체력에 뺀다.
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
            // nav.speed 는 대상이 목적지에 가는데 걸리는 속도를 조절하는것이다.
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTower"))
        {
            Atk_Time = true;
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


    public void Damaging(float _damage)
    {
        EnemyHP -= _damage;
        if (EnemyHP <= 0)
        {

            A = Random.Range(0, 2);
            if (A > 0)
                adi_s.clip = dead;
            else
            {
                adi_s.clip = dead1;
            }
            adi_s.Play();
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

    public bool GetAtk_ON()// 공격하는 중인지 외부에서 확인하는 함수
    {
        if (Atk_ON)
            return true;
        else
            return false;
    }
    public void ks()
     {
         WaveManager wavemanager = GameObject.Find("UI_Manager").GetComponent<WaveManager>();
         wavemanager.KillScore++;
     }

}