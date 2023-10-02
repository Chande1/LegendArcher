using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController1 : MonoBehaviour
{
    public GameObject pHand;    //플레이어 손 프리팹

    public float pMovespeed;    //플레이어 이동 속도
    public float pViewspeed;    //플레이어 시야 속도


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();       //플레이어 이동/시야
        //PlayerHandMovement();   //플레이어 손
    }

    //플레이어 움직임
    void PlayerMovement()
    {
        /*
        //이동
        if(Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Translate(0, 0, pMovespeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Translate(0, 0, -pMovespeed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Translate(-pMovespeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Translate(pMovespeed, 0,0);
        }

        //시야
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.Rotate(0, -pViewspeed, 0);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            gameObject.transform.Rotate(0,pViewspeed, 0);
        }
        else if (Input.GetKey(KeyCode.R))
        {
            gameObject.transform.Rotate(-pViewspeed,0, 0);
        }
        else if (Input.GetKey(KeyCode.F))
        {
            gameObject.transform.Rotate(pViewspeed, 0, 0);
        }
        */
        //시야
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.Rotate(0, -pViewspeed, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.Rotate(0, pViewspeed, 0);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.Rotate(-pViewspeed, 0, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.Rotate(pViewspeed, 0, 0);
        }


    }

    void PlayerHandMovement()
    {
        Vector3 mos = Input.mousePosition;

        mos.z = gameObject.transform.localPosition.z+0.7f;

        Vector3 dir = Camera.main.ScreenToWorldPoint(mos);  //월드 좌표를 플레이어 화면 좌표로 변환

        pHand.transform.localPosition= dir;

    }
}
