using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_M : MonoBehaviour
{
    //ü�� �Ŵ���
    public float Hp=10f;//hp���� ����
    public void HP_save(float hp)//hp�� �Է� ����
    {
         Hp=hp;//�Է� ���� hp�� HP�� ����
    }

    public void dmg_HP(float dmg)//dmg�� �޴´� 
    {
        

        Hp -= dmg;//������ hp�� dmg�� ���ش�
        
    }



}
