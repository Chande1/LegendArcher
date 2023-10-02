using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class throw_Enemy : MonoBehaviour
{
    //��ô ���Ϳ� ��ũ��Ʈ
    public AnimationClip Atk_anime;//���ݾִϸ��̼�
    public GameObject bullet;//���� �Ѿ�
    public GameObject hand;//�Ѿ��� ���� ��ġ
    UI_manager ui;
    public HP_M hp;//ü�� �Ŵ���
    public float EnemyHP = 100;//���� ü�� ����
    private Transform Player;//Ÿ�� �� Ÿ��
    public float Atk = 1;// ���ݷ�
    float EnemyHp_S = 0;

    Animator Ani;

    float ATK_delay = 3f;//���� ������ 
    float ATK_deal_re_save;//���ݵ����� �ʱ�ȭ�� ����
    bool Atk_ON = false;//���� ����Ī�� ����
    bool Active_bt = false;
    public float Time_to_Active = 1f;//���� �Ͼ�� ���� �ɸ��� �ð�
    float Time_to_Active_Count_1 = 0;
    float Time_to_Active_Count_2 = 0;
    float Time_to_Active_Count_3 = 0;
    float Time_to_Active_Count_4 = 0;
    float Count = 0f;

    public GameObject target;//�����Ҵ��

    float time_sturn = 6;//���Ͽ� 
    bool Arrow_sturn = false;//3.0f ���� ���п�
    bool Arrow_sturn_2 = false;//6.0f ���� ���п�
    AudioSource adi;
    public AudioClip dead;
    public AudioClip revive;
    public AudioClip throw_st_1;//1
    public AudioClip throw_st_2;//2
    bool ATK_ad_c = false;//false 1 Ʈ��� 2
    public bool stiff = false; //����

    [Header("�ް� �ִ� ��ų�� ����")]
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
        hp = GetComponent<HP_M>();//ü�� �Ŵ���

        if(GameObject.FindWithTag("PlayerTower"))
        {
            Player = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//�÷��̾��� ��ġ�� ĳ���̶�� �±׸� ���� ������Ʈ���Լ� �����´�.  
            target = Player.gameObject;//��ǥ ����(������)
        }
       
        ATK_delay = 5.9f;//����� �ð�+ ���ݽð�
        ATK_deal_re_save = Atk_anime.length;
        EnemyHp_S = EnemyHP;
    }
    private void Start()
    {
        hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�

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



            if (EnemyHP <= 0)//����
            {
                adi.clip = dead;//���� �� ������Ʈ�� ���� Ŭ���� ����־��
                adi.Play();
                Ani.SetBool("On", false);
                Ani.SetBool("isdie", true);
                Ani.SetBool("idel", false);
                Active_bt = false;
                if (Count == 0)
                    Time_to_Active = Time_to_Active_Count_1;//�ð� �ʱ�ȭ
                else if (Count == 1)
                    Time_to_Active = Time_to_Active_Count_2;//�ð� �ʱ�ȭ
                else if (Count >= 2)
                    Time_to_Active = Time_to_Active_Count_3;//�ð� �ʱ�ȭ 

                EnemyHP = EnemyHp_S;//�ʱ�ȭ
                Count++;
                //Invoke("ks", 3f);
            }
            if (stiff == true)
            {
                Ani.speed = 0f;
            }



            hp.HP_save(EnemyHP);//hp�Ŵ����� ���� ü�� �ֱ�


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
                    ATK_delay -= 0.02f;//���� �����̸� �ٿ��ش� 
                    if (ATK_delay <= 0 && EnemyHP > 0)// ���� �����̰� 0���� �Ʒ��� �Ǹ�
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
                        Attacking();//���� �Լ��� �ߵ�
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
        Instantiate(bullet, hand.gameObject.transform.position, Quaternion.identity);//�Ѿ� ��������
        ATK_delay = ATK_deal_re_save;//�ٽ� �ʱ�ȭ
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
