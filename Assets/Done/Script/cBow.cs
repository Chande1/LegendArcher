using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BowState
{
    Non,        //아무것도 아닌 상태
    Arrowning,  //장착중
    Drawing,    //당기는중
    Shoot,      //발사
}


public class cBow : MonoBehaviour
{
    public BowState bstate;
    public SkillValue skillvalue;

    [SerializeField] SkillData skilldata;
    [SerializeField] GameObject skillwindow;
    [SerializeField] Text nowskill;


    cBow()
    {
        bstate = BowState.Non;
    }

    public void SetBowAni()
    {
        switch (bstate)
        {
            case BowState.Non:
                break;
            case BowState.Drawing:

                break;
            case BowState.Shoot:
                skilldata.SkillShoot(bstate, skillvalue);   //스킬 사용
                bstate = BowState.Non;
                break;
        }
    }



    void Start()
    {
        Instantiate(skillwindow, transform.position,transform.rotation);                  //스킬 윈도우 생성
    }

    void Update()
    {
        skilldata = GameObject.Find("SkillWindow(30do)(Clone)").GetComponent<SkillData>();
        //Debug.Log("Lpos:" + destination.transform.localPosition);
        //Debug.Log("Wpos:" + destination.transform.position);
        //nowskill.text = skillvalue.ToString() + "\n" + transform.position + "\n" + transform.eulerAngles;
    }

    public void ShowBowState(string _state)
    {
        nowskill.text = _state;
    }
}