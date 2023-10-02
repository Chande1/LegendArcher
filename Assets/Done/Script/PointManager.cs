using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    GameObject Bow;
    [Header("ArrowPoint")]
    [SerializeField] bool pointwork;        //포인트가 작동할 때
    [SerializeField] Transform aptrs;       //arrowpoint의 좌표
    [SerializeField] Vector3 temptrs;       //임시저장 좌표(초기좌표)
    [Tooltip("(필수)최대 시위 범위를 설정해주세요.")]
    [Range(2,8)]
    [SerializeField] float maxapdistance;   //당기기 가능한 최대좌표(5~8추천)
    [Header("Bow-Destination")]
    [SerializeField] ArrowController arrdest;
    [Header("진동 세기")]
    [Range(1, 2)]
    [SerializeField] float controllvibe;
    [Header("시위 별 거리 차이")]
    [Range(1, 20)]
    [SerializeField] float stringtodest;


    void Start()
    {
        Bow = GameObject.Find("Bow");
        aptrs = gameObject.transform;
        temptrs = aptrs.localPosition ;       //초기좌표 저장
        maxapdistance *= 0.1f;
        pointwork = false;
    }

    void Update()
    {
        if (!gameObject.GetComponent<OVRGrabbable>().isGrabbed)  //줄을 잡아당기고 있지 않으면
        {
            pointwork = false;  //포인트 작동정지
            aptrs.localPosition = temptrs;
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);   //진동 끝
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);   //진동 끝
            gameObject.transform.rotation = Bow.transform.rotation;              //회전 초기화
        }
        else
        {
            if (Vector3.Distance(aptrs.localPosition, temptrs) > 0)  //줄이 조금이라도 당겨지면
            {
                OVRInput.SetControllerVibration(Vector3.Distance(aptrs.localPosition, temptrs) * controllvibe, Vector3.Distance(aptrs.localPosition, temptrs) * controllvibe, OVRInput.Controller.LTouch);   //당겨진 만큼 진동
                OVRInput.SetControllerVibration(Vector3.Distance(aptrs.localPosition, temptrs)* controllvibe, Vector3.Distance(aptrs.localPosition, temptrs)* controllvibe, OVRInput.Controller.RTouch);   //당겨진 만큼 진동
                pointwork = true;   //포인트 작동중

                if(aptrs.localPosition.z>temptrs.z)                                     //줄이 앞으로 넘어간경우
                {
                    aptrs.localPosition = temptrs;
                    OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);   //진동 끝
                    OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);   //진동 끝
                    gameObject.transform.rotation = Bow.transform.rotation;              //회전 초기화
                }
                else if (Bow.GetComponent<cBow>().bstate == BowState.Drawing)
                {
                    //gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
                    //Debug.Log("aptrs.position.z:" + aptrs.position.z + "temptrs.z:" + temptrs.z);

                     if(arrdest!=null)
                        arrdest.SetDestDistance(Vector3.Distance(aptrs.localPosition, temptrs) * 10f* stringtodest);       //조준점 거리 전달
                    //Debug.Log("Point:"+Vector3.Distance(aptrs.localPosition, temptrs) * 10f * stringtodest);
                }

                
                //Debug.Log("Distance:" + Vector3.Distance(aptrs.localPosition, temptrs) + "max:" + maxapdistance);
                if (Vector3.Distance(aptrs.localPosition, temptrs) >= maxapdistance) //줄이 최대 좌표 거리를 넘어가면
                {
                    aptrs.localPosition = new Vector3(aptrs.localPosition.x, aptrs.localPosition.y, temptrs.z - maxapdistance); //최대 좌표에 고정         
                    OVRInput.SetControllerVibration(Vector3.Distance(aptrs.localPosition, temptrs) * controllvibe, Vector3.Distance(aptrs.localPosition, temptrs) * controllvibe, OVRInput.Controller.LTouch);   //당겨진 거리만큼 진동               
                    OVRInput.SetControllerVibration(Vector3.Distance(aptrs.localPosition, temptrs)* controllvibe, Vector3.Distance(aptrs.localPosition, temptrs)* controllvibe, OVRInput.Controller.RTouch);   //당겨진 거리만큼 진동                                                                                                          
                }
                
            }
            else        //줄을 당기지 않으면
            {
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);   //진동 끝
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);   //진동 끝
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<ArrowController>()!=null)
            arrdest = other.gameObject.GetComponent<ArrowController>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<ArrowController>() != null)
            arrdest = null;
    }

    public bool isPoint()
    {
        if (pointwork)      //작동 중이면 true
            return true;
        else
            return false;   //작동중이 아니면 false
    }

    
}
