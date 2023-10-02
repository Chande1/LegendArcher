using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//스킬 종류
public enum SkillValue
{
    Non,            //무속성
    Fire,           //불속성
    Ice,            //얼음속성
    Lightning       //전기속성
}


public class Skill
{
    public SkillValue value;       //스킬 종류
    public bool useskill;          //스킬 활성 여부

    public float cooltime;         //쿨타임
    public float damage;           //데미지
    public int damagecount;         //데미지 횟수

    public float skilltime;         //스킬 지속 시간
    public bool stiff;              //경직 여부
    public bool userange;          //범위 스킬 여부
    public float range;            //스킬 범위

    public float startRt;          //스킬 선택 각도 시작 범위
    public float endRt;            //스킬 선택 각도 끝 범위

    public Image simage;           //스킬 이미지

    
    Skill()
    {
        value = SkillValue.Non;
        useskill = true;
        damagecount = 0;
        skilltime = 0f;
        stiff = false;
        userange = false;
        range = 0f;
        startRt = 0f;
        endRt = 0f;
        cooltime = 0f;
        damage = 0f;
    }

    public Skill(SkillValue _value, float _cooltime, float _damage,int _dcount, float _skilltime,bool _stiff,bool _userange,float _range,float _startRt,float _endRt,Image _simage)
    {
        value = _value;
        cooltime = _cooltime;
        damage = _damage;
        damagecount = _dcount;
        skilltime = _skilltime;
        stiff = _stiff;
        userange = _userange;
        range = _range;
        startRt = _startRt;
        endRt = _endRt;
        simage = _simage;
        useskill = true;
    }
}
