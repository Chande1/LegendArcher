using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower_originalHP : MonoBehaviour
{
    public float t_originalHP;
    // Start is called before the first frame update
    void Start()
    {
        t_originalHP = this.GetComponent<HP_M>().Hp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
