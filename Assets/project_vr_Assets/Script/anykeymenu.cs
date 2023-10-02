using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class anykeymenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*foreach (KeyCode everykey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(everykey))
            {
                SceneManager.LoadScene("menu01");
            }
        }*/

        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger) || OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            SceneManager.LoadScene("menu01");
        }
    }
}
