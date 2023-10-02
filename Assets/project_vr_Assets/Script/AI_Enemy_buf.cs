using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//�׺�Ž� ����ҷ��� �ʿ���

public class AI_Enemy_buf : MonoBehaviour
{
    //���� ���Ϳ� ��ũ��Ʈ
    public NavMeshAgent nav;//�׺�Ž� 
    public AnimationClip Atk_anime;//���ݾִϸ��̼�
    public HP_M hp;//ü�� �Ŵ���
    public float EnemyHP = 100;//���� ü�� ����

    Tower_fire T_F_M;//���� Ÿ��
    private Transform Player;//Ÿ�� �� Ÿ��
    public GameObject target;//�����Ҵ��
    AudioSource audio_s;
    public AudioClip ATk1;
    public AudioClip ATk2;
    public AudioClip dead1;
    public AudioClip dead2;
    public AudioClip hit1;
    public AudioClip hit2;
    int Rm = 0;
    public float Atk = 1;// ���ݷ�
    public float Atk_ex = 1;// ���ݷ��� ���� �� (������ ������ ���� ���·� ���ư��� ����)
    public float ATK_UP = 5f;//������ ������ �����ϴ� ���ݷ��� ��
    public float ATK_delay = 3f;//���� ������ 
    float ATK_deal_re_save = 0f;//���ݵ����� �ʱ�ȭ�� ����
    public float Atk_length = 2f;//���� ��Ÿ�
    bool Atk_ON = false;//���� ����Ī�� ����
    public float Speed = 2f;//�̼�
    public float Speed_up = 5f;//���ǵ� ���� ������ ������ �����ϴ� ��
    public float Speed_A = 0;
    public float nukBack_B = 0.5f;
    public bool attack = true; //���ݹ޴���
    public bool stiff = false; //����
    public float dead_Time;

    [Header("�ް� �ִ� ��ų�� ����")]
    public Skill s;
    public SkillValue sv;


    Animator ani;//�ִϸ�����

    // Start is called before the first frame update
    private void Awake()
    {
        audio_s = GetComponent<AudioSource>();
        Speed_A = Speed;//���ǵ��� �������� �����ϱ� ����
        hp = GetComponent<HP_M>();//ü�� �Ŵ���
        ani = GetComponent<Animator>();
        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//�÷��̾��� ��ġ�� ĳ���̶�� �±׸� ���� ������Ʈ���Լ� �����´�.
            this.nav = GetComponent<NavMeshAgent>();//�׺�޽� ����.
            this.nav.destination = Player.transform.position;//�׺�Ž��� �������� �÷��̾�(Ÿ��)�� �ִ� ��ġ�̴�.
            this.nav.autoBraking = false;//autoBraking�� ����� �������� ��������� �ӵ��� ���� �Ǵ°��̴�.
                                         //this.nav.stoppingDistance = Atk_length;//�׺�Ž��� ����Ÿ�
            this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�s
        }
      

    }
    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            if(GameObject.FindGameObjectWithTag("Tower_AI"))
            {
                T_F_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                T_F_M.GM_IN(gameObject);//�ڱ� ������ Ÿ������ �ϴ� �ش�
            }
          
        }
        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
        T_F_M.GM_IN(this.gameObject);//�ڱ� �ֱ�
        ATK_deal_re_save = ATK_delay;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        hp.HP_save(EnemyHP);

        if(this.nav)
            this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�s
        if (stiff == false&&Player)          //������ �ƴ϶��
        {
            if (nav.destination != null)
                this.nav.destination = Player.transform.position;//�� Ÿ���� hp�� 0���� ����� �������� �ٽ� �÷��̾�(Ÿ��)�� �ٲ۴�.  




            ani.SetBool("Atk", false);// ���� �ִϸ��̼� 

            if (Atk_ON == true)
            {


                if (nav.remainingDistance <= nav.stoppingDistance&&target)//Ÿ�ٰ��� �Ÿ��� 2���� ������ 
                {
                    Vector3 relativePos = target.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    transform.rotation = rotation;
                    ani.speed /= ATK_deal_re_save;
                    ani.SetBool("Atk", true);// ���� �ִϸ��̼� �ߵ�
                    ATK_delay -= Time.deltaTime;//���� �����̸� �ٿ��ش� 
                    if (ATK_delay <= 0)// ���� �����̰� 0���� �Ʒ��� �Ǹ�
                    {


                        Attacking();//���� �Լ��� �ߵ�
                        ATK_delay = ATK_deal_re_save;//�ٽ� �ʱ�ȭ
                    }
                }

                if (target.GetComponent<HP_M>().Hp <= 0)
                {
                    Atk_ON = false;//������ false�� ����
                    this.nav.destination = Player.transform.position;//�� Ÿ���� hp�� 0���� ����� �������� �ٽ� �÷��̾�(Ÿ��)�� �ٲ۴�.  
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
    void Attacking()//���Ͱ� �����ϴ����̸� 
    {
        target.GetComponent<HP_M>().dmg_HP(Atk);//�� ������Ʈ�� hp �Ŵ����� ���ݷ¸�ŭ  ü�¿� ����.
        Audio_play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//�÷��̾��� Ÿ���� Ÿ������ ��´�.
            Atk_ON = true;
            ATK_deal_re_save = Atk_anime.length;
            ATK_delay = Atk_anime.length;
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

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))// ���� ���� �ȿ� ������ ����� ����
        {
            if (Atk == Atk_ex)// ATK�� ó�� ������ ���ݷ��̶� ������
                Atk += ATK_UP;//���ݷ� ����
            if (nav.speed == Speed)//�׺�Ž��� �ӵ��� ���� ���ǵ�� ������
                nav.speed += Speed_up;//���ǵ���� �׺�Ž��� ���ǵ忡 �����ش�
            // nav.speed �� ����� �������� ���µ� �ɸ��� �ӵ��� �����ϴ°��̴�.
        }
        if (other.gameObject.CompareTag("Tower_type_1"))
        {
            nav.SetDestination(other.gameObject.transform.position);//�����ϴ� Ÿ���� ��ó�� ������ Ÿ���� �� Ÿ���� �ٲ۴�.
            target = other.gameObject;//Ÿ���� Tower_type_1�� �±׸� ���� ������Ʈ�̴�
            Atk_ON = true;
        }
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//�÷��̾��� Ÿ���� Ÿ������ ��´�.
            Atk_ON = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        /*
        if (this.gameObject.CompareTag("AI_buf") && other.gameObject.CompareTag("skill_move_speed up_02"))
        {
            if (nav.speed != Speed)//���ǵ尡 ����������
                nav.speed -= Speed_up;//���ǵ带 ���ش�.
            // nav.speed �� ����� �������� ���µ� �ɸ��� �ӵ��� �����ϴ°��̴�.
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
            // nav.speed �� ����� �������� ���µ� �ɸ��� �ӵ��� �����ϴ°��̴�.
        }

        if (this.gameObject.CompareTag("AI_buf_2") && other.gameObject.CompareTag("Skill_damge up"))
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
        }
        */
        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))//���� ���ǿ��� ���� ���
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

    public bool GetAtk_ON()// �����ϴ� ������ �ܺο��� Ȯ���ϴ� �Լ�
    {
        if (Atk_ON)
            return true;
        else
            return false;
    }
}
