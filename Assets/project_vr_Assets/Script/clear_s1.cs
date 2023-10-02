using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class clear_s1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyCode everykey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(everykey))
            {
                SceneManager.LoadScene("stage_2");
            }
        }
    }
}
