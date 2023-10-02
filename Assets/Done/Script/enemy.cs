using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    Vector3 pos; //시작위치
    
    public bool attack = true; //공격받는중
    public bool stiff = false; //경직


    [Header("받고 있는 스킬의 종류")]
    public Skill s;
    public SkillValue sv;
    public SkillValue lastskillvalue;

    [Header("적 HP바 설정")]
    [SerializeField] float maxhp = 50f;
    [SerializeField] float hp=50f;
    [SerializeField] Slider hpbar;
    [SerializeField] Image barcolor;
    [SerializeField] Text hpnum;

    [Space(10f)]
    [Header("적 움직임 설정")]           
    [Header("수평")]
    [SerializeField] public bool horizontal=false;   //수평
    [SerializeField] float hspeed = 1f;      //수평 움직임 속도
    [SerializeField] float hdistance = 1f;  //수평 움직임 거리
    [Space(10f)]
    [Header("수직")]
    [SerializeField] public bool verticality =false;   //수직
    [SerializeField] float vspeed = 1f;      //수평 움직임 속도
    [SerializeField] float vdistance = 1f;  //수평 움직임 거리
    [Header("전진모드")]
    [SerializeField] public bool frontmode = false;   //전진
    [SerializeField] float fmspeed = 1f;      //전진 모드 움직임 속도

    private IEnumerator coroutine;
    bool flag = false;
    float tempspeed = 1f;

    void Start()
    {
        //gameObject.transform.Rotate(90, 0, 0);
        hp = maxhp;                 //풀피로 시작
        pos = transform.position;   //시작 위치 저장
        attack = true;
        barcolor.color = Color.red;  //빨간색
        stiff = false;
        s = null;
        sv = SkillValue.Non;

        if (horizontal)
            tempspeed = hspeed;
        else if (verticality)
            tempspeed = vspeed;
        else if (frontmode)
            tempspeed = fmspeed;
    }

    void Update()
    {
        if (!stiff)          //경직이 아니라면
            Enemymoving();  //적의 움직임 설정값 적용
        else
            hspeed = 0f;

        hpbar.value = hp / maxhp ;
        hpnum.text= string.Format(hp+"/"+maxhp);

        if(hp<=0)   //만약 hp가 없으면
        {
            //Debug.Log("enemy:" + gameObject.name + " Destroy!");
            Invoke("Destroyenemy", 0.5f); //파괴
        }
    }


    public void SetMaxHP(float _maxhp)
    {
        maxhp = _maxhp;
    }

    public float GetHP()
    {
        return hp;
    }

    void Enemymoving()
    {
        Vector3 vec = pos;

        if (horizontal) //수평 모드
        {
            hspeed = tempspeed;
            vec.x += hdistance * Mathf.Sin(Time.time * hspeed);
            transform.position = vec;
        }
        if (verticality)    //수직 모드
        {
            vspeed = tempspeed;
            vec.z += vdistance * Mathf.Sin(Time.time * vspeed);
            transform.position = vec;
        }
        if(frontmode)   //전진 모드
        {
            fmspeed = tempspeed;
            transform.Translate(0, -fmspeed,0 );    //앞으로 이동
        }
    }

    public void Damaging(float _damage)
    {
        hp -= _damage;
    }

    public void ChangingBarColor(Color _color)
    {
        barcolor.color = _color;
    }

    public void SkillSetting(Skill _skill)
    {
        s = _skill;
        if (_skill == null||_skill.value==SkillValue.Non)
        {
            sv = SkillValue.Non;
        }
        else
        {
            sv = _skill.value;
            lastskillvalue = _skill.value;
        }
    }

    private void Destroyenemy()
    {
        //초기 정보로 초기화
        
        Destroy(gameObject);
    }
    //풀링용
    /*
    private void Destroyenemy()
    {
        //초기 정보로 초기화
        hp = maxhp;
        attack = true;
        barcolor.color = Color.red;
        stiff = false;
        s = null;
        sv = SkillValue.Non;
        transform.position = pos;
        MonsterSpawnerIndex.ReturnObject(this);
    }
    */
}
