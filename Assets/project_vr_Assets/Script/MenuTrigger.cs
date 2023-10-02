using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            Invoke("stage1", 3f);
            Debug.Log("LoadScene 'map01' 3seconds later");
        }
    }

    void stage1()
    {
        SceneManager.LoadScene("map01");
    }
}
