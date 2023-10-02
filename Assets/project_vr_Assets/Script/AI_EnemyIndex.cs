using UnityEngine;
using UnityEngine.AI;


public class AI_EnemyIndex : MonoBehaviour
{
    public NavMeshAgent nav;//�׺�Ž� 
    private Transform Player;//Ÿ�� �� Ÿ��
    public AnimationClip Atk_anime;//���� �ִϸ��̼� �ð� �������� ���� ��� �ִϸ����Ϳ��� Ŭ�� �������°� �߸���..
    public HP_M hp;//ü�� �Ŵ���
    public float EnemyHP = 100;//���� ü�� ����
    public GameObject target;//�����Ҵ��
    Tower_fire T_f_M;//Ÿ���� �� �ٶ�� ���

    public float Atk = 1f;// ���ݷ�
    public float Atk_ex = 1f;// ���ݷ��� ���� �� (������ ������ ���� ���·� ���ư��� ����)
    public float ATK_UP = 5f;//������ ������ �����ϴ� ���ݷ��� ��
    public float ATK_delay = 3f;//���� ������ 
    float ATK_deal_re_save = 0f;//���ݵ����� �ʱ�ȭ�� ����
    bool Atk_ON = false;//���� ����Ī�� ����
    public float Speed = 3f;//�̼�
    public float Speed_up = 5f;//���ǵ� ����
    bool Enemy_dmg_01;//�����̻�� ����
    bool Atk_anime_check = false;//�ִϸ��̼� ��ȯ�� �������� ����
    float time_sturn = 0;//���Ͽ� 
    bool Arrow_sturn = false;//3.0f ���� ���п�
    bool Arrow_sturn_2 = false;//6.0f ���� ���п�
    public float deadtime;

    public float nukBack_B = 0.5f;

    Animator ani;//�ִϸ�����
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

    public bool attack = true; //���ݹ޴���
    public bool stiff = false; //����


    [Header("�ް� �ִ� ��ų�� ����")]
    public Skill s;
    public SkillValue sv;
    /*
    [Space(10f)]
    [Header("�� ������ ����")]
    [Header("����")]
    [SerializeField] bool horizontal = false;   //����
    [SerializeField] float hspeed = 1f;      //���� ������ �ӵ�
    [SerializeField] float hdistance = 1f;  //���� ������ �Ÿ�
    [Space(10f)]
    [Header("����")]
    [SerializeField] bool verticality = false;   //����
    [SerializeField] float vspeed = 1f;      //���� ������ �ӵ�
    [SerializeField] float vdistance = 1f;  //���� ������ �Ÿ�
    [Header("�������")]
    [SerializeField] bool frontmode = false;   //����
    [SerializeField] float fmspeed = 1f;      //���� ��� ������ �ӵ�*/

    //

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<���� ������ AWAke>Start>update ���̴�.>>>>>>>>>>>>>>>>>>>>>>>>

    // Start is called before the first frame update

    private void Awake()
    {
        adi_s = GetComponent<AudioSource>();
        //Debug.Log(EnemyHP);
        Atk_ON = false;
        this.ani = GetComponent<Animator>();//��������
        Atk_ex = Atk;//���� �����ϰ�
        Atk = Atk_ex;//�����ϰ�
        hp = GetComponent<HP_M>();
        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();
            this.nav = GetComponent<NavMeshAgent>();//�׺�޽� ����.
                                                    //this.nav.destination = Player.transform.position;//�׺�Ž��� �������� �÷��̾�(Ÿ��)�� �ִ� ��ġ�̴�.
                                                    //this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�
            this.nav.autoBraking = false;//autoBraking�� ����� �������� ��������� �ӵ��� ���� �Ǵ°��̴�.
                                         //�׷��Ѱ͵��� �������� false�� �������ش�.
        }

