using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowState     //화살 상태
{
    OverHand,       //손 위에 있음
    OverBow,        //활 위에 있음
    Shoot,          //활을 쏘는 순간
    Piew,           //날아가는중
}


public class ArrowController : MonoBehaviour
{
    GameObject arrowbody;
    GameObject hand;
    GameObject arrowPos;
    GameObject bow;
    GameObject lookatthis;
    Rigidbody rigd;
    bool hit = false;
    float temptime = 0f;
    float ticktime = 0f;
    public bool done = false;
    public ArrowState astate;
    bool useEffect = false;
    bool imfull = false;                        //한명만 효과 적용
    bool useSound = false;                      //효과음 한번만
    GameObject mytarget;                        //피격된 대상(1명)
    SkillValue tempeffet;                       //전 이펙트


    [Header("Arrow-BasicSetting")]
    [Tooltip("(필수)화살 속도를 설정해주세요.")]
    [Range(0, 5)]
    [SerializeField] float arrspeed;                //화살 속도
    [Tooltip("(필수)비활동 화살의 파괴시간을 설정해주세요.")]
    [SerializeField] float destroytime;             //파괴 시간
    [SerializeField] Transform startpos;            //포물선 시작위치
    [SerializeField] Vector3 endpos;                //포물선 종료위치

    [Header("Skill")]
    [SerializeField] SkillValue askillvalue;        //확인용
    [SerializeField] Skill askill;                  //활에 장착된 스킬 상태
    [SerializeField] SkillData skilldata;           //스킬 데이터
    [Tooltip("(필수)범위 공격 프리팹을 설정해주세요.")]
    [SerializeField] GameObject damageplane;        //범위 공격

    [Header("Destination")]
    [Tooltip("(필수)목적지 프리팹을 설정해주세요.")]
    [SerializeField] GameObject destination;    //목적지 객체
    [Tooltip("(필수)목적지 프리팹 크기를 설정해주세요.")]
    [Range(0, 5)]
    [SerializeField] float destSize;            //목적지 객체 크기
    [Tooltip("(필수)시위를 당기지 않았을때의 거리값을 설정해주세요.")]
    [Range(1, 150)]
    [SerializeField] float tempdesttrs;         //기본 목적지 거리,시위를 당기고 있지 않을때(인스펙터에서 값을 주세요!)
    [SerializeField] float desttrs;             //목적지 거리(인스펙터에서 값을 주세요!)

    [Header("이펙트 설정")]
    [SerializeField] GameObject[] effect_skill;
    [Tooltip("콜라이더 둘레")]
    public float colliderRadius = 1f;
    [Range(0f, 1f)]
    public float collideOffset = 0.15f;


