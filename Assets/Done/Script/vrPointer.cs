using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class vrPointer : MonoBehaviour
{
    [Header("(필수)메인 포인터 선택")]
    [SerializeField] bool ImMain;                   //메인포인터(UI용)
    [Tooltip("(확인)메인포인터 검색")]
    [SerializeField] bool useMainPointer;           //현재 메인포인터가 있는지 확인용
    [Tooltip("(확인)UI")]
    [SerializeField] UIDialogue NowUI;              //현재 사용하는 UI
    [Tooltip("(확인)GUI")]
    [SerializeField] GameObject MyGUI;              //현재 사용하는 GUI
    [Tooltip("(확인)레이저가 될 라인 랜더러")]
    [SerializeField] LineRenderer laser;           // 레이저
    [Tooltip("(확인)충돌 객체")]
    [SerializeField] RaycastHit hit;           // 충돌된 레이의 히트포인트
    [Tooltip("(확인)충돌 객체 임시 저장용")]
    [SerializeField] GameObject tempObj;            // 가장 최근에 충돌한 객체를 저장하기 위한 객체
    [Header("레이저 설정")]
    [Range(0.1f, 100)]
    [Tooltip("(필수)레이저 거리")]
    [SerializeField] float raycastDistance;  // 레이저 포인터 감지 거리
    [Tooltip("(필수)트리거를 뗀 레이저 색깔")]
    [SerializeField] Color lasercolor1;             //레이저 색깔1
    [Tooltip("(필수)트리거를 누른 레이저 색깔")]
    [SerializeField] Color lasercolor2;             //레이저 색깔2
    [Tooltip("(필수)레이저 굵기")]
    [SerializeField] float laserThickness;         //레이저 굵기

    void Start()
    {
        laser = gameObject.GetComponent<LineRenderer>();                   //라인랜더러
        laser.positionCount = 2;                                           //레이저 시작과 끝점                                     
        laser.SetWidth(laserThickness, laserThickness);                     //레이저 굵기
        laser.SetColors(lasercolor1, lasercolor1);
        if (GameObject.Find("UI"))
        {
            NowUI = GameObject.Find("UI").GetComponent<UIDialogue>();
        }
       
    }

    private void Update()
    {
        if (GameObject.Find("GUI"))                  //메뉴 UI
        {
            MyGUI = GameObject.Find("GUI");
        }

        if (!ImMain&&GameObject.Find("LaserPointer"))  //메인포인터가 아니면서 현재 메인포인터가 활성화 되어 있으면
        {
            if(GameObject.Find("LaserPointer").activeSelf)
                gameObject.SetActiveRecursively(false);                //객체 비활성화 
            else
                gameObject.SetActiveRecursively(true);                //객체 비활성화 
        }

        // 시작 위치        
        laser.SetPosition(0, transform.position);

        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);


        // 충돌 감지 시      
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance) && gameObject.activeInHierarchy)
        {
            laser.SetPosition(1, hit.point);

            // 버튼 충돌        
            if (hit.collider.gameObject.CompareTag("Button"))
            {
                ButtonRayProcess();
            }
            // 기타 충돌        
            else
            {
                if (tempObj != null)
                {
                    // 버튼 전용        
                    if (tempObj.gameObject.CompareTag("Button"))
                        tempObj.GetComponent<Button>().OnPointerExit(null);

                    tempObj = null;
                }
            }
        }
        // 충돌 미 감지 시    
        else
        {
            laser.SetPosition(1, transform.position + (transform.forward * raycastDistance));

            if (tempObj != null)
            {

                if (tempObj.gameObject.CompareTag("Button"))
                    tempObj.GetComponent<Button>().OnPointerExit(null);



                tempObj = null;
            }
        }
    }


    // 버튼 감지            
    private void ButtonRayProcess()
    {
        // 클릭 Down          
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            laser.SetColors(lasercolor2, lasercolor2);                  //빨간색
            if(hit.collider.gameObject.GetComponent<Button>()!=null)
                hit.collider.gameObject.GetComponent<Button>().OnSelect(null);

        }
        // 클릭 Up            
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            laser.SetColors(lasercolor1, lasercolor1);                                          //파란색
            Button temp = hit.collider.gameObject.GetComponent<Button>();
            
            if (temp.interactable == true)                                                      //상호 작용 가능한 버튼일때(버튼에 체크 박스 있음)
            {
                if (MyGUI.transform.GetChild(0).gameObject.activeSelf)
                {
                    Debug.Log("메뉴 활성화");
                }
                else
                {
                    if (NowUI != null)                                                 //대사 UI가 있으면
                    {
                        NowUI = GameObject.Find("UI").GetComponent<UIDialogue>();   //UI불러오기
                        NowUI.NextIngNum();                                         //다음 대사로 이동
                    }
                }


                EventSystem.current.SetSelectedGameObject(hit.collider.gameObject);
                PointerEventData pointerEventData = new PointerEventData(null);
                pointerEventData.position = new Vector2(hit.point.x, hit.point.y);

                //temp.onClick.Invoke();
                temp.OnSelect(pointerEventData);
                temp.OnPointerClick(pointerEventData);
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            hit.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);        //마우스 위로 가져다 댔을때
            //Debug.Log("here");
        }
            


        tempObj = hit.collider.gameObject;
    }


   
}