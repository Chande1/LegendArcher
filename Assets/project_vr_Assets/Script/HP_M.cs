using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_M : MonoBehaviour
{
    //체력 매니저
    public float Hp=10f;//hp변수 생성
    public void HP_save(float hp)//hp를 입력 받음
    {
         Hp=hp;//입력 받은 hp는 HP에 저장
    }

    public void dmg_HP(float dmg)//dmg를 받는다 
    {
        

        Hp -= dmg;//저장한 hp에 dmg를 뺴준다
        
    }



}
