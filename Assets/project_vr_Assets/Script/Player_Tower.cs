using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Tower : MonoBehaviour
{
    //�÷��̾� Ÿ�� ���� �Ŵ���
    public HP_M hp;//ü�� �Ŵ���
    public float PlayerHP = 100;//ü�� ����

    private void Awake()
    {
        hp.HP_save(PlayerHP);//hp�Ŵ����� ü�� �ֱ�
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Tower HP:" + hp.Hp);
        if (hp.Hp<=0)
        {
            Destroy(this.gameObject);
        }

    }
}
