using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Tower : MonoBehaviour
{
    //플레이어 타워 관련 매니저
    public HP_M hp;//체력 매니져
    public float PlayerHP = 100;//체력 설정

    private void Awake()
    {
        hp.HP_save(PlayerHP);//hp매니저에 체력 넣기
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
