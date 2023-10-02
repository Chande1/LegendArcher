using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringManager : MonoBehaviour
{

    [SerializeField]LineRenderer line;  //라인
    [SerializeField] Transform Top;
    [SerializeField] Transform Middle;
    [SerializeField] Transform Bottom;

    void Start()
    {
        //라인렌더러 설정
        line = GetComponent<LineRenderer>();
        line.SetColors(Color.white, Color.white);
        line.SetWidth(0.008f, 0.008f);
    }

    // Update is called once per frame
    void Update()
    {
        Top = GameObject.Find("s_top").transform;
        Middle = GameObject.Find("s_middle").transform;
        Bottom = GameObject.Find("s_bottom").transform;

        //라인렌더러 처음위치 나중위치
        line.SetPosition(0, Top.position);
        line.SetPosition(1, Middle.position);
        line.SetPosition(2, Bottom.position);
    }
}
