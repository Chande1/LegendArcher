using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Vector3 way;
    Vector3 pos;
    public float dmg = 5;
    bool A = false;
    public float Time_to_stiff = 3.0f;
    Vector3 t_Num = new Vector3(0, 0, 0);
    int num = 0;
    GameObject[] gm = new GameObject[30];
    int s = 0;
    bool Stop = false;
    [Header("오디오관련")]
    [SerializeField] AudioSource ad_s;
    [SerializeField] AudioClip fire_clip;
    [SerializeField] AudioClip Lighting_clip;
    public float _Speed = 0.08f;
    private void Start()
    {
        ad_s = GetComponent<AudioSource>();
    }
    public void Fire_in(Vector3 Tr_, float dmg_)
    {
        if (Tr_ != t_Num)
        {


            if (A == false)
            {


                pos = this.transform.position;

                way = Tr_ - pos;
                dmg = dmg_;
                A = true;
            }
        }
    }


    private void Update()
    {
        if (Stop == true)
        {
            stiff_On();
            Time_to_stiff -= Time.deltaTime;
        }

        Destroy(this.gameObject, 12f);
        transform.position += new Vector3(way.x, 0, way.z) * _Speed;
    }

    void stiff_On()
    {
        if (Time_to_stiff >= 0)
        {


            for (int j = 0; j < s; j++)
            {
                if (gm[j] != null)
                {
                    if (gm[j].gameObject.GetComponent<AI_EnemyIndex>() != null)
                        gm[j].gameObject.GetComponent<AI_EnemyIndex>().stiff = true;
                    if (gm[j].gameObject.GetComponent<AI_Enemy_buf>() != null)
                        gm[j].gameObject.GetComponent<AI_Enemy_buf>().stiff = true;
                    if (gm[j].gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                        gm[j].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().stiff = true;
                    if (gm[j].gameObject.GetComponent<Suicide_Enemy>() != null)
                        gm[j].gameObject.GetComponent<Suicide_Enemy>().stiff = true;

                }

            }
        }
        else
        {
            for (int j = 0; j < s; j++)
            {
                if (gm[j] != null)
                {
                    if (gm[j].gameObject.GetComponent<AI_EnemyIndex>() != null)
                        gm[j].gameObject.GetComponent<AI_EnemyIndex>().stiff = false;
                    if (gm[j].gameObject.GetComponent<AI_Enemy_buf>() != null)
                        gm[j].gameObject.GetComponent<AI_Enemy_buf>().stiff = false;
                    if (gm[j].gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                        gm[j].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().stiff = false;
                    if (gm[j].gameObject.GetComponent<Suicide_Enemy>() != null)
                        gm[j].gameObject.GetComponent<Suicide_Enemy>().stiff = false;

                }

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {


            if (this.gameObject.CompareTag("Fire"))
            {
                ad_s.clip = fire_clip;
                ad_s.Play();
                if (other.gameObject.GetComponent<AI_EnemyIndex>() != null)
                    other.gameObject.GetComponent<AI_EnemyIndex>().EnemyHP -= dmg;
                if (other.gameObject.GetComponent<AI_Enemy_buf>() != null)
                    other.gameObject.GetComponent<AI_Enemy_buf>().EnemyHP -= dmg;
                if (other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                    other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>().EnemyHP -= dmg;
                if (other.gameObject.GetComponent<Suicide_Enemy>() != null)
                    other.gameObject.GetComponent<Suicide_Enemy>().EnemyHP -= dmg;

            }
            if (this.gameObject.CompareTag("Electric"))
            {
                ad_s.clip = Lighting_clip;
                ad_s.Play();
                gm[s] = other.gameObject;
                Stop = true;
                s++;
            }
        }
        /*
         if (other.gameObject.CompareTag("transparent"))
         {
             Destroy(this.gameObject);
         }
        */

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("wall"))
        {
            this.gameObject.transform.position += new Vector3(0, 0.01f, 0);
        }


    }


}
