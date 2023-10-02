using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour
{
    //int i;
    //최대 몬스터 생성 횟수
    int MaxinumCount = 0;
    public int MaxSpawn = 100;

    //몬스터가 출현할 위치를 담을 배열
    public Transform[] points;
    //몬스터 프리팹을 할당할 변수\
    public GameObject monsterPrefab; //Gameobject[]

    //몬스터를 발생시킬 주기
    public float createTime = 0f;
    //몬스터의 최대 발생 개수
    //public int maxMonster = 10;
    //게임 종료 여부 변수
    public bool isGameOver = false;

    // Use this for initialization
    void Start()
    {
        
        //Hierarchy View의 Spawn Point를 찾아 하위에 있는 모든 Transform 컴포넌트를 찾아옴
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            //몬스터 생성 코루틴 함수 호출
            //StartCoroutine(this.CreateMonster());
            InvokeRepeating("CreateMonster", 1, createTime+Time.deltaTime);
        }
    }
    //IEnumerator CreateMonster()
    void CreateMonster()
    {
        //게임 종료 시까지 무한 루프
        //while (!isGameOver)
        //while (MaxinumCount < MaxSpawn) 
        if (MaxinumCount < MaxSpawn)
            {
            //현재 생성된 몬스터 개수 산출
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("enemy").Length;

            if (monsterCount < MaxSpawn) //MaxMonster
            {
                //몬스터의 생성 주기 시간만큼 대기
                //yield return new WaitForSeconds(createTime);

                //불규칙적인 위치 산출
                int idx = Random.Range(1, points.Length);
                //몬스터의 동적 생성
                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation); //monsterPrefab[i]
                {
                    Debug.Log("Monster Spawned");
                    MaxinumCount = MaxinumCount + 1;
                    Debug.Log(MaxinumCount);
                }
               
            }


            /*
            if (MaxinumCount >= 5)
            {
                //Debug.Log("123");
                StopCoroutine(CreateMonster());
                //CancelInvoke("CreateMonster");
            }
            */
            
            /*
            else 
            {

                yield return null;
            }
            */
            



        }
    }
}