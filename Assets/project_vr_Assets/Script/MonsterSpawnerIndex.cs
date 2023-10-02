using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerIndex : MonoBehaviour
{
    //최대 몬스터 생성 횟수
    public int SpawnCount = 0;
    public int MaxSpawn = 100;

    //몬스터가 출현할 위치를 담을 배열
    public Transform[] points;
    //몬스터 프리팹을 할당할 변수\
    public GameObject[] monsterPrefab; //Gameobject[]

    public float StartDelay = 1f;
    //몬스터를 발생시킬 주기
    public float createTime = 1.5f;
    float temptime = 0;
    //몬스터의 최대 발생 개수
    //public int maxMonster = 10;
    //게임 종료 여부 변수
    public bool isGameOver = false;

    public AudioClip Spawnsound;

    /*
    float TimeLeft = 2.0f;
    float nextTime = 0.0f;
    float time;
    int hellostack = 0;
    bool bt = false;
    */
    // Use this for initialization
    private void Awake()
    {
        Spawnsound = Resources.Load<AudioClip>("spawnsound");
        temptime = createTime;
    }

    void Start()
    {

        //Hierarchy View의 Spawn Point를 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        //points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();


    }
    public void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            setting_spawnpoints();
            if(StartDelay>0)
                StartDelay -= Time.deltaTime;
            else if(StartDelay<=0)
            {
                //몬스터 생성 코루틴 함수 호출
                createTime -= Time.deltaTime;

                if (createTime <= 0)
                {
                    Invoke("CreateMonster_go", StartDelay);
                    createTime = temptime;
                }
            }
            

            
        }




        /*if (points.Length > 0)
        {
            time += Time.deltaTime;

            if (time > nextTime)
            {
                nextTime = time + createTime;

                if (bt == false && monsterPrefab[SpawnCount].name == "bandit_u")
                {
                    CreateMonster_ba();
                }
            }
        }*/


    }



    //IEnumerator CreateMonster()

    //SpawnCount가 MaxSpawn보다 적을때 고블린풀에 있는 고블린을 활성화시키고 활성화시킬때마다 SpawnCount +1
    public void CreateMonster_go()
    {
        //while (!isGameOver)
        //현재 생성된 몬스터 개수 산출
        //int monsterCount = (int)GameObject.FindGameObjectsWithTag("enemy").Length;

        if (SpawnCount < MaxSpawn) //MaxMonster
        {
            //몬스터의 생성 주기 시간만큼 대기
            //yield return new WaitForSeconds(createTime);

            //불규칙적인 위치 산출
            int idx = Random.Range(1, points.Length);


            //몬스터의 동적 생성
            Instantiate(monsterPrefab[SpawnCount], points[idx].position, points[idx].rotation);
            GameObject.Find("spawnsound_audiosource").GetComponent<AudioSource>().clip = Spawnsound;
            GameObject.Find("spawnsound_audiosource").GetComponent<AudioSource>().volume = 0.5f;
            GameObject.Find("spawnsound_audiosource").GetComponent<AudioSource>().Play();

            if (monsterPrefab != null)
            {
                SpawnCount+=1;
            }
        }
    }

    //SpawnCount가 MaxSpawn보다 적을때 밴딧풀에 있는 밴딧을 활성화시키고 활성화시킬때마다 SpawnCount +1
    /*public void CreateMonster_ba() 
    {

        if (SpawnCount < MaxSpawn) //MaxMonster
        {

            int idx = Random.Range(1, points.Length);

            
            GameObject monsterPrefab = ObjectPool.Instance.CreateBaPool();//getpooledobject
            if (monsterPrefab != null)
            {
                monsterPrefab.transform.position = points[idx].position;
                monsterPrefab.transform.rotation = points[idx].rotation;
                monsterPrefab.SetActive(true);
                //Debug.Log("Monster Spawned");
                SpawnCount = SpawnCount + 1;
            }
        }
    }*/
    public void setting_spawnpoints()
    {
        Transform m_mp1N, m_mp2N, m_mp3N, m_sp1N, m_sp2N, m_sp3N, s_mp1N, s_mp2N, s_mp3N, s_sp1N, s_sp2N, s_sp3N;
        m_mp1N = GameObject.Find("MainPoint1").transform; m_mp2N = GameObject.Find("MainPoint2").transform; m_mp3N = GameObject.Find("MainPoint3").transform;
        s_mp1N = GameObject.Find("sMainPoint1").transform; s_mp2N = GameObject.Find("sMainPoint2").transform; s_mp3N = GameObject.Find("sMainPoint3").transform;
        m_sp1N = GameObject.Find("SubPoint1").transform; m_sp2N = GameObject.Find("SubPoint2").transform; m_sp3N = GameObject.Find("SubPoint3").transform;
        s_sp1N = GameObject.Find("sSubPoint1").transform; s_sp2N = GameObject.Find("sSubPoint2").transform; s_sp3N = GameObject.Find("sSubPoint3").transform;

        Transform m_mp1H, m_mp2H, m_mp3H, m_sp1H, m_sp2H, m_sp3H, s_mp1H, s_mp2H, s_mp3H, s_sp1H, s_sp2H, s_sp3H;
        m_mp1H = GameObject.Find("MainPoint1").transform; m_mp2H = GameObject.Find("MainPoint2").transform; m_mp3H = GameObject.Find("MainPoint3").transform;
        s_mp1H = GameObject.Find("sMainPoint1").transform; s_mp2H = GameObject.Find("sMainPoint2").transform; s_mp3H = GameObject.Find("sMainPoint3").transform;
        m_sp1H = GameObject.Find("SubPoint1").transform; m_sp2H = GameObject.Find("SubPoint2").transform; m_sp3H = GameObject.Find("SubPoint3").transform;
        s_sp1H = GameObject.Find("sSubPoint1").transform; s_sp2H = GameObject.Find("sSubPoint2").transform; s_sp3H = GameObject.Find("sSubPoint3").transform;


        {
            if (this.gameObject.name == "MainSpawner_1N")
            {
                points[0] = m_mp1N;
                points[1] = m_sp1N;
            }
            if (this.gameObject.name == "MainSpawner_2N")
            {
                points[0] = m_mp2N;
                points[1] = m_sp2N;
            }
            if (this.gameObject.name == "MainSpawner_3N")
            {
                points[0] = m_mp3N;
                points[1] = m_sp3N;
            }
            if (this.gameObject.name == "SubSpawner_1N")
            {
                points[0] = s_mp1N;
                points[1] = s_sp1N;
            }
            if (this.gameObject.name == "SubSpawner_2N")
            {
                points[0] = s_mp2N;
                points[1] = s_sp2N;
            }
            if (this.gameObject.name == "SubSpawner_3N")
            {
                points[0] = s_mp3N;
                points[1] = s_sp3N;
            }

            //--------------------------------------------

            if (this.gameObject.name == "MainSpawner_1H")
            {
                points[0] = m_mp1H;
                points[1] = m_sp1H;
            }
            if (this.gameObject.name == "MainSpawner_2H")
            {
                points[0] = m_mp2H;
                points[1] = m_sp2H;
            }
            if (this.gameObject.name == "MainSpawner_3H")
            {
                points[0] = m_mp3H;
                points[1] = m_sp3H;
            }
            if (this.gameObject.name == "SubSpawner_1H")
            {
                points[0] = s_mp1H;
                points[1] = s_sp1H;
            }
            if (this.gameObject.name == "SubSpawner_2H")
            {
                points[0] = s_mp2H;
                points[1] = s_sp2H;
            }
            if (this.gameObject.name == "SubSpawner_3H")
            {
                points[0] = s_mp3H;
                points[1] = s_sp3H;
            }
        }

    }
}