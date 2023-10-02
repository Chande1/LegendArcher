using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow_land : MonoBehaviour
{
    public float Slow=3f;

    private void Awake()
    {
        
    }

    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AI_EnemyIndex>()!=null)
            other.gameObject.GetComponent<AI_EnemyIndex>().Speed -= Slow;
        if (other.gameObject.GetComponent<AI_Enemy_buf>() != null)
            other.gameObject.GetComponent<AI_Enemy_buf>().Speed -= Slow;
        if (other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
            other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>().Speed -= Slow;
        if (other.gameObject.GetComponent<Suicide_Enemy>() != null)
            other.gameObject.GetComponent<Suicide_Enemy>().Speed -= Slow;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<AI_EnemyIndex>() != null)
            other.gameObject.GetComponent<AI_EnemyIndex>().Speed += Slow;
        if (other.gameObject.GetComponent<AI_Enemy_buf>() != null)
            other.gameObject.GetComponent<AI_Enemy_buf>().Speed += Slow;
        if (other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
            other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>().Speed += Slow;
        if (other.gameObject.GetComponent<Suicide_Enemy>() != null)
            other.gameObject.GetComponent<Suicide_Enemy>().Speed += Slow;

    }

}
