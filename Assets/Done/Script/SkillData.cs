using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillData : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Bow;
    [Tooltip("쿨타임 숨김모드")]
    [SerializeField] bool HiddenMode;           //스킬 쿨타임 이미지 숨김 모드
    cBow bow;

    public Image[] simage;
    public List<Skill> SkillList = new List<Skill>();
    bool skillchange = false;
    float tempz;                                      //트리거를 눌렀을때 받는 기준이 되는 스킬 z좌표

    void Start()
    {
        Player = GameObject.Find("Player");
        Bow = GameObject.Find("Bow");
        bow = Bow.GetComponent<cBow>();

        List<Dictionary<string, object>> data = CSVReader.Read("SkillBaseData");    //csv읽어서 데이터 가져오기

        for (int i = 0; i < data.Count; i++)
        {
            SkillValue dskill_type = (SkillValue)Enum.Parse(typeof(SkillValue), data[i]["Skill_Type"].ToString());
            float dcool_time = float.Parse(data[i]["Cool_Time"].ToString());
            float ddamage = float.Parse(data[i]["Damage(1tick)"].ToString());
            int dtick_count = int.Parse(data[i]["Tick_Count"].ToString());
            int dskill_time = int.Parse(data[i]["Skill_Time"].ToString());
            bool dstiff = bool.Parse(data[i]["Stiff"].ToString());
            bool drange = bool.Parse(data[i]["Range"].ToString());
            int ddistance = int.Parse(data[i]["Distance"].ToString());
            int dskill_start = int.Parse(data[i]["Skill_Start"].ToString());
            int dskill_end = int.Parse(data[i]["Skill_End"].ToString());
            string cool_image = data[i]["Cool_Image"].ToString();
            Image dcool_image;
            if (cool_image == "null")
            {
                dcool_image = null;
            }
            else
            {
                dcool_image = simage[int.Parse(cool_image)];
            }
            SkillList.Add(new Skill(dskill_type, dcool_time, ddamage, dtick_count, dskill_time, dstiff, drange, ddistance, dskill_start, dskill_end, dcool_image));
            Debug.Log(SkillList);
        }
    }

    void Update()
    {
        //활대 움직임 따라가기

        if(HiddenMode)
        {
            for (int i = 0; i < simage.Length; i++)
            {
                simage[i].color = new Color(simage[i].color.r, simage[i].color.g, simage[i].color.b, 90 / 255f);    //이미지 투명도 90
            }
        }

        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))  //왼손 트리거를 누르고 있다면
        {
            //Debug.Log("trigger");
            gameObject.transform.position = Bow.transform.position;
            if (!skillchange)//트리거를 누른 순간만 임시값을 저장하기 위한 장치
            {
                tempz = Bow.transform.eulerAngles.z;                         //스킬 변화의 기준 z회전값
            }

            skillchange = true;                                              //스킬 변화

        }
        else
        {
            if (HiddenMode)
            {
                for (int i = 0; i < simage.Length; i++)
                {
                    simage[i].color = new Color(simage[i].color.r, simage[i].color.g, simage[i].color.b, 0);    //이미지 투명도 0
                }
            }
               
            skillchange = false;                                        //스킬을 바꾸지 않음(무조건 일반샷)
            gameObject.transform.position = Bow.transform.position;
            gameObject.transform.rotation = Bow.transform.rotation;     //회전 포함
        }

        bow = Bow.GetComponent<cBow>();
        bow.skillvalue = DistinctionSkill();   //활의 각도에 따라 활의 스킬 상태 실시간 변경
        bow.ShowBowState(bow.skillvalue + "\ntempz:" + tempz + "\nbowz:" + Bow.transform.eulerAngles.z +
            "\nbowrotationz:" + Bow.transform.rotation.z);
        RecoverCoolTime();
    }

    //cBow에서 사용됨!
    public void SkillShoot(BowState _bstate, SkillValue _skillValue)
    {
        if (_bstate == BowState.Shoot)
        {
            for (int i = 1; i < SkillList.Count; i++)   //기본 공격인 0은 제외
            {
                if (_skillValue == SkillList[i].value)
                {
                    if (SkillList[i].simage.fillAmount == 1)   //스킬 쿨타임이 꽉 차 있을때
                    {
                        SkillList[i].simage.fillAmount = 0; //스킬 쿨타임이 0이 됨->스킬이 사용됨
                    }
                }
            }
        }
    }

    //사용한 스킬 쿨타임 채우기
    void RecoverCoolTime()
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].simage != null)                   //이미지가 있을때
            {
                if (SkillList[i].simage.fillAmount < 1)    //쿨타임이 꽉 차지 않은 경우
                {
                    SkillList[i].simage.fillAmount += Time.deltaTime * (1 / SkillList[i].cooltime);
                }
            }

        }
    }
    //활의 각도에 따라 스킬 판별
    SkillValue DistinctionSkill()
    {
        if (skillchange)
        {
            for (int i = 0; i < SkillList.Count; i++)
            {
                float rt;
                if (tempz < 50)
                {
                    rt = 360 - (360 + tempz - Bow.transform.eulerAngles.z);
                    if (Bow.transform.eulerAngles.z < 50)
                    {
                        rt = 360 - (tempz - Bow.transform.eulerAngles.z);
                    }

                }
                else
                {
                    rt = 360 - (tempz - Bow.transform.eulerAngles.z);
                }
                if (rt <= SkillList[i].startRt && rt >= SkillList[i].endRt)
                {
                    return SkillList[i].value;
                }
            }
        }
        else
        {
            return SkillValue.Non;  //아닐때는 일반 공격
        }


        return SkillValue.Non;  //아닐때는 일반 공격
    }



}

