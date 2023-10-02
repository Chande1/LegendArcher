using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //��ô�� ���Ϳ� �Ѿ� ��ũ��Ʈ

    float Enemy_th_ATk;//��ô ������ ���ݷ��� �ޱ����� ����
    private Vector3 vel = Vector3.zero;//������ ������ ����ϱ� ���� ����

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
        Target = GameObject.FindWithTag("PlayerTower").GetComponent<Transform>();//�÷��̾��� ��ġ�� ĳ���̶�� �±׸� ���� ������Ʈ���Լ� �����´�.
        //Enemy_th_ATk = GameObject.FindWithTag("Throw_Enemy").GetComponent<throw_Enemy>().Atk;//���ݷ��� ������
        Enemy_th_ATk = GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().Atk;//���ݷ��� ������
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
        if (other.gameObject.CompareTag("PlayerTower"))//�ݶ��̴� ������ �÷��̾� Ÿ����
        {
            StoneSound.Play();
            //Debug.Log("playertower");
            other.gameObject.GetComponent<HP_M>().dmg_HP(Enemy_th_ATk);//�÷��̾� Ÿ���� ü�¸Ŵ����� ������� �ش�
            Destroy(this.gameObject, 2f);// ���� �ڽ��� �ı��ȴ�.
        }



    }

}