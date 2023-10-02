using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("개발자 모드")]
    [SerializeField] bool DeveloperMode;
    [Tooltip("진행하고 싶은 대사번호")]
    [SerializeField] int WantIngNum = 0;

    [Header("진행중인 대사 번호")]
    [SerializeField] int IngNum = 0;
    [Header("오브젝트 확인용")]
    [Tooltip("(확인)VR 튜토리얼 레이저")]
    [SerializeField] GameObject Laser;
    [Tooltip("(확인)VR 튜토리얼 진행 UI")]
    [SerializeField] GameObject UI;
    [Tooltip("(확인)VR 튜토리얼 UI 패널")]
    [SerializeField] GameObject UIWid;
    [Tooltip("(확인)활")]
    [SerializeField] GameObject Bow;
    [Tooltip("(확인)활 시위 포인트")]
    [SerializeField] GameObject ArrowPoint;
    [Tooltip("(확인)몬스터 스폰 포인트")]
    [SerializeField] Transform SPos;
    [SerializeField] Transform SPos2;
    [Tooltip("(확인)딜 타워")]
    [SerializeField] GameObject FireTower;
    [Tooltip("(확인)슬로우 타워")]
    [SerializeField] GameObject ElectricTower;

    [Header("타겟&몬스터 프리팹")]
    [Tooltip("(필수)튜토리얼 타겟")]
    [SerializeField] GameObject Target;
    [Tooltip("(필수)타겟 위치")]
    [SerializeField] Transform[] Pos;
    [Tooltip("(필수)몬스터 프리팹")]
    [SerializeField] GameObject[] Monster;

    [Header("나레이션")]
    [Tooltip("(확인)재생중인 오디오 소스")]
    [SerializeField] AudioSource IngAudio;
    [Tooltip("(필수)나레이션 음성클립")]
    [SerializeField] AudioClip[] t_narration;
    [SerializeField] bool ndone;                                //나레이션 반복 방지용

                            //진행중인 대사 번호
    [SerializeField] GameObject[] target; //소환되는 타겟
    [SerializeField] GameObject[] monster; //소환되는 몬스터
    
    
    bool tempflag = false;  //중간중간 사용할 플래그
    bool tempflag2 = false;
    int tempnum = 0;       //중간중간 사용할 변수

    void Start()
    {
        ndone = false;
        Laser = GameObject.Find("TutorialPointer");
        UI = GameObject.Find("UI");
        UIWid = GameObject.Find("UI_Window");
        Bow = GameObject.Find("Bow");
        ArrowPoint = GameObject.Find("arrowpoint");
        SPos = GameObject.Find("SpawnPoint").transform;
        SPos2 = GameObject.Find("SpawnPoint2").transform;
        FireTower = GameObject.Find("Tutorial_FI_Tower");
        ElectricTower = GameObject.Find("Tutorial_EL_Tower");
        IngNum = UI.GetComponent<UIDialogue>().GetIngNum();
        IngAudio = gameObject.GetComponent<AudioSource>();

       
    }


    IEnumerator DelayTime(float _delay)
    {
        yield return new WaitForSeconds(_delay);
     
    }

    void Update()
    {
        //실시간 받아오기
        Laser = GameObject.Find("TutorialPointer");
        UI = GameObject.Find("UI");
        UIWid = GameObject.Find("UI_Window");
        Bow = GameObject.Find("Bow");
        ArrowPoint = GameObject.Find("arrowpoint");
        IngNum = UI.GetComponent<UIDialogue>().GetIngNum();

        if (DeveloperMode)   //개발자 모드일때
        {
            for (int i = 0; i < WantIngNum; i++)                           //원하는 대사 수만큼
            {
                UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
            }
            Debug.Log("이동완료");
            DeveloperMode = false;
        }

        if (Laser!=null&&Laser.activeSelf)
        {
            switch (IngNum)
            {
                case 0:                                                         //첫 대사 나올때                                
                    Laser.GetComponent<LineRenderer>().enabled = true;          //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    //IngAudio.clip = t_narration[IngNum];                        //진행중인 대사번호의 나레이션

                    break;
                case 1:                                                         //첫 대사를 넘어간 후의 동작
                    Laser.GetComponent<LineRenderer>().enabled = false;         //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;
                    //UIWid.GetComponent<BoxCollider>().enabled = false;      //UI의 콜라이더를 비활성화(클릭 넘어가기 방지)

                    if (Bow.GetComponent<OVRGrabbable>().isGrabbed)            //활대를 잡을 경우
                    {
                        //Debug.Log("grab!");
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }
                    break;
                case 2:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 3:
                    Laser.GetComponent<LineRenderer>().enabled = false;               //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;
                    if (ArrowPoint.GetComponent<PointManager>().isPoint())            //시위 포인트를 잡을 경우
                    {
                        tempflag = true;
                    }
                    else
                    {
                        if (tempflag)   //시위 포인트를 한번이라도 잡았다가 놓았을 경우
                        {
                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            break;
                        }
                    }
                    break;
                case 4:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 5:
                    break;
                case 6:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    if (tempflag)
                    {
                        for (int i = 0; i < target.Length; i++)
                        {
                            target[i] = Instantiate(Target, Pos[i]); //각 포인트에 타겟 생성
                            target[i].GetComponent<enemy>().SetMaxHP(2);
                        }
                        tempflag = false;
                    }

                    if (target[0] == null && target[1] == null && target[2] == null)   //모두 null일때(모든 타겟을 다 죽였을때)
                    {
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }
                    break;
                case 7:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    tempflag = true;
                    break;
                case 8:
                    break;
                case 9:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    FireTower.GetComponent<Tower_fire>().enabled = false;       //타워 스크립트 비활성화
                    ElectricTower.GetComponent<Tower_fire>().enabled = false;


                    if (tempflag && monster[0] == null)
                    {
                        monster[0] = Instantiate(Monster[0], SPos); //고블린 생성
                        tempflag = false;
                    }

                    if (monster[0].GetComponent<AI_EnemyIndex>().EnemyHP <= 0)                                //몬스터가 죽으면
                    {
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }

                    if (!tempflag && monster[0].GetComponent<AI_EnemyIndex>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                    {
                        Destroy(monster[0]);
                        tempflag = true;
                    }


                    break;
                case 10:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 11:
                    tempflag = true;
                    FireTower.GetComponent<Tower_fire>().enabled = true;       //타워 스크립트 활성화
                    ElectricTower.GetComponent<Tower_fire>().enabled = true;
                    break;
                case 12:

                    if (tempflag && monster[0] == null)
                    {
                        monster[0] = Instantiate(Monster[4], SPos); //밴딧 생성
                        tempflag = false;
                    }

                    if (monster[0].GetComponent<AI_EnemyIndex>().EnemyHP <= 0)                                //몬스터가 죽으면
                    {
                        Destroy(monster[0]);
                        tempflag = true;
                        break;
                    }

                    if (!tempflag && monster[0].GetComponent<AI_EnemyIndex>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                    {
                        Destroy(monster[0]);
                        tempflag = true;
                    }

                    break;
                case 13:
                    if (tempflag && monster[0] == null)
                    {
                        monster[0] = Instantiate(Monster[4], SPos); //밴딧 생성
                        tempflag = false;
                    }

                    if (monster[0].GetComponent<AI_EnemyIndex>().EnemyHP <= 0)                                //몬스터가 죽으면
                    {
                        Destroy(monster[0]);
                        tempflag = true;
                        break;
                    }

                    if (!tempflag && monster[0].GetComponent<AI_EnemyIndex>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                    {
                        monster[0].GetComponent<AI_EnemyIndex>().EnemyHP = 0;
                        Destroy(monster[0]);
                        tempflag = true;
                    }
                    break;
                case 14:
                    if(monster[0]!=null)
                        Destroy(monster[0]);
                    FireTower.GetComponent<Tower_fire>().enabled = false;       //타워 스크립트 비활성화
                    ElectricTower.GetComponent<Tower_fire>().enabled = false;

                    break;
                case 15:

                    break;
                case 16:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    if (Bow.GetComponent<OVRGrabbable>().isGrabbed)           //활을 잡고 있고
                    {
                        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))  //왼손 트리거를 누르고 있다면=스킬 영역 활성화
                        {
                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            break;
                        }
                    }

                    break;
                case 17:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 18:
                    break;
                case 19:
                    break;
                case 20:
                    Destroy(target[0]);
                    break;
                case 21:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    if (Bow.GetComponent<cBow>().skillvalue == SkillValue.Fire)  //스킬을 파이어으로 설정하면
                    {
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }

                    break;
                case 22:
                    if (target[0] == null)
                    {
                        target[0] = Instantiate(Target, Pos[0]); //A포인트에 타겟 생성
                        target[0].GetComponent<enemy>().SetMaxHP(10);
                    }

                    //target[0].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임


                    if (target[0].GetComponent<enemy>().lastskillvalue == SkillValue.Fire) //타겟이 파이어샷을 맞을 경우
                    {
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }

                    break;
                case 23:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 24:
                    break;
                case 25:
                    tempflag = true;
                    FireTower.GetComponent<Tower_fire>().enabled = false;       //타워 스크립트 비활성화
                    ElectricTower.GetComponent<Tower_fire>().enabled = false;
                    break;
                case 26:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    if (tempflag && monster[0] == null)
                    {
                        monster[0] = Instantiate(Monster[1], SPos); //트롤 생성
                        tempflag = false;
                    }
                    if (!tempflag && monster[0].GetComponent<AI_Tower_destroyer_Enemy>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                    {
                        Destroy(monster[0]);
                        tempflag = true;
                    }

                    if (monster[0] != null && monster[0].GetComponent<AI_Tower_destroyer_Enemy>().sv == SkillValue.Fire)     //파이어 샷을 맞은 경우라면                           
                    {
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }
                    else if (monster[0] != null && monster[0].GetComponent<AI_Tower_destroyer_Enemy>().EnemyHP <= 0)     //파이어샷이 아닌 화살에 죽은 경우
                    {
                        tempflag = true;
                    }
                    
                    break;
                case 27:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 28:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    if (Bow.GetComponent<cBow>().skillvalue == SkillValue.Ice)  //스킬을 아이스으로 설정하면
                    {
                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }
                    tempnum = 2;
                    Destroy(target[0]);
                    break;
                case 29:
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            if (i == 0)
                                target[i] = Instantiate(Target, Pos[0]); //정면 앞 포인트에 타겟 생성
                            else
                                target[i] = Instantiate(Target, Pos[4]); //정면 뒤 포인트에 타겟 생성

                            target[i].GetComponent<enemy>().SetMaxHP(10);
                        }

                        target[i].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임


                        if (target[i].GetComponent<enemy>().lastskillvalue == SkillValue.Ice) //타겟이 아이스샷을 맞을 경우
                        {

                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            break;
                        }
                    }
                    break;
                case 30:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;

                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            if (i == 0)
                                target[i] = Instantiate(Target, Pos[0]); //정면 앞 포인트에 타겟 생성
                            else
                                target[i] = Instantiate(Target, Pos[4]); //정면 뒤 포인트에 타겟 생성

                            target[i].GetComponent<enemy>().SetMaxHP(10);
                        }

                        target[i].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임

                    }
                    break;
                case 31:
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            if (i == 0)
                                target[i] = Instantiate(Target, Pos[0]); //정면 앞 포인트에 타겟 생성
                            else
                                target[i] = Instantiate(Target, Pos[4]); //정면 뒤 포인트에 타겟 생성

                            target[i].GetComponent<enemy>().SetMaxHP(10);
                        }

                        target[i].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임

                    }
                    break;
                case 32:
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            if (i == 0)
                                target[i] = Instantiate(Target, Pos[0]); //정면 앞 포인트에 타겟 생성
                            else
                                target[i] = Instantiate(Target, Pos[4]); //정면 뒤 포인트에 타겟 생성

                            target[i].GetComponent<enemy>().SetMaxHP(10);
                        }

                        target[i].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임

                    }

                    break;
                case 33:
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            if (i == 0)
                                target[i] = Instantiate(Target, Pos[0]); //정면 앞 포인트에 타겟 생성
                            else
                                target[i] = Instantiate(Target, Pos[4]); //정면 뒤 포인트에 타겟 생성

                            target[i].GetComponent<enemy>().SetMaxHP(10);
                        }

                        target[i].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임

                    }

                    tempflag = true;
                    break;
                case 34:

                    if (tempflag)
                    {
                        if(GameObject.Find("DamagePlane(Clone)"))               //아이스샷이 남아있으면 삭제
                        {
                            Destroy(GameObject.Find("DamagePlane(Clone)"));
                        }

                        for (int i = 0; i < tempnum; i++)
                        {
                            if (target[i] != null)
                                Destroy(target[i]);
                        }

                        monster[0] = Instantiate(Monster[0], SPos); //고블린 생성
                        monster[1] = Instantiate(Monster[2], SPos); //리자드 생성

                        //몬스터들 멈춰서 대기
                        monster[0].GetComponent<AI_EnemyIndex>().stiff = true;
                        monster[1].GetComponent<AI_Enemy_buf>().stiff = true;

                        monster[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; //프리즈
                        monster[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; //프리즈

                        tempflag = false;
                    }

                    //몬스터들 멈춰서 대기
                    monster[0].GetComponent<AI_EnemyIndex>().stiff = true;
                    monster[1].GetComponent<AI_Enemy_buf>().stiff = true;

                    //hp계속 주입
                    monster[0].GetComponent<AI_EnemyIndex>().EnemyHP = monster[0].GetComponent<HP_M>().Hp;
                    monster[1].GetComponent<AI_Enemy_buf>().EnemyHP = monster[1].GetComponent<HP_M>().Hp;

                    //개발자 모드용(삭제 해도됨)
                    FireTower.GetComponent<Tower_fire>().enabled = false;       
                    ElectricTower.GetComponent<Tower_fire>().enabled = false;
                    break;
                case 35:
                    if (!tempflag)
                    {
                        Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                        Laser.GetComponent<vrPointer>().enabled = false;
                        //몬스터들 대기 풀기
                        monster[0].GetComponent<AI_EnemyIndex>().stiff = false;
                        monster[1].GetComponent<AI_Enemy_buf>().stiff = false;

                        monster[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;   //프리즈 해제
                        monster[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    }

                    for (int i = 0; i < tempnum; i++)
                    {
                        if (tempflag && monster[i] == null)
                        {
                            if (i == 0)
                                monster[i] = Instantiate(Monster[0], SPos); //고블린 생성
                            if (i == 1)
                                monster[i] = Instantiate(Monster[2], SPos); //리자드 생성

                            tempflag = false;
                        }
                        else if (monster[i] != null)
                        {
                            if (monster[i].GetComponent<AI_EnemyIndex>())
                            {
                                if(monster[i].GetComponent<AI_EnemyIndex>().EnemyHP <= 0)     //파이어샷이 아닌 화살에 죽은 경우
                                     tempflag = true;
                            }
                            if (monster[i].GetComponent<AI_Enemy_buf>())
                            {
                                if (monster[i].GetComponent<AI_Enemy_buf>().EnemyHP <= 0)     //파이어샷이 아닌 화살에 죽은 경우
                                    tempflag = true;
                            }
                        }

                        if (monster[i].GetComponent<AI_EnemyIndex>())
                        {
                            if (monster[i].GetComponent<AI_EnemyIndex>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                            {
                                Destroy(monster[i]);
                                tempflag = true;
                            }
                        }
                        if (monster[i].GetComponent<AI_Enemy_buf>())
                        {
                            if (monster[i].GetComponent<AI_Enemy_buf>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                            {
                                Destroy(monster[i]);
                                tempflag = true;
                            }
                        }



                        if (monster[i].GetComponent<AI_EnemyIndex>() != null && monster[i].GetComponent<AI_EnemyIndex>().sv == SkillValue.Ice) //고블린이 아이스샷을 맞을 경우
                        {

                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            break;
                        }
                        else if (monster[i].GetComponent<AI_Enemy_buf>() != null && monster[i].GetComponent<AI_Enemy_buf>().sv == SkillValue.Ice) //리자드 아이스샷을 맞을 경우
                        {

                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            break;
                        }
                    }
                   
                    break;
                case 36:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    tempnum = 3;
                    tempflag = true;
                    break;

                case 37:
                    if(tempflag)
                    {
                        for (int i = 0; i < tempnum; i++)
                        {
                            Destroy(monster[i]);
                        }
                        if (GameObject.Find("DamagePlane(Clone)"))               //아이스샷이 남아있으면 삭제
                        {
                            Destroy(GameObject.Find("DamagePlane(Clone)"));
                        }
                        tempflag = false;
                    }
                   
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;
                    
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            switch(i)
                            {
                                case 0:
                                    target[i] = Instantiate(Target, Pos[3]); //왼쪽 포인트에 타겟 생성
                                    break;
                                case 1:
                                    target[i] = Instantiate(Target, Pos[4]); //정면 포인트에 타겟 생성
                                    break;
                                case 2:
                                    target[i] = Instantiate(Target, Pos[5]); //오른쪽 포인트에 타겟 생성
                                    break;
                            }
                            

                            target[i].GetComponent<enemy>().SetMaxHP(6);
                        }

                        //target[i].GetComponent<enemy>().horizontal = true;  //타겟 수평 움직임


                        if (target[i].GetComponent<enemy>().lastskillvalue == SkillValue.Lightning) //타겟이 라이트닝샷을 맞을 경우
                        {

                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            break;
                        }
                    }
                    break;
                case 38:
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;

                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            switch (i)
                            {
                                case 0:
                                    target[i] = Instantiate(Target, Pos[3]); //왼쪽 포인트에 타겟 생성
                                    break;
                                case 1:
                                    target[i] = Instantiate(Target, Pos[4]); //정면 포인트에 타겟 생성
                                    break;
                                case 2:
                                    target[i] = Instantiate(Target, Pos[5]); //오른쪽 포인트에 타겟 생성
                                    break;
                            }

                            target[i].GetComponent<enemy>().SetMaxHP(6);
                        }
                    }
                    break;
                case 39:
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            switch (i)
                            {
                                case 0:
                                    target[i] = Instantiate(Target, Pos[3]); //왼쪽 포인트에 타겟 생성
                                    break;
                                case 1:
                                    target[i] = Instantiate(Target, Pos[4]); //정면 포인트에 타겟 생성
                                    break;
                                case 2:
                                    target[i] = Instantiate(Target, Pos[5]); //오른쪽 포인트에 타겟 생성
                                    break;
                            }


                            target[i].GetComponent<enemy>().SetMaxHP(6);
                        }
                    }
                    break;
                case 40:
                    for (int i = 0; i < tempnum; i++)
                    {
                        if (target[i] == null)
                        {
                            switch (i)
                            {
                                case 0:
                                    target[i] = Instantiate(Target, Pos[3]); //왼쪽 포인트에 타겟 생성
                                    break;
                                case 1:
                                    target[i] = Instantiate(Target, Pos[4]); //정면 포인트에 타겟 생성
                                    break;
                                case 2:
                                    target[i] = Instantiate(Target, Pos[5]); //오른쪽 포인트에 타겟 생성
                                    break;
                            }


                            target[i].GetComponent<enemy>().SetMaxHP(6);
                        }
                    }
                    tempflag = true;
                    tempnum = 3;
                    break;
                case 41:
                    if (tempflag)
                    {
                        
                        for (int i = 0; i < tempnum; i++)
                        {
                            Destroy(target[i]);
                        }
                        if (GameObject.Find("DamagePlane(Clone)"))               //남아있으면 삭제
                        {
                            Destroy(GameObject.Find("DamagePlane(Clone)"));
                        }
                        tempnum = 2;

                        monster[0] = Instantiate(Monster[0], SPos); //고블린 생성
                        monster[1] = Instantiate(Monster[3],SPos2); //봄버 생성


                        //몬스터들 멈춰서 대기
                        monster[0].GetComponent<AI_EnemyIndex>().stiff = true;
                        monster[1].GetComponent<Suicide_Enemy>().stiff = true;

                        monster[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; //프리즈
                        monster[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; //프리즈

                        tempflag = false;
                    }

                    //몬스터들 멈춰서 대기
                    monster[0].GetComponent<AI_EnemyIndex>().stiff = true;
                    monster[1].GetComponent<Suicide_Enemy>().stiff = true;
                    //hp계속 주입
                    monster[0].GetComponent<AI_EnemyIndex>().EnemyHP = monster[0].GetComponent<HP_M>().Hp;
                    monster[1].GetComponent<Suicide_Enemy>().EnemyHP = monster[1].GetComponent<HP_M>().Hp;


                    //개발자 모드용(삭제 해도됨)
                    FireTower.GetComponent<Tower_fire>().enabled = false;
                    ElectricTower.GetComponent<Tower_fire>().enabled = false;

                    break;
                case 42:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;

                    if (!tempflag&&monster[0]!=null && monster[1] != null)
                    {
                        //몬스터들 대기 풀기
                        monster[0].GetComponent<AI_EnemyIndex>().stiff = false;
                        monster[1].GetComponent<Suicide_Enemy>().stiff = false;

                        monster[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;   //프리즈 해제
                        monster[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        tempflag = true;
                    }

                    for (int i = 0; i < tempnum; i++)
                    {
                        if (tempflag && monster[i] == null)
                        {
                            if (i == 0)
                                monster[i] = Instantiate(Monster[0], SPos); //고블린 생성
                            if (i == 1)
                                monster[i] = Instantiate(Monster[3], SPos2); //봄버 생성

                            tempflag = false;
                        }
                        else if(monster[i] != null)
                        {
                            if (monster[i].GetComponent<AI_EnemyIndex>())
                            {
                                if (monster[i].GetComponent<AI_EnemyIndex>().EnemyHP <= 0)     //파이어샷이 아닌 화살에 죽은 경우
                                    tempflag = true;
                            }
                            if (monster[i].GetComponent<Suicide_Enemy>())
                            {
                                if (monster[i].GetComponent<Suicide_Enemy>().EnemyHP <= 0)     //파이어샷이 아닌 화살에 죽은 경우
                                    tempflag = true;
                            }
                        }

                        if (monster[i] != null&&monster[i].GetComponent<AI_EnemyIndex>()!=null)
                        {
                            if (monster[i].GetComponent<AI_EnemyIndex>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                            {
                                Destroy(monster[i]);
                                tempflag = true;
                            }
                        }
                        if (monster[i] != null && monster[i].GetComponent<Suicide_Enemy>()!=null)
                        {
                            if (monster[i].GetComponent<Suicide_Enemy>().GetAtk_ON()) //공격 모션이 나올때=타워에 도달했을때
                            {
                                Destroy(monster[i]);
                                tempflag = true;
                            }
                        }
                        if (monster[i] != null && monster[i].GetComponent<AI_EnemyIndex>() != null && monster[i].GetComponent<AI_EnemyIndex>().sv == SkillValue.Lightning) //고블린이 아이스샷을 맞을 경우
                        {

                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            tempflag = true;
                            break;
                        }
                        else if (monster[i] != null && monster[i].GetComponent<Suicide_Enemy>() != null && monster[i].GetComponent<Suicide_Enemy>().sv == SkillValue.Lightning) //리자드 아이스샷을 맞을 경우
                        {

                            UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                            tempflag = true;
                            break;
                        }
                    }
                    break;
                case 43:
                    /*
                    if(tempflag)
                    {
                        for (int i = 0; i < tempnum; i++)
                        {
                            Destroy(monster[i]);
                        }
                        if (GameObject.Find("DamagePlane(Clone)"))               //아이스샷이 남아있으면 삭제
                        {
                            Destroy(GameObject.Find("DamagePlane(Clone)"));
                        }

                        tempflag = false;
                    }
                    */
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;
                    break;
                case 44:
                    tempflag = true;
                    break;
                case 45:
                    Laser.GetComponent<LineRenderer>().enabled = false;  //선택 레이저 비활성화
                    Laser.GetComponent<vrPointer>().enabled = false;
                    if (OVRInput.GetDown(OVRInput.RawButton.A))  //a버튼을 누르면
                    {
                        gameObject.GetComponent<AudioSource>().Stop();
                        gameObject.GetComponent<AudioSource>().clip = t_narration[IngNum+1];
                        gameObject.GetComponent<AudioSource>().Play();
                        tempflag2 = true;

                        UI.GetComponent<UIDialogue>().NextIngNum();        //다음 대사로 이동
                        break;
                    }
                    break;
                case 46:
                    if (OVRInput.Get(OVRInput.RawButton.A))  //a버튼을 누르면
                    {
                        Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                        Laser.GetComponent<vrPointer>().enabled = true;

                        break;
                    }
                    break;
                case 47:
                    if(tempflag2)
                    {
                        gameObject.GetComponent<AudioSource>().Stop();
                        gameObject.GetComponent<AudioSource>().clip = t_narration[IngNum + 1];
                        gameObject.GetComponent<AudioSource>().Play();
                        tempflag2 = false;
                    }
                    
                    Laser.GetComponent<LineRenderer>().enabled = true;  //선택 레이저 활성화
                    Laser.GetComponent<vrPointer>().enabled = true;

                    break;
                case 48:
                    if (!tempflag2)
                    {
                        gameObject.GetComponent<AudioSource>().Stop();
                        gameObject.GetComponent<AudioSource>().clip = t_narration[IngNum];
                        gameObject.GetComponent<AudioSource>().Play();
                        tempflag2 = true;
                    }
                    
                    break;
            }

            if(!tempflag2)
            {
                if (IngAudio.clip != null && IngAudio.clip != t_narration[IngNum])              //다음 대사로 넘어간 경우 다음대사로 빠르게 교체
                {
                    IngAudio.Stop();
                    ndone = false;

                }
                if (!ndone && !IngAudio.isPlaying)                                            //오디오가 멈춰있을때
                {
                    IngAudio.clip = t_narration[IngNum];
                    IngAudio.Play();
                    ndone = true;
                    //IngAudio.PlayOneShot(t_narration[IngNum]);                  //진행중인 대사번호의 나레이션 한번만 실행
                }
            }
            
        }

    }
}
