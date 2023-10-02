using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClick : MonoBehaviour
{
    // Start is called before the first frame update
    public void MoveObject()
    {
        //Debug.Log("move");
        gameObject.transform.Translate(0, 1, 0);
    }
}
