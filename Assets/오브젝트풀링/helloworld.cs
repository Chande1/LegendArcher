using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helloworld : MonoBehaviour
{
    float TimeLeft = 2.0f;
    float nextTime = 0.0f;
    float time;
    int hellostack = 0 ;
    bool bt = false;
    
    // Start is called before the first frame update
    void hello()
    {
        Debug.Log("Hello World!");
    }

    void bye()
    {
        Debug.Log("GoodBye!");
    }
    
    void Update()
    {

        time += Time.deltaTime;

        //if (Time.time > nextTime)
        if (time > nextTime)
        {
            nextTime = time + TimeLeft;
            if (bt == false)
            {
                hello();
                hellostack = hellostack + 1;
                //Debug.Log(hellostack);
                //bt = true;
            }
        }

        if (hellostack == 2)
        {
            
            bt = true;
            //nextTime = time + TimeLeft;
            if (bt == true)
            {
                bye();
                hellostack = 0;
                //nextTime = 0.0f;
                //time = 0.0f;
                bt = false;
                //hello();  
            }

            //bye();
        }
    }


    // Update is called once per frame




}
