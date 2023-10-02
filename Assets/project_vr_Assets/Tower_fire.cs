using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Tower_fire : MonoBehaviour
{

    GameObject[] Target_TR = new GameObject[100];
    public float Atk_Time;
    float Atk_save;


    public GameObject FireA;
    public float dmg;
    int A = 0;
    public Vector3 grid;
    bool Atk_Time_bool = false;
    int i = 0;
    int j = 0;//몇개 있는지 체크 할라고 만든것
    public AudioSource ad_s;



    public Transform hand;
    public void GM_IN(GameObject _gm)//값 받기
    {

        for (; ; )
        {
            if (i == 100) //i == 10;
            {
                i = 0;
                Target_TR[i] = _gm;
                break;
            }


            if (Target_TR[i] != null)
            {
                i++;
            }
            else
            {
                Target_TR[i] = _gm;
                j++;//값 증가
                i = 0;
                break;
            }

        }


    }

    private void Start()
    {
        Atk_save = Atk_Time;
    }
    public void Gm_Out(GameObject gm_)//게임오브젝트 관리
    {


        for (int i = 0; i < j; i++)
        {
            if (Target_TR[i] == gm_)
            {
                Target_TR[i] = null;//빈공간으로 만들기      
                j--;//값이 나간것
                break;
            }
        }
        if (j > 0)
        {


            for (int i = 0; i < j; i++)
            {


                if (Target_TR[i] == null)
                {
                    if (i + 1 < 100)
                        if (Target_TR[i + 1] != null)
                        {
                            Target_TR[i] = Target_TR[i + 1];
                            Target_TR[i + 1] = null;
                            i = 0;
                            break;
                        }
                    //else if (Target_TR[i + 1] == null&&Target_TR[i+2]!=null)//혹시몰라서 넣는것
                    //{
                    //    Target_TR[i] = Target_TR[i + 2];
                    //    Target_TR[i + 2] = null;
                    //    break;
                    //    //+2 값 을 비교 넣기 위에꺼가 오작동을 일으키면 ex: 여러명이 동시에 죽었을떄 처리가 어떻게 되는지에 따라 달라질수있음
                    //}

                }


            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (j >= 0)
        {
            for (int i = 0; i < j; i++)//한 명이라도 타워 안의 범위 안에 들어오면 공격을 트루로 한다
            {
                if (Target_TR[i] != null)
                {
                    if (Target_TR[i].transform.position.x > (this.gameObject.transform.position.x - (grid.x * 0.5)) && Target_TR[i].transform.position.x < (this.gameObject.transform.position.x + (grid.x * 0.5)) && Target_TR[i].transform.position.z > (this.gameObject.transform.position.z - (grid.z * 0.5)) && Target_TR[i].transform.position.z < (this.gameObject.transform.position.z + (grid.z * 0.5)))
                    {

                        Atk_Time_bool = true;
                    }
                }
            }
        }
        else { Atk_Time_bool = false; }

        if (Atk_Time > 0 && Atk_Time_bool == true)
            Atk_Time -= Time.deltaTime;
        if (Atk_Time <= 0)
        {
            ad_s.Play();
            Instantiate(FireA, hand.position, Quaternion.identity);
            Atk_Time = Atk_save;
        }




    }

    private void OnTriggerEnter(Collider other)
    {
        A = Random.Range(0, j);//난수 
        if (other.gameObject.CompareTag("Fire"))
        {
            if (Target_TR[A] != null)
                other.gameObject.GetComponent<Fire>().Fire_in(Target_TR[A].transform.position, dmg);
            else
            {
                if (Target_TR[0] != null)
                    other.gameObject.GetComponent<Fire>().Fire_in(Target_TR[0].transform.position, dmg);
            }
        }
        else if (other.gameObject.CompareTag("Electric"))
        {
            if (Target_TR[A] != null)
                other.gameObject.GetComponent<Fire>().Fire_in(Target_TR[A].transform.position, dmg);
            else
            {
                if (Target_TR[0] != null)
                    other.gameObject.GetComponent<Fire>().Fire_in(Target_TR[0].transform.position, dmg);
            }
        }


    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(grid.x, grid.y, grid.z));
    }
}