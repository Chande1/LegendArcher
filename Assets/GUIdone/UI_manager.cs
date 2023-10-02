using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_manager : MonoBehaviour
{

    [SerializeField] public GameObject VRpointer;
    [SerializeField] GameObject Tpointer;  //튜토리얼 포인터
    [SerializeField] GameObject PlayerInfo;

    //-----------------------------------메뉴
    [SerializeField] GameObject menu;
    [SerializeField] GameObject menub2;
    [SerializeField] GameObject menub3;
    [SerializeField] GameObject menub4;
    [SerializeField] GameObject menub5;
    [SerializeField] GameObject menucc;
    [SerializeField] GameObject gui;
    [SerializeField] GameObject retry_menu;

    [SerializeField] GameObject exitwindow;
    [SerializeField] GameObject exitcf;
    [SerializeField] GameObject exitcc;
    //-----------------------------------옵션설정
    [SerializeField] GameObject option;
    [SerializeField] Image sound_bar_fill;
    [SerializeField] Text sound_ratio;
    [SerializeField] GameObject soundu;
    [SerializeField] GameObject soundd;
    [SerializeField] GameObject optioncc;
    //---------------------------------그래픽옵션
    [SerializeField] GameObject blur_off;
    [SerializeField] GameObject blur_on;
    [SerializeField] GameObject dof_off;
    [SerializeField] GameObject dof_on;
    [SerializeField] GameObject bloom_off;
    [SerializeField] GameObject bloom_on;
    [SerializeField] GameObject ab_off;
    [SerializeField] GameObject ab_on;

    static public bool isblur = false;
    static public bool isdof = false;
    static public bool isbloom = false;
    static public bool isab = false;
    //---------------------------------난이도설정
    [SerializeField] GameObject difficulty_menu;
    [SerializeField] GameObject difficulty_stage1hard;
    [SerializeField] GameObject difficulty_stage1normal;
    [SerializeField] GameObject difficulty_stage1normal_button;
    [SerializeField] GameObject difficulty_stage1hard_button;
    [SerializeField] GameObject difficulty_confirm_cb; //difficulty_confirm_cancle_button
    [SerializeField] GameObject difficulty_confirm_cbN; //difficulty_confirm_cancle_button
    [SerializeField] GameObject difficulty_confirm_cbH; //difficulty_confirm_cancle_button
    [SerializeField] GameObject difficulty_normal_confirm;
    [SerializeField] GameObject difficulty_normal_confirm_button;
    [SerializeField] GameObject difficulty_hard_confirm;
    //----------------------------------재시작
    [SerializeField] GameObject retry_confirm_cb; //retry_confirm_cancle_button
    [SerializeField] GameObject retry_confirm_b; //retry_confirm_button



    public static int stage1difficulty = 0; //1노말 2하드


    public static PostProcessVolume postprocess;
    public static PostProcessLayer postprocess_AA;

    public Material dayskybox;
    public Material nightskybox;
    public AudioClip bgm_normal;
    public AudioClip bgm_hard;
    public AudioClip bgm_mainmenu;
    public AudioClip effect_confirm;
    public AudioClip effect_cancle;



    private void Awake()
    {
        //postprocess_AA.antialiasingMode = UI_manager.postprocess_AA.antialiasingMode;
        stage1difficulty = UI_manager.stage1difficulty;
        isblur = UI_manager.isblur;
        isdof = UI_manager.isdof;
        isab = UI_manager.isab;
        isbloom = UI_manager.isbloom;


        //ScoreBoard= GameObject.Find("Scoreboard");
        PlayerInfo = GameObject.Find("PlayerInfo");
        gui = GameObject.Find("GUI");
        postprocess = GameObject.Find("UI_Manager").GetComponent<PostProcessVolume>();
        postprocess_AA = GameObject.Find("CenterEyeAnchor").GetComponent<PostProcessLayer>();

        bgm_normal = Resources.Load<AudioClip>("normal_BGM");
        bgm_hard = Resources.Load<AudioClip>("hard_BGM");
        bgm_mainmenu = Resources.Load<AudioClip>("maintitle_BGM");
        effect_confirm = Resources.Load<AudioClip>("Confirm_yes");
        effect_cancle = Resources.Load<AudioClip>("Cancel_no");

        retry_menu = GameObject.Find("retry_confirm");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01"))
        {
            //GameObject.Find("PlayerInfo").SetActive(false);
            PlayerInfo.SetActive(false);
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().clip = bgm_mainmenu;
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().Play();
        }

        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("map01"))
            stage1difficulty = 0;
        //------------------------------------------------------------난이도별 BGM 설정
        if (stage1difficulty == 1)
        {
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().clip = bgm_normal;
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().Play();
        }
        if (stage1difficulty == 2)
        {
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().clip = bgm_hard;
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().Play();
        }
        if (stage1difficulty == 0 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
            GameObject.Find("CenterEyeAnchor").GetComponent<AudioSource>().clip = null;
        //------------------------------------------------------------------------------



        if (difficulty_stage1hard && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01")) //0216
        {
            WaveController[] wavecontrollers_hard = difficulty_stage1hard.GetComponentsInChildren<WaveController>();
            MonsterSpawnerIndex[] monsterspawners_hard = difficulty_stage1hard.GetComponentsInChildren<MonsterSpawnerIndex>();
            for (int i = 0; i < wavecontrollers_hard.Length; i++) //하드난이도의 웨이브컨트롤러 모두 끄기
                wavecontrollers_hard[i].enabled = false;
            for (int i = 0; i < monsterspawners_hard.Length; i++)
                monsterspawners_hard[i].enabled = false;
        }
        if (difficulty_stage1normal && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        {
            WaveController[] wavecontrollers_normal = difficulty_stage1normal.GetComponentsInChildren<WaveController>();
            MonsterSpawnerIndex[] monsterspawners_normal = difficulty_stage1normal.GetComponentsInChildren<MonsterSpawnerIndex>();
            for (int i = 0; i < wavecontrollers_normal.Length; i++)  //노말난이도의 웨이브컨트롤러 모두 끄기
                wavecontrollers_normal[i].enabled = false;
            for (int i = 0; i < monsterspawners_normal.Length; i++)
                monsterspawners_normal[i].enabled = false;
        }


        difficulty_menu = GameObject.Find("difficulty");
        difficulty_normal_confirm = GameObject.Find("difficulty_confirm_stage1normal");
        difficulty_hard_confirm = GameObject.Find("difficulty_confirm_stage1hard");

        //레이저
        VRpointer = GameObject.Find("LaserPointer");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1")
          || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01")
           ||(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01")))
        {
            Tpointer = GameObject.Find("TutorialPointer");
        }

        //--------------------------------------------------------------------메뉴
        menu = GameObject.Find("Menu");

        menub2 = GameObject.Find("menu_button2");
        Button menub2_b = menub2.GetComponent<Button>();
        menub2_b.onClick.AddListener(retry_confirm_on);

        retry_confirm_b = GameObject.Find("retry_confirm_button");
        Button retry_confirm_b_b = retry_confirm_b.GetComponent<Button>();
        retry_confirm_b_b.onClick.AddListener(retry);

        retry_confirm_cb = GameObject.Find("retry_cancle_button");
        Button retry_confirm_cb_b = retry_confirm_cb.GetComponent<Button>();
        retry_confirm_cb_b.onClick.AddListener(retry_cancle_on);

        menub3 = GameObject.Find("menu_button3");
        Button menub3_b = menub3.GetComponent<Button>();
        menub3_b.onClick.AddListener(difficulty_menu_on);

        menub4 = GameObject.Find("menu_button4");
        Button menub4_b = menub4.GetComponent<Button>();
        menub4_b.onClick.AddListener(option_window);

        menub5 = GameObject.Find("menu_button5");
        Button menub5_b = menub5.GetComponent<Button>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1"))
        {
            menub5_b.onClick.AddListener(exit_confirm);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01"))
        {
            menub5_b.onClick.AddListener(exit_confirm);
            GameObject.Find("exit_text").GetComponent<Text>().text = "게임을 완전히 \n 종료 하시겠습니까?";
        }

        menucc = GameObject.Find("menu_cancle_button");
        Button menucc_b = menucc.GetComponent<Button>();
        menucc_b.onClick.AddListener(menu_cancle_button);

        //------------------------------------------------------------------------------------나가기
        exitwindow = GameObject.Find("exit_confirm");

        exitcf = GameObject.Find("exit_confirm_button"); //현재 스테이지에서 나가서 메뉴창으로 가기
        Button exitcf_b = exitcf.GetComponent<Button>();
        exitcf_b.onClick.AddListener(exit_confirm_button);

        exitcc = GameObject.Find("exit_cancle_button"); // 나가기 확인창에서 나가기
        Button exitcc_b = exitcc.GetComponent<Button>();
        exitcc_b.onClick.AddListener(exit_cancle_button_);

        difficulty_confirm_cb = GameObject.Find("difficulty_cancle_button"); // 난이도 옵션창에서 나가기
        Button difficulty_cb = difficulty_confirm_cb.GetComponent<Button>();
        difficulty_cb.onClick.AddListener(difficulty_menu_cancle_on);

        difficulty_confirm_cbN = GameObject.Find("difficulty_confirm_cancle_buttonN"); // 노말 난이도 옵션창에서 나가기
        Button difficulty_cbN = difficulty_confirm_cbN.GetComponent<Button>();
        difficulty_cbN.onClick.AddListener(difficulty_normal_cancle_on);

        difficulty_confirm_cbH = GameObject.Find("difficulty_confirm_cancle_buttonH"); // 하드 난이도 옵션창에서 나가기
        Button difficulty_cbH = difficulty_confirm_cbH.GetComponent<Button>();
        difficulty_cbH.onClick.AddListener(difficulty_hard_cancle_on);


        //-------------------------------------------------옵션
        option = GameObject.Find("option");
        optioncc = GameObject.Find("option_cancle_button");
        Button optioncc_b = optioncc.GetComponent<Button>();
        optioncc_b.onClick.AddListener(option_cancle);


        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        {
            difficulty_stage1normal_button = GameObject.Find("difficulty_stage1N");//노말모드버튼
            Button difficulty_stage1normal_on_b = difficulty_stage1normal_button.GetComponent<Button>();
            difficulty_stage1normal_on_b.onClick.AddListener(difficulty_confirm_stage1normal_on);


            difficulty_stage1hard_button = GameObject.Find("difficulty_stage1H"); //하드모드버튼
            Button difficulty_stage1hard_on_b = difficulty_stage1hard_button.GetComponent<Button>();
            difficulty_stage1hard_on_b.onClick.AddListener(difficulty_confirm_stage1hard_on); //=difficulty_confirm_stage1hard_on 임시


            difficulty_normal_confirm_button = GameObject.Find("difficulty_confirm_buttonN"); //노말난이도 확인버튼
            Button difficulty_normal_confirm_b = difficulty_normal_confirm_button.GetComponent<Button>();
            difficulty_normal_confirm_b.onClick.AddListener(difficulty_confirm_stage1normal_change);


            difficulty_normal_confirm_button = GameObject.Find("difficulty_confirm_buttonH"); //하드난이도 확인버튼
            Button difficulty_hard_confirm_b = difficulty_normal_confirm_button.GetComponent<Button>();
            difficulty_hard_confirm_b.onClick.AddListener(difficulty_confirm_stage1hard_change);
        }


        //-------------------------------------------------소리
        sound_bar_fill = GameObject.Find("sound_bar_UI").GetComponent<Image>();     //사운드바
        sound_ratio = GameObject.Find("volume_ratio_text").GetComponent<Text>();    //사운드바 글씨

        soundu = GameObject.Find("sound_up_button");
        Button soundu_b = soundu.GetComponent<Button>();
        soundu_b.onClick.AddListener(sound_up);

        soundd = GameObject.Find("sound_down_button");
        Button soundd_b = soundd.GetComponent<Button>();
        soundd_b.onClick.AddListener(sound_down);

        //--------------------------------------------------그래픽옵션
        //dof
        dof_off = GameObject.Find("dof_unchecked");
        Button dof_off_b = dof_off.GetComponent<Button>();
        dof_off_b.onClick.AddListener(option_dof_on);

        dof_on = GameObject.Find("dof_checked");
        Button dof_on_b = dof_on.GetComponent<Button>();
        dof_on_b.onClick.AddListener(option_dof_off);

        //bloom
        blur_off = GameObject.Find("blur_unchecked");
        Button blur_off_b = blur_off.GetComponent<Button>();
        blur_off_b.onClick.AddListener(option_blur_on);

        blur_on = GameObject.Find("blur_checked");
        Button blur_on_b = blur_on.GetComponent<Button>();
        blur_on_b.onClick.AddListener(option_blur_off);

        //ab
        ab_off = GameObject.Find("ab_unchecked");
        Button ab_off_b = ab_off.GetComponent<Button>();
        ab_off_b.onClick.AddListener(option_ab_on);

        ab_on = GameObject.Find("ab_checked");
        Button ab_on_b = ab_on.GetComponent<Button>();
        ab_on_b.onClick.AddListener(option_ab_off);

        //blur
        bloom_off = GameObject.Find("bloom_unchecked");
        Button bloom_off_b = bloom_off.GetComponent<Button>();
        bloom_off_b.onClick.AddListener(option_bloom_on);

        bloom_on = GameObject.Find("bloom_checked");
        Button bloom_on_b = bloom_on.GetComponent<Button>();
        bloom_on_b.onClick.AddListener(option_bloom_off);
    }


    // Start is called before the first frame update
    void Start()
    {
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        //{
        difficulty_stage1normal = GameObject.Find("노말 모드"); // 시작시 노말모드 비활성화
        if (difficulty_stage1normal)
            difficulty_stage1normal.SetActive(false);
        difficulty_stage1hard = GameObject.Find("하드 모드"); //시작시 하드모드 비활성화
        if (difficulty_stage1hard)
            difficulty_stage1hard.SetActive(false);
        //}

        if (stage1difficulty == 2)
        {
            GameObject.Find("MainLight").GetComponent<Light>().color = Color.grey;
            GameObject.Find("MainLight").GetComponent<Light>().intensity = 0.2f;
        }
        if (stage1difficulty == 1 || stage1difficulty == 0)
        {
            if(GameObject.Find("torch"))
                GameObject.Find("torch").SetActive(false);
        }


        gui.GetComponent<AudioSource>().volume = 0.1f; //효과음 너무시끄러워서 0.1으로 고정함

        //GameObject.Find("Menu").SetActive(false); //게임이 시작할때는 메뉴창을 비활성화 상태로 만든다.
        if (menu)
            menu.SetActive(false); //게임이 시작할때는 메뉴창을 비활성화 상태로 만든다.

        //GameObject.Find("exit_confirm").SetActive(false);

        if (exitwindow)
            exitwindow.SetActive(false);

        if (VRpointer)
            VRpointer.SetActive(false);

        if (option)
            option.SetActive(false);

        if (difficulty_menu)
            difficulty_menu.SetActive(false);

        if (retry_menu)
            retry_menu.SetActive(false);

        if (blur_on)
            blur_on.SetActive(false);
        if (dof_on)
            dof_on.SetActive(false);
        if (ab_on)
            ab_on.SetActive(false);
        if (bloom_on)
            bloom_on.SetActive(false);



        if (stage1difficulty == 1)
        {
            difficulty_stage1normal.SetActive(true);
            WaveController[] wavecontrollers_normal = difficulty_stage1normal.GetComponentsInChildren<WaveController>();
            for (int i = 0; i < wavecontrollers_normal.Length; i++)  //노말난이도의 웨이브컨트롤러 모두 켜기
                wavecontrollers_normal[i].enabled = true;
            if (difficulty_stage1hard.activeSelf == true)
                difficulty_stage1hard.SetActive(false);

            RenderSettings.skybox = dayskybox;
        }
        if (stage1difficulty == 2)
        {
            difficulty_stage1hard.SetActive(true);
            WaveController[] wavecontrollers_hard = difficulty_stage1hard.GetComponentsInChildren<WaveController>();
            for (int i = 0; i < wavecontrollers_hard.Length; i++)  //하드난이도의 웨이브컨트롤러 모두 켜기
                wavecontrollers_hard[i].enabled = true;
            if (difficulty_stage1normal.activeSelf == true)
                difficulty_stage1normal.SetActive(false);

            RenderSettings.skybox = nightskybox;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stage1difficulty == 2)
            RenderSettings.ambientLight = Color.grey;
        RenderSettings.ambientIntensity = 0.8f;

        if (!GameObject.Find("maintower"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);
        }

        if (GameObject.Find("maintower") || SceneManager.GetActiveScene() != SceneManager.GetSceneByName("map01"))
        {
            if (isbloom == true&&bloom_off.activeSelf == true)
            {
                Bloom bloom;
                postprocess.profile.TryGetSettings(out bloom);
                bloom.active = true;
                bloom_off.SetActive(false);
                bloom_on.SetActive(true);
            }
            if (isbloom == false && bloom_off.activeSelf == false)
            {
                Bloom bloom;
                postprocess.profile.TryGetSettings(out bloom);
                bloom.active = false;
                bloom_off.SetActive(true);
                bloom_on.SetActive(false);
            }
            if (isab == true)
            {
                AmbientOcclusion abo;
                postprocess.profile.TryGetSettings(out abo);
                abo.active = true;
                ab_off.SetActive(false);
                ab_on.SetActive(true);
                Debug.Log("aon");
            }
            if (isab == false)
            {
                AmbientOcclusion abo;
                postprocess.profile.TryGetSettings(out abo);
                abo.active = false;
                ab_off.SetActive(true);
                ab_on.SetActive(false);
                Debug.Log("aoff");
            }
            if (isdof == false)
            {
                DepthOfField dof;
                postprocess.profile.TryGetSettings(out dof);
                dof.active = false;
                dof_off.SetActive(true);
                dof_on.SetActive(false);
                Debug.Log("doff");
            }
            if (isdof == true)
            {
                DepthOfField dof;
                postprocess.profile.TryGetSettings(out dof);
                dof.active = true;
                dof_off.SetActive(false);
                dof_on.SetActive(true);
                Debug.Log("don");
            }
            if (isblur == true)
            {
                /*MotionBlur motionblur;
                postprocess.profile.TryGetSettings(out motionblur);
                motionblur.active = true;*/
                blur_off.SetActive(false);
                blur_on.SetActive(true);
                postprocess_AA.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                //AA = true;
            }
            if (isblur == false)
            {
                /*MotionBlur motionblur;
                postprocess.profile.TryGetSettings(out motionblur);
                motionblur.active = false;*/
                blur_off.SetActive(true);
                blur_on.SetActive(false);
                postprocess_AA.antialiasingMode = PostProcessLayer.Antialiasing.None;
                //AA = false;
            }
        }



        GameObject.Find("GUI").transform.position =
            new Vector3(GameObject.Find("ui_position").transform.position.x - 0.02f,
            GameObject.Find("ui_position").transform.position.y + 0.1f,
            GameObject.Find("ui_position").transform.position.z);
        GameObject.Find("GUI").transform.LookAt(GameObject.Find("CenterEyeAnchor").transform);

        //GameObject.Find("GUI").transform.position = GameObject.Find("ui_position").transform.position;

        if (GameObject.Find("PlayerInfo"))
            PlayerInfo = GameObject.Find("PlayerInfo"); //실시간 받기

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01") || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1"))
        {

            PlayerInfo.transform.position = GameObject.Find("ui_position").transform.position;
            PlayerInfo.transform.LookAt(GameObject.Find("CenterEyeAnchor").transform);
        }
        if (menu.activeSelf == false && OVRInput.GetDown(OVRInput.RawButton.A)) //오큘러스 A버튼을 누르면
        //if (menu.activeSelf == false && Input.GetKeyDown(KeyCode.N))
        {
            //메뉴를 열고 있는 동안에는 체력바 비활성화
            if (PlayerInfo)
                PlayerInfo.SetActive(false);

            button_sound_yes();
            if (GameObject.Find("retry_confirm"))
                GameObject.Find("retry_confirm").SetActive(false);

            menu.SetActive(true); //메뉴창이 나온다

            VRpointer.SetActive(true);
            if (Tpointer)
                Tpointer.SetActive(false);

            SoundBar();

            Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
            for (int i = 0; i < every_buttons.Length; i++)
            {
                every_buttons[i].enabled = true;
            } //모든 버튼을 활성화시킨다.
        }

        else if (menu.activeSelf == true && OVRInput.GetDown(OVRInput.RawButton.A)) //메뉴창이 활성화되있는 상태에서 A버튼을 누르면
        //else if (menu.activeSelf == true && Input.GetKeyDown(KeyCode.N)) //메뉴창이 활성화되있는 상태에서 N버튼을 누르면
        {
            button_sound_no();

            {
                menu.SetActive(false); //메뉴화면이 비활성화 된다.
                exitwindow.SetActive(false);
                //if (VRpointer)
                //    VRpointer.SetActive(true);
                option.SetActive(false);
                difficulty_menu.SetActive(false);

                if (PlayerInfo)
                    PlayerInfo.SetActive(true);

                if (VRpointer)
                    VRpointer.SetActive(false);

                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01")
                    || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1")||
                    SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01")&&SceneChanger.scoreboard_status!=0)
                {
                    Tpointer.SetActive(true);
                }

                Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
                for (int i = 0; i < every_buttons.Length; i++)
                {
                    every_buttons[i].enabled = false;
                } //모든 버튼을 비활성화시킨다.
            }
        }

    }



    public void difficulty_normal_cancle_on() //노말난이도 옵션창 끄기 (X)
    {
        button_sound_no();
        difficulty_normal_confirm.SetActive(false);

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = false;
        }
        Button[] difficulty_menu_bt = difficulty_menu.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < difficulty_menu_bt.Length; i++)
        {
            difficulty_menu_bt[i].enabled = true;
        }
    }
    public void difficulty_hard_cancle_on() //하드 "
    {
        button_sound_no();
        difficulty_hard_confirm.SetActive(false);

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = false;
        }
        Button[] difficulty_menu_bt = difficulty_menu.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < difficulty_menu_bt.Length; i++)
        {
            difficulty_menu_bt[i].enabled = true;
        }


    }

    public void difficulty_confirm_stage1hard_on() //하드난이도 옵션창 켜기
    {

        if (stage1difficulty == 1 || stage1difficulty == 0)
        {
            button_sound_yes();
            if (difficulty_normal_confirm)
                difficulty_normal_confirm.SetActive(false);
            difficulty_hard_confirm.SetActive(true);

            Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
            for (int i = 0; i < every_buttons.Length; i++)
            {
                every_buttons[i].enabled = false;
            }
            Button[] difficulty_hard_confirm_bt = difficulty_hard_confirm.GetComponentsInChildren<Button>();
            for (int i = 0; i < difficulty_hard_confirm_bt.Length; i++)
            {
                difficulty_hard_confirm_bt[i].enabled = true;
            } //하드모드변경 확인창을 제외한 모든 오브젝트의 버튼을 비활성화 시킨다.
        }
        if (stage1difficulty == 2)
            button_sound_no();
    }

    public void difficulty_confirm_stage1normal_on() //노말난이도 옵션창 켜기
    {

        if (stage1difficulty == 2 || stage1difficulty == 0)
        {
            button_sound_yes();
            if (difficulty_hard_confirm)
                difficulty_hard_confirm.SetActive(false);
            difficulty_normal_confirm.SetActive(true);

            Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
            for (int i = 0; i < every_buttons.Length; i++)
            {
                every_buttons[i].enabled = false;
            }
            Button[] difficulty_normal_confirm_bt = difficulty_normal_confirm.GetComponentsInChildren<Button>();
            for (int i = 0; i < difficulty_normal_confirm_bt.Length; i++)
            {
                difficulty_normal_confirm_bt[i].enabled = true;
            } //노말모드변경 확인창을 제외한 모든 오브젝트의 버튼을 비활성화 시킨다.
        }
        if (stage1difficulty == 1)
            button_sound_no();
    }

    public void difficulty_confirm_stage1hard_change() //난이도 옵션창에서 확인누르고 하드난이도로 바꾸기
    {
        button_sound_yes();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        {

            if (difficulty_stage1normal)
                difficulty_stage1normal.SetActive(false);
            difficulty_stage1hard.SetActive(true);
            difficulty_hard_confirm.SetActive(false);


            WaveController[] wavecontrollers_hard = difficulty_stage1hard.GetComponentsInChildren<WaveController>();
            for (int i = 0; i < wavecontrollers_hard.Length; i++) //하드난이도의 웨이브컨트롤러 모두 켜기
                wavecontrollers_hard[i].enabled = true;

            WaveController[] wavecontrollers_normal = difficulty_stage1normal.GetComponentsInChildren<WaveController>();
            MonsterSpawnerIndex[] monsterspawners_normal = difficulty_stage1normal.GetComponentsInChildren<MonsterSpawnerIndex>();
            for (int i = 0; i < wavecontrollers_normal.Length; i++)  //노말난이도의 웨이브컨트롤러 모두 끄기
            {
                wavecontrollers_normal[i].wavecount = 0;
                wavecontrollers_normal[i].enabled = false;
            }
            for (int i = 0; i < monsterspawners_normal.Length; i++)
            {
                monsterspawners_normal[i].SpawnCount = 0;
                //monsterspawners_normal[i].points[0] = null;
                //monsterspawners_normal[i].points[1] = null;
                monsterspawners_normal[i].enabled = false;
            }
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);  //enemy태그를 가지고있는 모든 오브젝트를 파괴한다.

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.

        stage1difficulty = 2;
        SceneManager.LoadScene("map01");
    }
    public void difficulty_confirm_stage1normal_change() //난이도 옵션창에서 확인누르고 노말난이도로 바꾸기
    {
        button_sound_yes();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01"))
        {
            if (difficulty_stage1hard)
                difficulty_stage1hard.SetActive(false);
            difficulty_stage1normal.SetActive(true);
            difficulty_normal_confirm.SetActive(false);
            WaveController[] wavecontrollers_normal = difficulty_stage1normal.GetComponentsInChildren<WaveController>();
            for (int i = 0; i < wavecontrollers_normal.Length; i++) //노말난이도의 웨이브컨트롤러 모두 켜기
                wavecontrollers_normal[i].enabled = true;

            WaveController[] wavecontrollers_hard = difficulty_stage1hard.GetComponentsInChildren<WaveController>();
            MonsterSpawnerIndex[] monsterspawners_hard = difficulty_stage1hard.GetComponentsInChildren<MonsterSpawnerIndex>();

            for (int i = 0; i < wavecontrollers_hard.Length; i++)  //노말난이도의 웨이브컨트롤러 모두 끄기
            {
                wavecontrollers_hard[i].enabled = false;
                wavecontrollers_hard[i].wavecount = 0;
            }
            for (int i = 0; i < monsterspawners_hard.Length; i++)
            {
                monsterspawners_hard[i].SpawnCount = 0;
                monsterspawners_hard[i].enabled = false;
            }
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);  //enemy태그를 가지고있는 모든 오브젝트를 파괴한다.

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.

        stage1difficulty = 1;
        SceneManager.LoadScene("map01");
    }



    public void difficulty_menu_cancle_on()
    {
        button_sound_no();
        difficulty_menu.SetActive(false);

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.
    }



    //------------------------------------------------------------------------------------------------
    public void exit_confirm()
    {
        button_sound_yes();
        exitwindow.SetActive(true); //5번째 메뉴버튼을 누르면 종료확인창을 활성화 한다.

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = false;
        } //모든 버튼을 비활성화시킨다.

        Button[] exit_confirm_bt = exitwindow.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < exit_confirm_bt.Length; i++)
        {
            exit_confirm_bt[i].enabled = true;
        } //exit_confirm의 모든버튼을 활성화시킨다.
    }

    public void retry() //현재스테이지 다시시작
    {
        button_sound_yes();
        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //현재 스테이지를 다시 불러옴.
    }

    public void retry_confirm_on()
    {

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01"))
        {
            button_sound_no();
        }
        button_sound_yes();
        retry_menu.SetActive(true);

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = false;
        } //모든 버튼을 비활성화시킨다.

        Button[] retry_confirm_bt = retry_menu.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < retry_confirm_bt.Length; i++)
        {
            retry_confirm_bt[i].enabled = true;
        } //retry_confirm_bt의 모든버튼을 활성화시킨다.
    }
    public void retry_cancle_on()
    {
        button_sound_no();
        retry_menu.SetActive(false); //종료 확인창에서 취소버튼을 누르게되면 종료확인창을 비활성화 한다.

        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.
    }
    public void exit_confirm_button()
    {
        button_sound_yes();
        Button[] every_buttons = gui.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01"))
            quit_game();
        SceneManager.LoadScene("menu01"); //종료 확인창에서 확인버튼을 누르게되면 메인화면씬으로 전환 한다.
    }
    public void exit_cancle_button_()
    {
        button_sound_no();
        exitwindow.SetActive(false); //종료 확인창에서 취소버튼을 누르게되면 종료확인창을 비활성화 한다.

        Button[] every_buttons = GameObject.Find("GUI").GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.
    }

    public void menu_cancle_button()
    {
        button_sound_no();
        //Time.timeScale = 1f;
        menu.SetActive(false); //메뉴창에서 취소버튼을 누르게되면 메뉴창을 비활성화 한다.
        VRpointer.SetActive(false);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu01")
                    || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1") ||
                    SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01") && SceneChanger.scoreboard_status != 0)
        {

            Tpointer.SetActive(true);
        }

        if (PlayerInfo)
            PlayerInfo.SetActive(true);

        Button[] every_buttons = GameObject.Find("GUI").GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.
    }

    public void option_window()
    {
        button_sound_yes();
        option.SetActive(true); //옵션을 누르면 옵션창이 활성화 된다.
        Button[] every_buttons = GameObject.Find("GUI").GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = false;
        } //모든 버튼을 비활성화시킨다.

        Button[] option_window_bt = option.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < option_window_bt.Length; i++)
        {
            option_window_bt[i].enabled = true;
        } //옵션 하위의 모든 버튼을 활성화 시킨다.
    }
    public void option_cancle()
    {
        button_sound_no();
        option.SetActive(false); // 옵션창에서 취소버튼을 누르면 비활성화 된다.

        Button[] every_buttons = GameObject.Find("GUI").GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = true;
        } //모든 버튼을 활성화시킨다.

        Debug.Log("cancleoption");
    }

    public void difficulty_menu_on() //난이도 조정 창 열기
    {
        button_sound_yes();
        difficulty_menu.SetActive(true);

        Button[] every_buttons = GameObject.Find("GUI").GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < every_buttons.Length; i++)
        {
            every_buttons[i].enabled = false;
        } //모든 버튼을 활성화시킨다.

        Button[] difficulty_menu_bt = difficulty_menu.GetComponentsInChildren<Button>(); //~
        for (int i = 0; i < difficulty_menu_bt.Length; i++)
        {
            difficulty_menu_bt[i].enabled = true;
        } //난이도 조정창 하위의 모든 버튼을 활성화 시킨다.


        if (difficulty_hard_confirm)
        {
            difficulty_hard_confirm.SetActive(false);
        }
        if (difficulty_normal_confirm)
        {
            difficulty_normal_confirm.SetActive(false);
        }
    }
    public void difficulty_menu_off()
    {
        button_sound_no();
        difficulty_menu.SetActive(false);
    }


    //--------------------------------------------------------------------------------------------- 그래픽옵션
    public void option_blur_off()
    {
        isblur = false;
        button_sound_no();

    }
    public void option_blur_on()
    {
        isblur = true;
        button_sound_yes();

    }
    //-----------------------------------------------------
    public void option_dof_on()
    {
        isdof = true;
        button_sound_yes();

    }
    public void option_dof_off()
    {
        isdof = false;
        button_sound_no();

    }
    //-------------------------------------------------------
    public void option_ab_off()
    {
        isab = false;
        button_sound_no();

    }
    public void option_ab_on()
    {
        isab = true;
        button_sound_yes();

    }
    //---------------------------------------------------------
    public void option_bloom_off()
    {
        isbloom = false;
        button_sound_no();

    }
    public void option_bloom_on()
    {
        isbloom = true;
        button_sound_yes();

    }

    //----------------------------------------------------------------------------------사운드
    public void SoundBar()

    {
        //float AudioVolume_now = AudioListener.volume; //볼륨값을 받아옴
        float AudioVolume_now = Mathf.Round(AudioListener.volume * 10) * 0.1f; //AudioVolume_now는 현재볼륨 값의 소숫점둘째자리에서 반올림한 값이다.

        sound_bar_fill.fillAmount = AudioVolume_now / 2;

        sound_ratio.text = string.Format("현재 볼륨 {0}/" + "2", AudioVolume_now);

        if (AudioVolume_now < 0)
        {
            AudioListener.volume = 0f;
        }
        if (AudioListener.volume > 2)
        {
            AudioListener.volume = 2f;
        }
    }

    public void sound_up()
    {
        button_sound_yes();
        AudioListener.volume += 0.25f;
    }

    public void sound_down()
    {
        button_sound_yes();
        AudioListener.volume -= 0.25f;
    }

    public void button_sound_yes()
    {
        gui.GetComponent<AudioSource>().volume = 0.1f;
        gui.GetComponent<AudioSource>().clip = effect_confirm;
        gui.GetComponent<AudioSource>().Play();
    }
    public void button_sound_no()
    {
        gui.GetComponent<AudioSource>().volume = 0.2f;
        gui.GetComponent<AudioSource>().clip = effect_cancle;
        gui.GetComponent<AudioSource>().Play();
    }

    public void quit_game()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false; //Application.Quit은 빌드에서만 실행됨
    }


}