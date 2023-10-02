using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : MonoBehaviour
{
    [Header("이펙트 설정")]
    [SerializeField] GameObject[] effect_skill;

    [Tooltip("콜라이더 둘레")]
    public float colliderRadius = 1f;
    [Range(0f, 1f)]
    public float collideOffset = 0.15f;

    [Header("확인용 설정")]
    ArrowState arrowstate;  //화살 상태
    int arrowskill;      //화살 스킬
    bool flag = false;


    private void Update()
    {
        arrowstate = gameObject.GetComponentInParent<ArrowController>().astate;
        arrowskill = (int)gameObject.GetComponentInParent<ArrowController>().GetSkillValue();

        switch (arrowstate)
        {
            case ArrowState.OverBow:
                switch (arrowskill)
                {
                    case 1:
                        effect_skill[0].SetActive(true);
                        effect_skill[1].SetActive(false);
                        effect_skill[2].SetActive(false);
                        break;
                    case 2:
                        effect_skill[0].SetActive(false);
                        effect_skill[1].SetActive(true);
                        effect_skill[2].SetActive(false);
                        break;
                    case 3:
                        effect_skill[0].SetActive(false);
                        effect_skill[1].SetActive(false);
                        effect_skill[2].SetActive(true);
                        break;
                }
                break;
            case ArrowState.Piew:
                if(!flag)
                {
                    effect_skill[arrowskill - 1].transform.GetChild(0).gameObject.SetActive(false);
                    effect_skill[arrowskill - 1].transform.GetChild(1).gameObject.SetActive(true);
                    effect_skill[arrowskill - 1].transform.GetChild(2).gameObject.SetActive(false);
                    Debug.Log(effect_skill[arrowskill - 1].transform.GetChild(1).gameObject.name);
                }
                
                break;
        }

    }

    private void OnTriggerEnter(Collider other) //트리거 영역에 들어갔을 때
    {
        if (other.CompareTag("enemy") || other.CompareTag("wall"))
        {
            flag = true;
            effect_skill[arrowskill - 1].transform.GetChild(0).gameObject.SetActive(true);
            effect_skill[arrowskill - 1].transform.GetChild(1).gameObject.SetActive(false);
            effect_skill[arrowskill - 1].transform.GetChild(2).gameObject.SetActive(false);
            Debug.Log(effect_skill[arrowskill - 1].transform.GetChild(0).gameObject.name);
            //effect_skill[arrowskill - 1].transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop();
            //effect_skill[arrowskill - 1].transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
            //effect_skill[arrowskill - 1].transform.GetChild(2).gameObject.
        }
    }
}