    void Start()
    {
        destination = Instantiate(destination, new Vector3(gameObject.transform.position.x,
        gameObject.transform.position.y - 10f, gameObject.transform.position.z + tempdesttrs), Quaternion.identity);     //목적지 생성

        destination.transform.localScale = new Vector3(destSize, destSize, destSize);   //목적지 크기 설정
        desttrs = tempdesttrs;
        destination.transform.SetParent(gameObject.transform);              //활을 부모로 설정(같이 움직이기 위해서!)

        astate = ArrowState.OverBow;
        arrowbody = GameObject.Find("Arrowbody");   //화살 몸통
        hand = GameObject.Find("CustomHandRight"); //오른손 읽기
        arrowPos = GameObject.Find("arrowpoint"); //화살 장착 좌표
        bow = GameObject.Find("Bow");
        lookatthis = GameObject.Find("LookPoint");
        rigd = GetComponent<Rigidbody>();
        startpos = transform;
        endpos = destination.transform.position;
        imfull = false;
        tempeffet = SkillValue.Non;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        skilldata = GameObject.Find("SkillWindow(30do)(Clone)").GetComponent<SkillData>();
        if (destination != null)
        {
            destination.transform.localPosition = new Vector3(0, 0, desttrs);   //설정한 거리만큼 유지
            //Debug.Log(gameObject.name+"Arrow:"+ desttrs);
        }


        switch (astate)
        {
            case ArrowState.OverBow:                                            //활에 장착
                gameObject.transform.position = arrowPos.transform.position;
                gameObject.transform.LookAt(lookatthis.transform);

                //rigd.freezeRotation = true;
                SkillArrowSetting();    //화살에 스킬 정보 전달

                if (askill.value == SkillValue.Non)
                {
                    useEffect = false;
                    gameObject.transform.Find("ArrowEffect").gameObject.SetActive(false);
                }
                else
                {
                    gameObject.transform.Find("ArrowEffect").gameObject.SetActive(true);            //스킬 이펙트 활성화
                    useEffect = true;   //활성화


                    if (useEffect)
                    {
                        switch (askill.value)                               //활대 기울기에 따라 스킬 이펙트 키기
                        {
                            case SkillValue.Fire:
                                effect_skill[0].SetActive(true);    //파이어
                                effect_skill[1].SetActive(false);   //아이스
                                effect_skill[2].SetActive(false);   //라이트닝
                               
                                if (tempeffet == SkillValue.Non||tempeffet!=SkillValue.Fire)    //그 전 이펙트가 보통 화살이였거나 파이어샷이 아니였을때
                                {
                                    useSound = true;
                                    tempeffet = SkillValue.Fire;
                                }
                                break;
                            case SkillValue.Ice:
                                effect_skill[0].SetActive(false);
                                effect_skill[1].SetActive(true);
                                effect_skill[2].SetActive(false);

                                if (tempeffet == SkillValue.Non || tempeffet != SkillValue.Ice)    //그 전 이펙트가 보통 화살이였거나 아이스샷이 아니였을때
                                {
                                    useSound = true;
                                    tempeffet = SkillValue.Ice;
                                }
                                break;
                            case SkillValue.Lightning:
                                effect_skill[0].SetActive(false);
                                effect_skill[1].SetActive(false);
                                effect_skill[2].SetActive(true);
                                
                                if (tempeffet == SkillValue.Non || tempeffet != SkillValue.Lightning)    //그 전 이펙트가 보통 화살이였거나 라이트닝샷이 아니였을때
                                {
                                    useSound = true;
                                    tempeffet = SkillValue.Lightning;
                                }
                                break;
                        }
                        
                        for(int i=0;i<3;i++)
                        {
                            if(effect_skill[i].activeSelf&&useSound)          //효과가 켜져있는 경우
                            {
                                effect_skill[i].transform.GetChild(0).gameObject.GetComponent<AudioSource>().PlayOneShot(
                                    effect_skill[1].transform.GetChild(0).gameObject.GetComponent<AudioSource>().clip);    //폭발음 재생(파이어샷 때문에 아이스샷 고정으로)
                                useSound = false;
                            }
                        }
                        
                    }
                }

                break;

            case ArrowState.Shoot:
                if(askill.value==SkillValue.Non&& !gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    gameObject.GetComponent<AudioSource>().PlayOneShot(gameObject.GetComponent<AudioSource>().clip);
                }

                startpos = transform;
                //Debug.Log("StartPos:" + startpos);
                if (destination != null)
                {
                    endpos = destination.transform.position;
                    Destroy(destination);               //목적지 삭제(조준선 엉킴 방지)
                }

                astate = ArrowState.Piew;
                break;

            case ArrowState.Piew:
                //Debug.Log("날아가는중");

                Shoot();
                if (!hit && useEffect)
                {
                    effect_skill[((int)askill.value) - 1].transform.GetChild(0).gameObject.SetActive(false);
                    effect_skill[((int)askill.value) - 1].transform.GetChild(1).gameObject.SetActive(true);
                    effect_skill[((int)askill.value) - 1].transform.GetChild(2).gameObject.SetActive(false);
                    Debug.Log(effect_skill[((int)askill.value) - 1].transform.GetChild(1).gameObject.name);
                }
                break;
        }
    }

