using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeReference] GameObject TPointer;

    [SerializeReference] GameObject scoreboard;
    [SerializeReference] Text scoreboard_result;
    [SerializeReference] GameObject scoreboard_check;
    [SerializeReference] GameObject scoreboard_retry;
    [SerializeReference] GameObject uimanager;
    [SerializeReference] GameObject result_star1;
    [SerializeReference] GameObject result_star2;
    [SerializeReference] GameObject result_star3;

    public AudioClip effect_confirm;
    public AudioClip effect_cancle;
    public AudioClip victroy_bgm;
    public AudioClip defeat_bgm;
    [SerializeField] GameObject gui;

    bool gameover = false;

    static int scoreboard_stars = 0; //점수처리

    public static int scoreboard_status = 0; //1 = 승리처리 2 = 패배처리

    void Awake()
    {
        TPointer = GameObject.Find("TutorialPointer");
        scoreboard = GameObject.Find("Scoreboard");
        uimanager = GameObject.Find("UI_Manager");
        scoreboard_check = GameObject.Find("result_check");
        scoreboard_retry = GameObject.Find("result_retry");
        result_star1 = GameObject.Find("star1"); result_star2 = GameObject.Find("star2"); result_star3 = GameObject.Find("star3");
        scoreboard_result = GameObject.Find("result_txt").GetComponent<Text>();

        gui = GameObject.Find("GUI");
        effect_confirm = Resources.Load<AudioClip>("Confirm_yes");
        effect_cancle = Resources.Load<AudioClip>("Cancle_no");
        victroy_bgm = Resources.Load<AudioClip>("victory_BGM");
        defeat_bgm = Resources.Load<AudioClip>("defeat_BGM");

        /*
        scoreboard.transform.position = new Vector3(GameObject.Find("Player").transform.position.x,
    GameObject.Find("Player").transform.position.y,
    GameObject.Find("Player").transform.position.z + 1);
        */

        Button scoreboard_check_b = scoreboard_check.GetComponent<Button>();
        scoreboard_check_b.onClick.AddListener(loadmenu);

        Button scoreboard_retry_b = scoreboard_retry.GetComponent<Button>();
        scoreboard_retry_b.onClick.AddListener(retry);


    }

    void Start()
    {
        if (GameObject.Find("TutorialPointer"))
            TPointer = GameObject.Find("TutorialPointer");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
            TPointer.SetActive(false);
        scoreboard.SetActive(false);
    }


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("TutorialPointer"))
            TPointer = GameObject.Find("TutorialPointer");

        if (GameObject.Find("Scoreboard"))
            scoreboard = GameObject.Find("Scoreboard");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01")
            && GameObject.Find("maintower") == null
            && uimanager.GetComponent<WaveManager>().KillScore < uimanager.GetComponent<WaveManager>().WaveMonsterCount
            && gameover == false)
        {
            scoreboard.GetComponent<AudioSource>().clip = defeat_bgm;
            scoreboard.GetComponent<AudioSource>().volume = 0.5f;
            scoreboard.GetComponent<AudioSource>().Play();


            scoreboard_status = 2; //패배처리

            //GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().stiff = true;
            //GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().enabled = false;

            if (GameObject.Find("Towers"))
                GameObject.Find("Towers").SetActive(false);

            if (scoreboard_status == 2)
            {
                if (GameObject.Find("노말 모드"))
                {
                    for (int i = 0; i < GameObject.Find("노말 모드").transform.childCount; i++)
                    {
                        for(int j=0;j<8;j++)
                        {
                            GameObject.Find("노말 모드").transform.GetChild(i).gameObject.GetComponent<MonoBehaviour>().enabled = false;
                        }
                        GameObject.Find("노말 모드").transform.GetChild(i).gameObject.GetComponent<WaveController>().enabled = false;
                        GameObject.Find("노말 모드").transform.GetChild(i).gameObject.GetComponent<MonsterSpawnerIndex>().enabled = false;
                        GameObject.Find("노말 모드").transform.GetChild(i).gameObject.SetActive(false);
                    }
                    GameObject.Find("노말 모드").SetActive(false);

                }

                if (GameObject.Find("하드 모드"))
                {
                    for (int i = 0; i < GameObject.Find("하드 모드").transform.childCount; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            GameObject.Find("하드 모드").transform.GetChild(i).gameObject.GetComponent<MonoBehaviour>().enabled = false;
                        }
                        GameObject.Find("하드 모드").transform.GetChild(i).gameObject.GetComponent<WaveController>().enabled = false;
                        GameObject.Find("하드 모드").transform.GetChild(i).gameObject.GetComponent<MonsterSpawnerIndex>().enabled = false;
                        GameObject.Find("하드 모드").transform.GetChild(i).gameObject.SetActive(false);
                    }
                    GameObject.Find("하드 모드").SetActive(false);
                }


                TPointer.SetActive(true);
                scoreboard.SetActive(true);
                GameObject.Find("victory_txt").SetActive(false);
                GameObject.Find("stars").SetActive(false);
                GameObject.Find("angelring_background").SetActive(false);
                scoreboard_result.text = string.Format("타워가 파괴되어 패배하였습니다.");

            }
            gameover = true;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01")
            && GameObject.Find("maintower") != null
            && uimanager.GetComponent<WaveManager>().KillScore >= uimanager.GetComponent<WaveManager>().WaveMonsterCount
            && GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().EnemyHP >= 0
            && gameover == false)
        {
            scoreboard.GetComponent<AudioSource>().clip = victroy_bgm;
            scoreboard.GetComponent<AudioSource>().volume = 0.5f;
            scoreboard.GetComponent<AudioSource>().Play();

            scoreboard_status = 1; //승리처리

            if (GameObject.Find("Towers"))
                GameObject.Find("Towers").SetActive(false);
            //GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().stiff = true;
            // GameObject.Find("Forest Golem").GetComponent<throw_Enemy>().enabled = false;
            if (scoreboard_status == 1)
            {
                if (GameObject.Find("노말 모드"))
                {
                    for (int i = 0; i < GameObject.Find("노말 모드").transform.childCount; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            GameObject.Find("노말 모드").transform.GetChild(i).gameObject.GetComponent<MonoBehaviour>().enabled = false;
                        }

                        GameObject.Find("노말 모드").transform.GetChild(i).gameObject.GetComponent<WaveController>().enabled = false;
                        GameObject.Find("노말 모드").transform.GetChild(i).gameObject.GetComponent<MonsterSpawnerIndex>().enabled = false;
                        GameObject.Find("노말 모드").transform.GetChild(i).gameObject.SetActive(false);
                    }
                    GameObject.Find("노말 모드").SetActive(false);

                }

                if (GameObject.Find("하드 모드"))
                {
                    for (int i = 0; i < GameObject.Find("하드 모드").transform.childCount; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            GameObject.Find("하드 모드").transform.GetChild(i).gameObject.GetComponent<MonoBehaviour>().enabled = false;
                        }

                        GameObject.Find("하드 모드").transform.GetChild(i).gameObject.GetComponent<WaveController>().enabled = false;
                        GameObject.Find("하드 모드").transform.GetChild(i).gameObject.GetComponent<MonsterSpawnerIndex>().enabled = false;
                        GameObject.Find("하드 모드").transform.GetChild(i).gameObject.SetActive(false);
                    }
                    GameObject.Find("하드 모드").SetActive(false);
                }

                scoreboard.SetActive(true);
                TPointer.SetActive(true);
                if (GameObject.Find("maintower").GetComponent<HP_M>().Hp >= 200 && GameObject.Find("maintower").GetComponent<HP_M>().Hp <= 300)
                {
                    scoreboard_stars = 3;
                    result_star1.SetActive(true); result_star2.SetActive(true); result_star3.SetActive(true);
                }

                if (GameObject.Find("maintower").GetComponent<HP_M>().Hp >= 100 && GameObject.Find("maintower").GetComponent<HP_M>().Hp < 200)
                {
                    scoreboard_stars = 2;
                    result_star1.SetActive(true); result_star2.SetActive(true); result_star3.SetActive(false);
                }

                if (GameObject.Find("maintower").GetComponent<HP_M>().Hp >= 1 && GameObject.Find("maintower").GetComponent<HP_M>().Hp < 99)
                {
                    scoreboard_stars = 1;
                    result_star1.SetActive(true); result_star2.SetActive(false); result_star3.SetActive(false);
                }
            }
            scoreboard_result.text = string.Format("승리 하여 {0}별로 클리어 하셨습니다!", scoreboard_stars);

            gameover = true;
        }


    }

    public void loadmenu()
    {
        gui.GetComponent<AudioSource>().clip = effect_cancle;
        gui.GetComponent<AudioSource>().Play();
        scoreboard.SetActive(false);
        scoreboard_status = 0;
        SceneManager.LoadScene("menu01");
    }

    public void retry()
    {
        gui.GetComponent<AudioSource>().clip = effect_cancle;
        gui.GetComponent<AudioSource>().Play();
        scoreboard.SetActive(false);
        scoreboard_status = 0;
        uimanager.GetComponent<WaveManager>().KillScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
