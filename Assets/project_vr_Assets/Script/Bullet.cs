using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //투척용 몬스터용 총알 스크립트

    float Enemy_th_ATk;//투척 몬스터의 공격력을 받기위한 변수
    private Vector3 vel = Vector3.zero;//스무스 덤프를 사용하기 위한 변수

    Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    public float Sp_y=3f;

    Transform Projectile;
    private Transform myTransform;
    AudioSource StoneSound;

    void Awake()
    {
        StoneSound = GetComponent<AudioSource>();
        Projectile = this.transform;
        Target = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//플레이어의 위치는 캐슬이라는 태그를 가진 오브젝트에게서 가져온다.
        //Enemy_th_ATk = GameObject.FindWithTag("Throw_Enemy").GetComponent<throw_Enemy>().Atk;//공격력을 가져옴
        Enemy_th_ATk = GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().Atk;//공격력을 가져옴
        myTransform = transform;
    }

    void Start()
    {
        StartCoroutine(SimulateProjectile());
    }


    IEnumerator SimulateProjectile()
    {




        Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);


        float target_Distance = Vector3.Distance(Projectile.position, Target.position);


        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);


        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);


        float flightDuration = target_Distance / Vx;


        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Sp_y * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTower"))//콜라이더 닿은게 플레이어 타워면
        {
            StoneSound.Play();
            //Debug.Log("playertower");
            other.gameObject.GetComponent<HP_M>().dmg_HP(Enemy_th_ATk);//플레이어 타워의 체력매니저에 대미지를 준다
            Destroy(this.gameObject, 2f);// 그후 자신은 파괴된다.
        }



    }

}