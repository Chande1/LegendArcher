using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<HP_M>().Hp <= 0)
        {
            Destroy(this.gameObject, 3f);
        }
    }
}
