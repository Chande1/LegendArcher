using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DamagePlane : MonoBehaviour
{
    public Skill ps;                            //스킬
    [SerializeField] SkillValue psv;            //스킬종류
    [SerializeField] float dmgrange = 0f;       //공격범위

    [SerializeField] float temptime = 0f;       //스킬시간
    float ticktime = 0f;                        //틱 횟수
    [SerializeField] public bool dmg = false;   //데미지 식별
    [SerializeField] bool same = false;         //같은 적인지 식별
    [SerializeField] bool stiffmoment = false;   //경직 식별
    [SerializeField] List<GameObject> enemys = new List<GameObject>();  //범위 내의 적 관리를 위한 동적 리스트

    [Header("상태이상 지속시간Bar 설정")]
    [SerializeField] float maxhp = 0f;          //최대hp
    [SerializeField] float hp = 0f;             //현재hp
    [SerializeField] Slider hpbar;              //hp바img
    [SerializeField] Image barcolor;            //hp바 색깔
    [SerializeField] Text hpnum;                //hp바 text

    [Header("이펙트 사운드")]
    [SerializeField] bool sounddone;            //효과음 재생용
    [Header("아이스샷 사운드")]
    [SerializeField] AudioClip[] IceSound;        //이펙트 효과음 배열
    [Header("라이트닝샷 사운드")]
    [SerializeField] AudioClip[] LightningSound;  //이펙트 효과음 배열

    private void Start()
    {
        stiffmoment = false;
        sounddone = false;
        maxhp = ps.skilltime;   //최대 시간 설정
        hp = ps.skilltime;      //시간 채워주기
        if (ps.stiff)
            ChangingBarColor(Color.blue);
        else
            ChangingBarColor(Color.yellow);

        StartCoroutine("StiffMoment");    //1초 뒤에 경직모드 실행(경직 몬스터 마감)
    }

    private void FixedUpdate()
    {
        hpbar.value = hp / maxhp;
        hpnum.text = string.Format(hp + "/" + maxhp);

        if (temptime == 0 && ticktime == 0)
        {
            temptime = ps.skilltime;
            switch(ps.value)                //스킬 종류에 따라 등장 효과음 재생
            {
                case SkillValue.Ice:
                    gameObject.GetComponent<AudioSource>().PlayOneShot(IceSound[0]);
                    break;
                case SkillValue.Lightning:
                    gameObject.GetComponent<AudioSource>().PlayOneShot(LightningSound[0]);
                    gameObject.GetComponent<AudioSource>().loop = true;
                    break;
            }
        }
        else if (temptime > 0)   //스킬 지속 시간동안
        {
            float tick = (int)Time.deltaTime % ((int)(ps.skilltime / ps.damagecount));

            if (tick == 0 && ((int)ps.skilltime - ticktime) == Mathf.RoundToInt(temptime))      //데미지 타격 횟수가 남아있을때(float반올림)
            {
                ticktime += ((int)(ps.skilltime / ps.damagecount));

                if ((int)temptime != 0)
                {
                    Debug.Log("temptime:" + temptime);
                    dmg = true;
                }
            }
            hp = (int)temptime;
            temptime -= Time.deltaTime;
            
            if(!sounddone&&(float)(Math.Truncate(temptime*10)/10)==1.2f)     //소숫점 1자리까지 반올림
            {
                switch (ps.value)                //스킬 종류에 따라 파괴 효과음 재생
                {
                    case SkillValue.Ice:
                        gameObject.GetComponent<AudioSource>().PlayOneShot(IceSound[1]);
                        break;
                    case SkillValue.Lightning:
                        //gameObject.GetComponent<AudioSource>().PlayOneShot(LightningSound[1]);
                        break;
                }
                sounddone = true;
            }
        }
        if (temptime < 0)
        {
            DistroyPlane();                                         //스킬 지속 시간이 끝나면 삭제!
            Debug.Log("destroy1");
        }

        if (enemys.Count == 0 && temptime < 0)
        {
            Destroy(gameObject);
            Debug.Log("destroy2");
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))                              //태그가 적인 경우
        {
            //if (!stiffmoment)
                //other.GetComponent<AI_EnemyIndex>().ChangingBarColor(Color.yellow);  //피통을 노란색으로 변경


            if (!stiffmoment && ps.stiff)    //경직모드가 아니면서 경직인 경우
            {
                    if (other.gameObject.GetComponent<enemy>() != null)
                    {
                        other.GetComponent<enemy>().stiff = true;
                    }
                    if (other.gameObject.GetComponent<AI_EnemyIndex>() != null)
                    {
                        other.GetComponent<AI_EnemyIndex>().stiff = true;
                    }
                    if (other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                    {
                        other.GetComponent<AI_Tower_destroyer_Enemy>().stiff = true;
                    }
                    if (other.gameObject.GetComponent<AI_Enemy_buf>() != null)
                    {
                        other.GetComponent<AI_Enemy_buf>().stiff = true;
                    }
                    if (other.gameObject.GetComponent<throw_Enemy>() != null)
                    {
                        other.GetComponent<throw_Enemy>().stiff = true;
                    }
                    if (other.gameObject.GetComponent<Suicide_Enemy>() != null)
                    {
                        other.GetComponent<Suicide_Enemy>().stiff = true;
                    }
                    //other.GetComponent<enemy>().ChangingBarColor(Color.blue);  //파란색
                }

            if (enemys.Count == 0) //처음 객체
            {
                Debug.Log("First Add list(" + other.gameObject + ")");
                if (!stiffmoment|| (stiffmoment && other.CompareTag("enemy"))) //전기 스킬이거나 얼음 스킬이면서 아무도 없는 땅을 쐈던 경우
                {
                    Debug.Log("here");
                    stiffmoment = false;
                    enemys.Add(other.gameObject);                       //리스트에 추가
                }
            }
            for (int i = 0; i < enemys.Count; i++)                  //리스트 전체 검색
            {
                if (enemys.Count == 0)
                {
                    Debug.Log("destroy3");
                    Destroy(gameObject);
                }

                if (enemys[i].gameObject != null&&enemys[i].transform == other.gameObject.transform)        //리스트에 있는 객체인 경우
                {
                    same = true;                                    //같은 객체 있음!
                }

                if (enemys[i].gameObject == null)
                {
                    enemys.RemoveAt(i);
                    Debug.Log("1번");
                }
            }
            if (!same)                                               //같은 객체가 없는 경우
            {
                //Debug.Log("Add list(" + other.gameObject + ")");
                if (!stiffmoment)
                    enemys.Add(other.gameObject);                       //리스트에 추가
            }

            same = false;
        }

        Debug.Log("enemycount:" + enemys.Count);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy"))          //태그가 적일때
        {
            if (dmg)
            {
                if (enemys.Count == 0)
                {
                    Destroy(gameObject);
                }
                for (int i = 0; i < enemys.Count; i++)          //리스트 전체 검색
                {
                    if (enemys[i].gameObject == null)
                    {
                        enemys.RemoveAt(i);
                    }
                    //Debug.Log("Damage(" + enemys[i].gameObject + ")");
                    else if (enemys[i].gameObject.GetComponent<enemy>() != null)
                    {
                        Debug.Log("dmg" + ps.damage);
                        enemys[i].gameObject.GetComponent<enemy>().SkillSetting(ps);      //데미지 받고 있는 스킬 전달
                        enemys[i].gameObject.GetComponent<enemy>().Damaging(ps.damage);   //데미지
                    }
                    else if (enemys[i].gameObject.GetComponent<AI_EnemyIndex>() != null)
                    {
                        Debug.Log("dmg"+ps.damage);
                        enemys[i].gameObject.GetComponent<AI_EnemyIndex>().SkillSetting(ps);      //데미지 받고 있는 스킬 전달
                        enemys[i].gameObject.GetComponent<AI_EnemyIndex>().Damaging(ps.damage);   //데미지
                    }
                    else if (enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                    {
                        Debug.Log("dmg" + ps.damage);
                        enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().SkillSetting(ps);      //데미지 받고 있는 스킬 전달
                        enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().Damaging(ps.damage);   //데미지
                    }
                    else if (enemys[i].gameObject.GetComponent<AI_Enemy_buf>() != null)
                    {
                        Debug.Log("dmg" + ps.damage);
                        enemys[i].gameObject.GetComponent<AI_Enemy_buf>().SkillSetting(ps);      //데미지 받고 있는 스킬 전달
                        enemys[i].gameObject.GetComponent<AI_Enemy_buf>().Damaging(ps.damage);   //데미지
                    }
                    else if (enemys[i].gameObject.GetComponent<throw_Enemy>() != null)
                    {
                        Debug.Log("dmg" + ps.damage);
                        enemys[i].gameObject.GetComponent<throw_Enemy>().SkillSetting(ps);      //데미지 받고 있는 스킬 전달
                        enemys[i].gameObject.GetComponent<throw_Enemy>().Damaging(ps.damage);   //데미지
                    }
                    else if (enemys[i].gameObject.GetComponent<Suicide_Enemy>() != null)
                    {
                        Debug.Log("dmg" + ps.damage);
                        enemys[i].gameObject.GetComponent<Suicide_Enemy>().SkillSetting(ps);      //데미지 받고 있는 스킬 전달
                        enemys[i].gameObject.GetComponent<Suicide_Enemy>().Damaging(ps.damage);   //데미지
                    }
                }
                dmg = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy"))                          //태그가 적인 경우
        {
            for (int i = 0; i < enemys.Count; i++)              //리스트 전체 검색
            {
                if (enemys.Count == 0)
                {
                    Destroy(gameObject);
                }
                if (enemys[i]!=null&&enemys[i].transform == other.gameObject.transform)    //리스트에 있는 객체인 경우
                {
                    if (!stiffmoment)
                    {
                        //enemys[i].gameObject.GetComponent<enemy>().ChangingBarColor(Color.red);     //피통을 빨간색으로 변경
                        if (other.gameObject.GetComponent<enemy>() != null)
                        {
                            enemys[i].gameObject.GetComponent<enemy>().stiff = false;                   //경직 풀기
                            enemys[i].gameObject.GetComponent<enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                        }
                        if (other.gameObject.GetComponent<AI_EnemyIndex>() != null)
                        {
                            enemys[i].gameObject.GetComponent<AI_EnemyIndex>().stiff = false;                   //경직 풀기
                            enemys[i].gameObject.GetComponent<AI_EnemyIndex>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                        }
                        if (other.gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                        {
                            enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().stiff = false;                   //경직 풀기
                            enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                        }
                        if (other.gameObject.GetComponent<AI_Enemy_buf>() != null)
                        {
                            enemys[i].gameObject.GetComponent<AI_Enemy_buf>().stiff = false;                   //경직 풀기
                            enemys[i].gameObject.GetComponent<AI_Enemy_buf>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                        }
                        if (other.gameObject.GetComponent<throw_Enemy>() != null)
                        {
                            enemys[i].gameObject.GetComponent<throw_Enemy>().stiff = false;                   //경직 풀기
                            enemys[i].gameObject.GetComponent<throw_Enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                        }
                        if (other.gameObject.GetComponent<Suicide_Enemy>() != null)
                        {
                            enemys[i].gameObject.GetComponent<Suicide_Enemy>().stiff = false;                   //경직 풀기
                            enemys[i].gameObject.GetComponent<Suicide_Enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                        }
                        Debug.Log("Remove list(" + other.gameObject + ")");

                        enemys.RemoveAt(i);                                                         //리스트에서 삭제
                    }
                }
                    
            }
        }
    }

    public void ChangingBarColor(Color _color)
    {
        barcolor.color = _color;
    }

    private void DistroyPlane()
    {
        for (int i = 0; i < enemys.Count; i++)
        {
            if (enemys.Count == 0)
            {
                Debug.Log("destroy5");
                Destroy(gameObject);
            }

            if (enemys[i].gameObject == null)
            {
                enemys.RemoveAt(i);
            }
            else
            {
                //Debug.Log("return setting" + enemys[i].gameObject.name);
                //enemys[i].gameObject.GetComponent<enemy>().ChangingBarColor(Color.red);     //피통을 빨간색으로 변경

                if (enemys[i].gameObject.GetComponent<enemy>() != null)
                {
                    enemys[i].gameObject.GetComponent<enemy>().stiff = false;                   //경직 풀기
                    enemys[i].gameObject.GetComponent<enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                }
                if (enemys[i].gameObject.GetComponent<AI_EnemyIndex>() != null)
                {
                    enemys[i].gameObject.GetComponent<AI_EnemyIndex>().stiff = false;                   //경직 풀기
                    enemys[i].gameObject.GetComponent<AI_EnemyIndex>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                }
                if (enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>() != null)
                {
                    enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().stiff = false;                   //경직 풀기
                    enemys[i].gameObject.GetComponent<AI_Tower_destroyer_Enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                }
                if (enemys[i].gameObject.GetComponent<AI_Enemy_buf>() != null)
                {
                    enemys[i].gameObject.GetComponent<AI_Enemy_buf>().stiff = false;                   //경직 풀기
                    enemys[i].gameObject.GetComponent<AI_Enemy_buf>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                }
                if (enemys[i].gameObject.GetComponent<throw_Enemy>() != null)
                {
                    enemys[i].gameObject.GetComponent<throw_Enemy>().stiff = false;                   //경직 풀기
                    enemys[i].gameObject.GetComponent<throw_Enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                }
                if (enemys[i].gameObject.GetComponent<Suicide_Enemy>() != null)
                {
                    enemys[i].gameObject.GetComponent<Suicide_Enemy>().stiff = false;                   //경직 풀기
                    enemys[i].gameObject.GetComponent<Suicide_Enemy>().SkillSetting(null);              //데미지 받고 있는 스킬 전달
                }
            }
        }
        enemys.Clear();

        if (gameObject != null)
        {
            Debug.Log("destroy6");
            Destroy(gameObject);
        }

    }


    //옵션 세팅
    public void SettingOption(Skill _skill)
    {
        ps = _skill;
        psv = _skill.value;
        dmgrange = _skill.range;
        gameObject.transform.localScale = new Vector3(dmgrange, dmgrange, dmgrange);    //데미지 범위 설정
    }

    IEnumerator StiffMoment()
    {
        yield return new WaitForSeconds(1);
        switch(psv)
        {
            case SkillValue.Ice:
                stiffmoment = true;                  //처음 좌표에 있던 적들만 경직모드
                break;

            case SkillValue.Lightning:
                break;
        }
            
       

    }
}
