using System.Collections;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [Header("AimLine-Setting")]
    [Tooltip("(필수)화살 프리팹을 설정해주세요.")]
    [SerializeField] GameObject arr;
    [Tooltip("(필수)화살 머리 프리팹을 설정해주세요.")]
    [SerializeField] GameObject head;
    [Tooltip("(필수)에임선 색깔의 시작과 끝을 설정해주세요.")]
    [SerializeField] Color colr1;
    [SerializeField] Color colr2;
    [Tooltip("(필수)에임선의 굵기를 설정해주세요.")]
    [Range(0.01f, 0.1f)]
    [SerializeField] float thickness;

    LineRenderer line;

    void Start()
    {
        //라인렌더러 설정
        line = GetComponent<LineRenderer>();
        line.SetColors(colr1, colr2);
        line.SetWidth(thickness, thickness);
    }

    // Update is called once per frame
    void Update()
    {
        if (arr.GetComponent<ArrowController>().astate != ArrowState.Piew && arr.GetComponent<ArrowController>().astate != ArrowState.Shoot)
        {
            Transform arrTrs = head.transform;
            Transform destTrs = GameObject.Find("Destination(Clone)").transform;

            //라인렌더러 처음위치 나중위치
            line.SetPosition(0, arrTrs.position);
            line.SetPosition(1, destTrs.position);
        }
        else
        {
            Destroy(line);
        }
    }



}