    public SkillValue GetSkillValue()
    {
        return askillvalue;
    }


    //외부 거리 설정용
    public void SetDestDistance(float _dest)
    {
        desttrs = tempdesttrs + _dest;
    }

    public void Shoot()
    {
        rigd.velocity = Vector3.zero;
        rigd.angularVelocity = Vector3.zero;

        if (!hit)
        {
            //Debug.Log("temptime:" + temptime + ",realtime:" + Time.time);
            transform.position = Vector3.MoveTowards(startpos.position, endpos, arrspeed);
            if ((temptime += Time.deltaTime) >= destroytime)
            {
                Destroy(gameObject);
            }
        }
        else if (hit && mytarget != null && mytarget.gameObject.CompareTag("enemy")) //hit되었을때
        {
            //Debug.Log("ㅎㅎ");
            if (!askill.userange)           //범위 공격이 아니면
            {
                if (imfull)
                {
                    //Debug.Log("여기까지");
                    DistinctionAttack(mytarget.gameObject, askill);
                }
            }

            if (!askill.useskill)
            {
                askill.useskill = true;
                //Debug.Log("destroy-useskill");
                Destroy(gameObject);
            }
        }

    }

    //스킬 화살 세팅
    void SkillArrowSetting()
    {
        for (int i = 0; i < skilldata.SkillList.Count; i++)
        {
            if (bow.GetComponent<cBow>().skillvalue == skilldata.SkillList[i].value) //현재 선택된 스킬을 스킬 데이터에서 검색
            {
                if (skilldata.SkillList[i].simage != null && skilldata.SkillList[i].simage.fillAmount == 1) //스킬 이미지가 null이 아니면서 스킬의 쿨타임이 차있는 경우
                {
                    askill = skilldata.SkillList[i];    //선택된 스킬 데이터를 화살 스킬 데이터에 저장
                }
                else //스킬 쿨이 아직 안찬 경우
                {
                    askill = skilldata.SkillList[0];    //기본 공격으로 설정
                }

                askillvalue = askill.value;
            }
        }
    }


