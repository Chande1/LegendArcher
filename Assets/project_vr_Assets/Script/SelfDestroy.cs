using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelfDestroy : MonoBehaviour
{
    Transform Target;
    public float DestroyTime = 5.0f;
    bool once=false;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name== "Lizard_1(Clone)" && once==false)
        {
            this.gameObject.transform.parent = other.gameObject.transform;
            once = true;
        }
        
    }

}
