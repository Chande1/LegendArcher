using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_self : MonoBehaviour
{
    //���� ���� ��ũ��Ʈ
    Transform buf_target;//������ ���� Ÿ���� ��ǥ�� �ޱ� ���� ���
    private Vector3 vel = Vector3.zero;//������ ������ ����ϱ� ���� ���
    public float desrroy_Time = 5f;//�ı��Ǵ� �ð�


    public void Awake()
    {
        buf_target = GameObject.Find("Lizard_1").GetComponent<Transform>();//�÷��̾��� ��ġ�� ���� �±׸� ���� ������Ʈ���Լ� �����´�.
    }

    private void Start()
    {
        //Destroy(this.gameObject, desrroy_Time);//desrroy_Time�� �ڿ� �ı�

    }


    // Update is called once per frame
    public void Update()
    {
        
        Vector3 target_v = buf_target.TransformPoint(new Vector3(0,0,0));

        this.transform.position = Vector3.SmoothDamp(this.transform.position, target_v, ref vel, 0.1f);//����
        //SmoothDamp�� �� �״�� �ε巴�� ���°��̴�
        //this.transform.position= ���� ���� ������
        //target_v�� �����Ϸ��� ��ġ
        //ref vel�� ���� �ӵ� �̴�(�Ź� ȣ��Ǵ� �Լ��� ���� �����ȴ�)
        //0.1f�� �ִ�ӵ��̴�.

       

        Destroy(this.gameObject, desrroy_Time);//desrroy_Time�� �ڿ� �ı�
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.name== "Lizard_1")
        {
            buf_target = other.gameObject.transform;
        }

    }
   

}