    private void OnTriggerEnter(Collider other) //트리거 영역에 들어갔을 때
    {
        if (other.tag == "point")   //활의 포인트
        {
            Debug.Log("장착!");

            if (hand.GetComponent<cHand>().hstate == HandState.TakeArrow)
                astate = ArrowState.OverBow;    //상태 변환
            if (hand.GetComponent<cHand>().hstate != HandState.TakeString)
                hand.GetComponent<cHand>().hstate = HandState.Non;     //손 상태 전환:화살 장착중->무
            if (bow.GetComponent<cBow>().bstate != BowState.Drawing)
                bow.GetComponent<cBow>().bstate = BowState.Arrowning;
        }
        else if (other.CompareTag("enemy") || other.CompareTag("wall")) //적이나 바닥
        {
            //이펙트를 사용
            if (useEffect)
            {
                //피격 이펙트 활성화
                if (effect_skill[(int)askill.value - 1].transform.childCount==4)
                {
                    effect_skill[(int)askill.value - 1].transform.GetChild(3).gameObject.SetActive(true);
                    effect_skill[(int)askill.value - 1].transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
                }
                else
                {
                    effect_skill[(int)askill.value - 1].transform.GetChild(0).gameObject.SetActive(true);
                    effect_skill[(int)askill.value - 1].transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
                }
                    
                effect_skill[(int)askill.value - 1].transform.GetChild(1).gameObject.SetActive(false);
                effect_skill[(int)askill.value - 1].transform.GetChild(2).gameObject.SetActive(false);
                

                //폭발음 재생
                //effect_skill[(int)askill.value - 1].transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
                //Handheld.Vibrate(); //헤드셋 진동
                //Debug.Log(effect_skill[(int)askill.value - 1].transform.GetChild(0).gameObject.name);
            }

            Debug.Log("명중!" + other.gameObject.name);
            if(arrowbody)
                arrowbody.SetActive(false);     //몸통만 안보이게(눈속임)


            //아직 비피격상태이고 범위 공격일때
            if (!hit && askill.userange)
            {
                Vector3 ground = transform.position;                          //범위 공격이 설치될 대략적인 땅의 좌표를 입력받을 변수
                if (other.CompareTag("enemy"))
                {
                    //범위 공격일때 몬스터가 화살을 첫타를 맞을시 화살을 피격당한 몬스터만 추가 데미지
                    CheckEnemyDamage(other.gameObject, askill.damage);

                    ground = new Vector3(ground.x, (ground.y / 3)*2, ground.z); //몬스터 키의 반값 빼서 땅에 생성
                }
                GameObject dmgplane = Instantiate(damageplane, ground, Quaternion.identity) as GameObject;    //원통형 공격 범위 생성

                //스킬 종류에 따라 맞는 설치물 이펙트를 활성화
                switch (askill.value)
                {
                    case SkillValue.Ice://얼음
                        dmgplane.transform.Find("SpikeWaveIce").gameObject.SetActive(true);
                        break;
                    case SkillValue.Lightning://전기
                        dmgplane.transform.Find("LightningShield").gameObject.SetActive(true);
                        break;
                }

                dmgplane.GetComponent<DamagePlane>().SettingOption(askill);  //범위 공격에 스킬값 전달
                hit = true;                                                  //화살 타격 확인
                //Debug.Log("destroy-userange2");
                Destroy(gameObject, 1f);                                      //화살 삭제(파괴 이펙트가 나올 1초 대기)
            }
            else if (!hit && !askill.userange)
            {
                hit = true;
                temptime = 0;
                //튜토리얼용
                if (other.gameObject.GetComponent<enemy>() != null)
                    other.gameObject.GetComponent<enemy>().SkillSetting(askill);      //데미지 받고 있는 스킬 전달
                if (other.gameObject.GetComponent<AI_EnemyIndex>() != null)
                    other.gameObject.GetComponent<AI_EnemyIndex>().SkillSetting(askill);      //데미지 받고 있는 스킬 전달
                if (other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                    other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>().SkillSetting(askill);      //데미지 받고 있는 스킬 전달
                if (other.gameObject.GetComponent<AI_Enemy_buf>() != null)
                    other.gameObject.GetComponent<AI_Enemy_buf>().SkillSetting(askill);      //데미지 받고 있는 스킬 전달
                if (other.gameObject.GetComponent<throw_Enemy>() != null)
                    other.gameObject.GetComponent<throw_Enemy>().SkillSetting(askill);      //데미지 받고 있는 스킬 전달
                if (other.gameObject.GetComponent<Suicide_Enemy>() != null)
                    other.gameObject.GetComponent<Suicide_Enemy>().SkillSetting(askill);      //데미지 받고 있는 스킬 전달

                if (other.CompareTag("enemy") && !imfull)
                {
                    transform.parent = other.gameObject.transform; //맞춘 대상의 자식으로 설정(화살의 위치 때문에)
                    mytarget = other.gameObject;        //타겟 고정
                    StartCoroutine(DelaySecond(0.3f));    //0.3초 딜레이
                }
                else if (other.CompareTag("wall"))
                {
                    Destroy(gameObject, 1f);
                }
            }
        }
        else
        {
            //Debug.Log(other.gameObject.name); //확인용
        }
    }

    /*화살 속도로 인한 콜라이터 뚫림 방지용
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy") || other.CompareTag("wall")) //적이나 바닥
        {
            arrspeed = 0f;  //화살속도 초기화(화살 멈추기)
            rigd.constraints= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ; //프리즈
            hit = true;
        }
    }*/

    IEnumerator DelaySecond(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        imfull = true;
    }

    private void CheckEnemyDamage(GameObject _enemy, float _damage)
    {
        if (_enemy.gameObject.GetComponent<enemy>() != null)
        {
            _enemy.GetComponent<enemy>().Damaging(_damage);
        }
        else if (_enemy.GetComponent<AI_EnemyIndex>() != null)
        {
            _enemy.GetComponent<AI_EnemyIndex>().Damaging(_damage);
        }
        else if (_enemy.GetComponent<AI_Tower_destroyer_Enemy>() != null)
        {
            _enemy.GetComponent<AI_Tower_destroyer_Enemy>().Damaging(_damage);
        }
        else if (_enemy.GetComponent<AI_Enemy_buf>() != null)
        {
            _enemy.GetComponent<AI_Enemy_buf>().Damaging(_damage);
        }
        else if (_enemy.GetComponent<throw_Enemy>() != null)
        {
            _enemy.GetComponent<throw_Enemy>().Damaging(_damage);
        }
        else if (_enemy.GetComponent<Suicide_Enemy>() != null)
        {
            _enemy.GetComponent<Suicide_Enemy>().Damaging(_damage);
        }
    }

    private void CheckEnemyStiff(GameObject _enemy, bool _stiff)
    {
        if (_enemy.gameObject.GetComponent<enemy>() != null)
        {
            _enemy.GetComponent<enemy>().stiff = _stiff;

        }
        else if (_enemy.GetComponent<AI_EnemyIndex>() != null)
        {
            _enemy.GetComponent<AI_EnemyIndex>().stiff = _stiff;

        }
        else if (_enemy.GetComponent<AI_Tower_destroyer_Enemy>() != null)
        {
            _enemy.GetComponent<AI_Tower_destroyer_Enemy>().stiff = _stiff;

        }
        else if (_enemy.GetComponent<AI_Enemy_buf>() != null)
        {
            _enemy.GetComponent<AI_Enemy_buf>().stiff = _stiff;
        }
        else if (_enemy.GetComponent<throw_Enemy>() != null)
        {
            _enemy.GetComponent<throw_Enemy>().stiff = _stiff;
        }
        else if (_enemy.GetComponent<Suicide_Enemy>() != null)
        {
            _enemy.GetComponent<Suicide_Enemy>().stiff = _stiff;
        }

        if (_stiff == false)
        {
            Debug.Log("stiff!");

            temptime = 0;
            ticktime = 0;
            done = true;
        }
    }

    public void DistinctionAttack(GameObject _enemy, Skill _attack)
    {
        done = false;
        if (temptime == 0 && ticktime == 0)
        {
            temptime += Time.deltaTime;
        }
        if (temptime >= _attack.skilltime)
        {
            Debug.Log("enemy:" + gameObject.name + "초기화!");
            CheckEnemyStiff(_enemy, false);

            if (done)   //모든 마무리 작업이 끝날 경우
            {
                _attack.useskill = false;
                Debug.Log("destroy-done");
                Destroy(gameObject);
            }
        }
        else if (temptime < _attack.skilltime)   //스킬 지속 시간동안
        {
            if (_attack.stiff)   //경직일때
            {
                CheckEnemyStiff(_enemy, true);
            }

            float tick = (int)temptime % ((int)(_attack.skilltime / _attack.damagecount));    //tick이 0이 될때가 틱 공격 타이밍

            //Debug.Log("enemy:" + _enemy.name + "/ticktime:" + ticktime + "/temptime:" + temptime + "/tick:" + ((int)(_attack.skilltime / _attack.damagecount)) + ",realtime:" + Time.time);
            if (tick == 0 && ticktime == (int)temptime)      //데미지 타격 횟수가 남아있을때
            {
                Debug.Log("enemy:" + _enemy.name + "/damage:" + _attack.damage);
                ticktime += ((int)(_attack.skilltime / _attack.damagecount));
                if (temptime != _attack.skilltime && tick != ticktime)//수정
                {
                    //몬스터가 가진 스크립트 확인 후 데미지
                    CheckEnemyDamage(_enemy, _attack.damage);
                }
            }

            temptime += Time.deltaTime;
            //Debug.Log("enemy:" + _enemy.name + "/ticktime:" + ticktime + "/temptime:" + temptime + "/attackskilltime:" + ((int)(_attack.skilltime)));
        }


    }

}