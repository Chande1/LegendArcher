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
    int j = 0;//� �ִ��� üũ �Ҷ�� �����
    public AudioSource ad_s;



    public Transform hand;
    public void GM_IN(GameObject _gm)//�� �ޱ�
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
                j++;//�� ����
                i = 0;
                break;
            }

        }


    }

    private void Start()
    {
        Atk_save = Atk_Time;
    }
    public void Gm_Out(GameObject gm_)//���ӿ�����Ʈ ����
    {


        for (int i = 0; i < j; i++)
        {
            if (Target_TR[i] == gm_)
            {
                Target_TR[i] = null;//��������� �����      
                j--;//���� ������
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
                    //else if (Target_TR[i + 1] == null&&Target_TR[i+2]!=null)//Ȥ�ø��� �ִ°�
                    //{
                    //    Target_TR[i] = Target_TR[i + 2];
                    //    Target_TR[i + 2] = null;
                    //    break;
                    //    //+2 �� �� �� �ֱ� �������� ���۵��� ����Ű�� ex: �������� ���ÿ� �׾����� ó���� ��� �Ǵ����� ���� �޶���������
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
            for (int i = 0; i < j; i++)//�� ���̶� Ÿ�� ���� ���� �ȿ� ������ ������ Ʈ��� �Ѵ�
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
        A = Random.Range(0, j);//���� 
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