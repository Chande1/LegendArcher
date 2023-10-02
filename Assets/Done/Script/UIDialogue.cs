using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Dialogue
{
    public string SceneName;   //진행중인 씬 이름
    public int Dnum;           //진행중인 대사 순서
    public string Sdialogue;   //진행중인 대사

    Dialogue()
    {
        SceneName = "";
        Dnum = 0;
        Sdialogue = "";
    }

    public Dialogue(string _scene, int _dnum, string _dlg)
    {
        SceneName = _scene;
        Dnum = _dnum;
        Sdialogue = _dlg;
    }
}

public class UIDialogue : MonoBehaviour
{
    [Header("UI 대사 설정")]
    [Tooltip("(확인용)UI 다음 진행 요소")]
    [SerializeField] GameObject UIW;                                     //다음으로 넘어가기 위한 클릭 요소
    [Tooltip("(확인용)UI 대사 텍스트")]
    [SerializeField] Text UID;                                          //ui 대사텍스트
    [Tooltip("(확인용)진행중인 씬")]
    [SerializeField] string ingsname;                                   //씬 이름
    [Tooltip("(확인용)진행중인 대사 번호")]
    [SerializeField] int ingnum;                                        //ui 대사 번호

    public List<Dialogue> DialogueList = new List<Dialogue>();          //대사 리스트

    void Start()
    {
        //초기화 작업
        UIW = GameObject.Find("UI_Window");
        UID = GameObject.Find("UI_Text").GetComponent<Text>();
        ingsname = SceneManager.GetActiveScene().name;
        ingnum = 0;

        List<Dictionary<string, object>> data = CSVReader.Read("UIDialogueData");    //csv읽어서 데이터 가져오기

        for (int i = 0; i < data.Count; i++)
        {
            string sname = data[i]["SceneName"].ToString();                         //씬 이름 읽기
            int dnum = int.Parse(data[i]["DialogueNum"].ToString());                //대사 번호 읽기
            string dlg = data[i]["Dialogue"].ToString();                            //대사 읽기

            DialogueList.Add(new Dialogue(sname, dnum, dlg));                         //대사 추가
            //Debug.Log(DialogueList);                                                //리스트 로그로 출력
        }
        //ShowDialogue();
    }

    private void Update()
    {
        ShowDialogue();
    }

    /*현재 씬과 진행중인 번호에 따라 대사를 출력해주는 함수*/
    public void ShowDialogue()
    {
        for (int i = 0; i < DialogueList.Count; i++)
        {
            if (DialogueList[i].SceneName == SceneManager.GetActiveScene().name)      //씬 이름이 현재 씬과 일치하고
            {
                if (DialogueList[i].Dnum == ingnum)                                    //대사 번호가 현재 진행중인 대사 번호와 일치할때
                {
                    //Debug.Log("[" + DialogueList[i].Dnum + "]:" + DialogueList[i].Sdialogue);
                    if (DialogueList[i].Sdialogue.Contains("/"))                    //해당 대사에 띄어쓰기가 있는 경우
                    {
                        string txt = DialogueList[i].Sdialogue;                     //전체 대사
                        char sp = '/';                                              //구분할 대사
                        string[] spstring = txt.Split(sp);

                        for (int j = 0; j < spstring.Length; j++)                        //띄어쓰기로 구분되어 저장된 문자열 출력 
                        {
                            if (j == 0)                                             //대사의 첫번째 문장인 경우 
                                UID.text = spstring[j];                             //그 전 대사를 초기화
                            else                                                    //대사의 첫번째 문장이 끝난 경우
                                UID.text += "\n" + spstring[j];                       //다음 대사를 띄어쓰기 후 출력
                        }
                    }
                    else                                                            //띄어쓰기가 없는 대사일 경우
                    {
                        UID.text = DialogueList[i].Sdialogue;                       //해당 대사 출력      
                    }
                }
            }
        }                                                              //다음 대사 번호 
        //Debug.Log("plusnum:" + ingnum);
    }

    //현재 대사 번호 반환하는 함수
    public int GetIngNum()
    {
        return ingnum;
    }

    public void NextIngNum()
    {
        ingnum++;    //다음 대사로
    }
}
