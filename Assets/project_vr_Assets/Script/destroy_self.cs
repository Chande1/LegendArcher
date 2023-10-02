using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroy_self : MonoBehaviour
{
    //버프 관련 스크립트
    Transform buf_target;//버프를 쓰는 타겟의 좌표를 받기 위해 사용
    private Vector3 vel = Vector3.zero;//스무스 덤프를 사용하기 위해 사용
    public float desrroy_Time = 5f;//파괴되는 시간


    public void Awake()
    {
        buf_target = GameObject.Find("Lizard_1").GetComponent<Transform>();//플레이어의 위치는 버프 태그를 가진 오브젝트에게서 가져온다.
    }

    private void Start()
    {
        //Destroy(this.gameObject, desrroy_Time);//desrroy_Time초 뒤에 파괴

    }


    // Update is called once per frame
    public void Update()
    {
        
        Vector3 target_v = buf_target.TransformPoint(new Vector3(0,0,0));

        this.transform.position = Vector3.SmoothDamp(this.transform.position, target_v, ref vel, 0.1f);//따라감
        //SmoothDamp는 말 그대로 부드럽게 가는것이다
        //this.transform.position= 현재 나의 포지션
        //target_v는 도달하려는 위치
        //ref vel은 현재 속도 이다(매번 호출되는 함수에 의해 수정된다)
        //0.1f는 최대속도이다.

       

        Destroy(this.gameObject, desrroy_Time);//desrroy_Time초 뒤에 파괴
    }

    private void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.name== "Lizard_1")
        {
            buf_target = other.gameObject.transform;
        }

    }
   

}
