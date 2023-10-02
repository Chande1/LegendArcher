using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AI_Tower_destroyer_Enemy : MonoBehaviour
{
    //Ÿ�� ������ ���Ϳ� ��ũ��Ʈ
    GameObject gm;
    public NavMeshAgent nav;//�׺�Ž� 
    private Transform Player;//Ÿ�� �� Ÿ��
    public AnimationClip Atk_anime;//���ݾִϸ��̼�
    public HP_M hp;//ü�� �Ŵ���
    public float EnemyHP = 10f;
    Tower_fire T_F_M;
    public GameObject target;//�����Ҵ��

    public float Atk = 1;// ���ݷ�
    public float Atk_ex = 1;// ���ݷ��� ���� �� (������ ������ ���� ���·� ���ư��� ����)
    public float ATK_UP = 5f;//������ ������ �����ϴ� ���ݷ��� ��
    float ATK_delay = 3f;//���� ������ 
    float ATK_delay_re_save = 0f;//���ݵ����� �ʱ�ȭ�� ����
    public float Atk_length = 2.5f;//���� ��Ÿ�
    bool Atk_ON = false;//���� ����Ī�� ����
    public float dead_Time;

    AudioSource audio_s;
    public AudioClip ATk1;
    public AudioClip ATk2;
    public AudioClip dead1;
    public AudioClip dead2;
    public AudioClip hit1;
    public AudioClip hit2;
    int Rm = 0;
    public float Speed = 2f;//�̼�
    public float Speed_up = 2f;//���ǵ� ����
    public bool attack = true; //���ݹ޴���
    public bool stiff = false; //����
    public float nukBack_B = 0.5f;

    Animator ani;


    [Header("�ް� �ִ� ��ų�� ����")]
    public Skill s;
    public SkillValue sv;
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<���� ������ AWAke>Start>update ���̴�.>>>>>>>>>>>>>>>>>>>>>>>>

    // Start is called before the first frame update
    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        Atk_ex = Atk;//���� �����ϰ�
        Atk = Atk_ex;//�����ϰ�
        hp = GetComponent<HP_M>();
        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//�÷��̾��� ��ġ�� ĳ���̶�� �±׸� ���� ������Ʈ���Լ� �����´�.
            this.nav = GetComponent<NavMeshAgent>();//�׺�޽� ����.
            this.nav.destination = Player.transform.position;//�׺�Ž��� �������� �÷��̾�(Ÿ��)�� �ִ� ��ġ�̴�.
            this.nav.autoBraking = false;//autoBraking�� ����� �������� ��������� �ӵ��� ���� �Ǵ°��̴�.
            target = Player.gameObject;//��ǥ ����(������)
        }
       
        ATK_delay_re_save = Atk_anime.length;
        ATK_delay = Atk_anime.length;

    }
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Tower_AI"))
        {
            for (int i = 0; i < 2; i++)
            {
                T_F_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                T_F_M.GM_IN(gameObject);//�ڱ� ������ Ÿ������ �ϴ� �ش�
            }
        }
        

        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
    }

    // Update is called once per frame
    void FixedUpdate()//�� ������ ���� �����Ǵ°�
    {
        
        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
        if (stiff == false&&Player)          //������ �ƴ϶��
        {
            this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�//d
            ani.speed = 1f;

            if (ani.GetBool("ATK") != false)
                ani.SetBool("ATK", false);
          
            if (nav.destination != null)
                this.nav.destination = Player.transform.position;//�� Ÿ���� hp�� 0���� ����� �������� �ٽ� �÷��̾�(Ÿ��)�� �ٲ۴�.  
            if (nav.remainingDistance <= nav.stoppingDistance)//Ÿ�ٰ��� �Ÿ��� 2���� ������ 
            {
                if(target)
                {
                    Vector3 relativePos = target.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = rotation;
                }
                

                ATK_delay -= Time.deltaTime;//���� �����̸� �ٿ��ش� 
                if (ATK_delay <= 0)// ���� �����̰� 0���� �Ʒ��� �Ǹ�
                {
                    Attacking();//���� �Լ��� �ߵ�
                    ATK_delay = ATK_delay_re_save;//�ٽ� �ʱ�ȭ
                }
            }


        }
        else if (stiff == true)
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

            self_Out();
            Invoke("ks", 1f);
            nav.ResetPath();
            this.gameObject.tag = "Untagged";
            ani.SetBool("die", true);
            Destroy(this.gameObject, dead_Time);
            attack = false;
        }





    }

    void self_Out()//����
    {
        T_F_M.Gm_Out(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)//��ų���̳� Ÿ�� ���� ��ũ��Ʈ�� 
    {
        //if (other.gameObject.CompareTag("AI_ common"))//������ ���߰�
        //{
        //    //Debug.Log("d");
        //    nav.isStopped = true;
        //}
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            Debug.Log("d");
            target = other.gameObject;//�÷��̾��� Ÿ���� Ÿ������ ��´�.
            ani.SetBool("ATK", true);
            Atk_ON = true;
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            this.nav.speed = 0;
            Vector3 VFirst;//������
            Vector3 VLast;// �ڽ�
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
    void Attacking()//���Ͱ� �����ϴ����̸� 
    {
        target.GetComponent<HP_M>().dmg_HP(Atk);
        Audio_play();
    }

    void Audio_play()
    {
        Rm = Random.Range(0, 2);
        //����
        if (Rm == 0 && EnemyHP > 0 && ATK_delay < 0)
        {
            audio_s.clip = ATk1;

        }
        else if ((Rm == 1 && EnemyHP > 0) && ATK_delay < 0)
        {

            audio_s.clip = ATk2;
        }
        //�׾�����
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

    private void OnTriggerStay(Collider other)//���������
    {

        // ���� ��ų�� ���� ������ ���ٰ� �ѹ��� �ִ� ������ || �����̴�.
        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))// ���� ���� �ȿ� ������ ����� ����
        {
            if (Atk == Atk_ex)// ATK�� ó�� ������ ���ݷ��̶� ������
                Atk += ATK_UP;//���ݷ� ����
            if (nav.speed == Speed)//�׺�Ž��� �ӵ��� ���� ���ǵ�� ������
                nav.speed += Speed_up;//���ǵ���� �׺�Ž��� ���ǵ忡 �����ش�
            // nav.speed �� ����� �������� ���µ� �ɸ��� �ӵ��� �����ϴ°��̴�.
        }
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//�÷��̾��� Ÿ���� Ÿ������ ��´�.
            Atk_ON = true;
            ani.SetBool("ATK", true);
        }

    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))//���� ���ǿ��� ���� ���
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
            if (nav.speed != Speed)
                nav.speed -= Speed_up;
        }
        //if (other.gameObject.CompareTag("AI_ common"))//�տ� ������ ���߰� �Ҷ�� ���� �ߴ���
        //{
        //    Debug.Log("d2");
        //    nav.isStopped = false;
        //}


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
    public bool GetAtk_ON()// �����ϴ� ������ �ܺο��� Ȯ���ϴ� �Լ�
    {
        if (Atk_ON)
            return true;
        else
            return false;
    }
}