using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_AI : MonoBehaviour
{
    //�Ϲ� ��ġ�� Ÿ�� ��ũ��Ʈ
    public HP_M hp;//ü�� �Ŵ���
    public float TowerHP=100f;//Ÿ�� ü�� 
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
