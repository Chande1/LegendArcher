using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmove : MonoBehaviour
{
    public Transform s, e;

    private void Start()
    {
        transform.position = s.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, e.position, 3f*Time.deltaTime);
    }
}