        ATK_deal_re_save = ATK_delay;
        //this.nav.stoppingDistance = 4.5f;//�׺�Ž��� ����Ÿ�
    }
    private void Start()
    {
        adi_s = GetComponent<AudioSource>();
        for (int i = 0; i < 2; i++)
        {
            if (GameObject.FindGameObjectWithTag("Tower_AI"))    //���� Ÿ���� ���� ��� ���
            {
                T_f_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                T_f_M.GM_IN(gameObject);//�ڱ� ������ Ÿ������ �ϴ� �ش�
            }

        }

        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
        //this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�

    }

    private void Update()
    {
        //Debug.Log(stiff);
        if (stiff == false)          //������ �ƴ϶��
        {
            ani.speed = 1f;
            if (EnemyHP > 0)
            {
                if (ani.GetBool("ATK") != false)
                    ani.SetBool("ATK", false);

                if (nav.destination != null)
                    this.nav.destination = Player.transform.position;//�׺�Ž��� �������� �÷��̾�(Ÿ��)�� �ִ� ��ġ�̴�.
            }


            if (time_sturn > 0)
            {
                time_sturn -= Time.deltaTime;
                this.ani.StopPlayback();//�ִϸ��̼��� �����
            }


            if (time_sturn <= 0)
            {
                anime_ON();//�ִϸ��̼� ��� �뵵
                hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
                this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�

                if (Atk_ON == true)
                {
                    if (nav.remainingDistance <= nav.stoppingDistance&&target)//Ÿ�ٰ��� �Ÿ��� 2���� ������ 
                    {
                        Vector3 relativePos = target.transform.position - transform.position;
                        Quaternion rotation = Quaternion.LookRotation(relativePos);
                        transform.rotation = rotation;

                        ani.speed /= ATK_deal_re_save;
                        ATK_delay -= Time.deltaTime;//���� �����̸� �ٿ��ش�
                        Atk_anime_check = true;//���� �ִϸ��̼� �ߵ�

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
                        Atk_anime_check = false;//������ ��� ��Ȱ��ȭ
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
                if (GameObject.FindGameObjectWithTag("Tower_AI"))    //���� Ÿ���� ���� ��� ���
                {
                    T_f_M = GameObject.FindGameObjectsWithTag("Tower_AI")[i].GetComponent<Tower_fire>();
                    T_f_M.Gm_Out(this.gameObject);//�ڱ� ������ Ÿ������ �ϴ� �ش�
                }

            }
            //T_f_M.Gm_Out(this.gameObject);
            this.gameObject.tag = "Untagged";
            this.nav.speed = 0;//������ �̵��ӵ��� 1f�̴�
            Invoke("ks", 1f);
            nav.ResetPath();
            ani.SetBool("die", true);
            Destroy(this.gameObject, deadtime);
            attack = false;
        }
    }


    void Audio_play()
    {
        //����
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
        //�׾�����
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


    private void anime_ON()//�ִϸ��̼� �⿬ �뵵
    {

        if (Atk_anime_check == true)//���� �ִϸ��̼�
        {
            ani.SetBool("Attack", true);//�����ϴ� �ִϸ��̼� �ߵ�

        }

        if (Atk_anime_check == false)//�������� �ƴ϶�� 
        {
            ani.SetBool("Attack", false);// �ȴ� �ִϸ��̼� �ߵ�
        }

    }

    private void OnTriggerEnter(Collider other)//��ų���̳� Ÿ�� ���� ��ũ��Ʈ�� 
    {
        //Debug.Log("OnTriggerEnter");
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//�÷��̾��� Ÿ���� Ÿ������ ��´�.
            //ATK_deal_re_save = Atk_anime.length;
            //ATK_delay = Atk_anime.length;
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
        /*
        if(other.gameObject.CompareTag("Bullet"))// �÷��̾��� ���� �±� �־����
          {
          hp.dmg_HP(5f);//����� ���� ��ġ�ϰ�
          }
        */
        //�Ȼ�� �ǰ����� ���� ���Ͽ뵵
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
    void Attacking()//���Ͱ� �����ϴ����̸� 
    {
        target.GetComponent<HP_M>().dmg_HP(Atk);//�� ������Ʈ�� hp �Ŵ����� ���ݷ¸�ŭ  ü�¿� ����.
        Audio_play();
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");

        if (other.gameObject.CompareTag("Skill_damage up") || other.gameObject.CompareTag("Skill_damge up_02"))//���� ���ǿ��� ���� ���
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
            if (nav.speed != Speed)
                nav.speed -= Speed_up;
        }


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
        }
        //if (other.gameObject.CompareTag("Tower_type_1"))
        //{
        //    nav.SetDestination(other.gameObject.transform.position);//�����ϴ� Ÿ���� ��ó�� ������ Ÿ���� �� Ÿ���� �ٲ۴�.
        //    target = other.gameObject;//Ÿ���� Tower_type_1�� �±׸� ���� ������Ʈ�̴�
        //    Atk_ON = true;
        //}
        if (other.gameObject.CompareTag("PlayerTower"))
        {
            target = other.gameObject;//�÷��̾��� Ÿ���� Ÿ������ ��´�.
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

    public bool GetAtk_ON()// �����ϴ� ������ �ܺο��� Ȯ���ϴ� �Լ�
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
