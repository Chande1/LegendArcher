using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_AI : MonoBehaviour
{
    //일반 설치형 타워 스크립트
    public HP_M hp;//체력 매니져
    public float TowerHP=100f;//타워 체력 
    // Start is called before the first frame update
    void Start()
    {
        hp = GetComponent<HP_M>();
        hp.HP_save(TowerHP);
    }

    // Update is called once per frame
    void Update()
    {
       
       
     if (hp.Hp<=0)
        {
           Destroy(gameObject);
        }        
    }
}
