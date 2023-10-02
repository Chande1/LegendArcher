using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class difficulty_current : MonoBehaviour
{
    GameObject NormalMode;
    GameObject HardMode;
    public Text difficultytext;


    void Awake()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        {
            NormalMode = GameObject.Find("Spawners_N");
            HardMode = GameObject.Find("Spawners_H");
            NormalMode.name = "노말 모드";
            HardMode.name = "하드 모드";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       //  HardMode.SetActive(false); //임시.
    }

    // Update is called once per frame
    void Update()
    {
        difficulty_now();
    }

    public void difficulty_now()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        {
            string difficulty_normal_name = NormalMode.name;
            string difficulty_hard_name = HardMode.name;

            if (NormalMode.activeSelf == true && HardMode.activeSelf == false)
                difficultytext.text = string.Format("현재 : {0}", difficulty_normal_name);

            else if (NormalMode.activeSelf == false && HardMode.activeSelf == true)
                difficultytext.text = string.Format("현재 : {0}", difficulty_hard_name);

            else if (NormalMode.activeSelf == true && HardMode.activeSelf == true)
                difficultytext.text = string.Format("오류 : 난이도 설정");

            else if (NormalMode.activeSelf == false && HardMode.activeSelf == false)
                difficultytext.text = string.Format("오류 : 난이도 설정");
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01"))
        {
            difficultytext.text = string.Format("현재 : 메뉴");
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1"))
        {
            difficultytext.text = string.Format("현재 : 튜토리얼");
        }

    }
}
