using UnityEngine;
using UnityEngine.AI;


public class AI_EnemyIndex1 : MonoBehaviour
{
    NavMeshAgent nav;//�׺�Ž� 
    private Transform Player;//Ÿ�� �� Ÿ��

    public HP_M hp;//ü�� �Ŵ���
    public float EnemyHP = 100;//���� ü�� ����
    public GameObject target;//�����Ҵ��

    public float Atk = 1;// ���ݷ�
    public float Atk_ex = 1;// ���ݷ��� ���� �� (������ ������ ���� ���·� ���ư��� ����)
    public float ATK_UP = 5f;//������ ������ �����ϴ� ���ݷ��� ��
    public float ATK_delay = 3f;//���� ������ 
    public float ATK_deal_re_save = 3f;//���ݵ����� �ʱ�ȭ�� ����
    bool Atk_ON = false;//���� ����Ī�� ����
    public float Speed=3f;//�̼�
    public float Speed_up = 5f;//���ǵ� ����
    bool Enemy_dmg_01;//�����̻�� ����
    bool Atk_anime_check = false;//�ִϸ��̼� ��ȯ�� �������� ����
    float time_sturn=0;//���Ͽ� 
    bool Arrow_sturn=false;//3.0f ���� ���п�
    bool Arrow_sturn_2 = false;//6.0f ���� ���п�

    Animator ani;//�ִϸ�����
   
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<���� ������ AWAke>Start>update ���̴�.>>>>>>>>>>>>>>>>>>>>>>>>

    // Start is called before the first frame update
    private void Awake()
    {
        //Debug.Log(EnemyHP);
        Atk_ON = false;
        this.ani = GetComponent<Animator>();//��������
        Atk_ex = Atk;//���� �����ϰ�
        Atk = Atk_ex;//�����ϰ�
        hp = GetComponent<HP_M>();
    }
    private void Start()
    {
    }
    private void FixedUpdate()
    {
        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
        if (time_sturn>0)
        {
            time_sturn -= Time.deltaTime;
            this.ani.StopPlayback();//�ִϸ��̼��� �����
        }

        if (time_sturn<=0)
        {
            anime_ON();//�ִϸ��̼� ��� �뵵

            /*Vector3 relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = rotation;*/
            if (Atk_ON == true)
            {

                if (nav.remainingDistance <= nav.stoppingDistance)//Ÿ�ٰ��� �Ÿ��� 2���� ������ 
                {
                    Atk_anime_check = true;//���� �ִϸ��̼� �ߵ�
                    ATK_delay -= Time.deltaTime;//���� �����̸� �ٿ��ش� 
                    if (ATK_delay <= 0)// ���� �����̰� 0���� �Ʒ��� �Ǹ�
                    {

                        ATK_delay = ATK_deal_re_save;//�ٽ� �ʱ�ȭ
                        Attacking();//���� �Լ��� �ߵ�
                    }
                }

                if (target.GetComponent<HP_M>().Hp <= 0)
                {
                    Atk_ON = false;//������ false�� ����
                    this.nav.destination = Player.transform.position;//�� Ÿ���� hp�� 0���� ����� �������� �ٽ� �÷��̾�(Ÿ��)�� �ٲ۴�.
                    Atk_anime_check = false;//������ ��� ��Ȱ��ȭ
                }

            }

            /*
            if (hp.Hp == 0)
            {
                Destroy(this.gameObject);//hp�� 0�̸� ���� ������Ʈ ����
            }
            */
        }
    }
    // Update is called once per frame
    void Update()//�� ������ ���� �����Ǵ°�
    {
        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�
        Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();
        this.nav = GetComponent<NavMeshAgent>();//�׺�޽� ����.
        this.nav.destination = Player.transform.position;//�׺�Ž��� �������� �÷��̾�(Ÿ��)�� �ִ� ��ġ�̴�.
        this.nav.speed = Speed;//������ �̵��ӵ��� 1f�̴�
        this.nav.autoBraking = false;//autoBraking�� ����� �������� ��������� �ӵ��� ���� �Ǵ°��̴�.
        //�׷��Ѱ͵��� �������� false�� �������ش�.
        this.nav.stoppingDistance = 1f;//�׺�Ž��� ����Ÿ�


    }



    private void anime_ON()//�ִϸ��̼� �⿬ �뵵
    {

        if(Atk_anime_check==true)//���� �ִϸ��̼�
        {
            ani.SetBool("Attack",true);//�����ϴ� �ִϸ��̼� �ߵ�
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

            Atk_ON = true;
        }
        /*
        if(other.gameObject.CompareTag("Bullet"))// �÷��̾��� ���� �±� �־����
          {
          hp.dmg_HP(5f);//����� ���� ��ġ�ϰ�
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

                //������ HP�� 0�̰ų� 0�̴�ũ�� ��Ȱ��ȭ��Ų��.
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
    void Attacking()//���Ͱ� �����ϴ����̸� 
    {
    
        target.GetComponent<HP_M>().dmg_HP(Atk);//�� ������Ʈ�� hp �Ŵ����� ���ݷ¸�ŭ  ü�¿� ����.
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");
      
        if (other.gameObject.CompareTag("Skill_damage up")|| other.gameObject.CompareTag("Skill_damge up_02"))//���� ���ǿ��� ���� ���
        {
            if (Atk != Atk_ex)
                Atk -= ATK_UP;
        }
        if(other.gameObject.CompareTag("Skill_move_speed up")|| other.gameObject.CompareTag("skill_move_speed up_02"))//�̼� ���� �������� ����������
        {
            if(nav.speed!= Speed)
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
        }
        if (other.gameObject.CompareTag("Skill_move_speed up") || other.gameObject.CompareTag("skill_move_speed up_02"))//�̼� ���� ������ ������
        {
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

}
