using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu_difficulty_UI : MonoBehaviour
{

    [SerializeField] GameObject menu_normal;
    [SerializeField] GameObject menu_hard;
    [SerializeField] GameObject menu_exit;
    [SerializeField] GameObject menu_tutorial;
    [SerializeField] GameObject uimanager;

    void Awake()
    {
        uimanager = GameObject.Find("UI_Manager");
        menu_hard = GameObject.Find("hard");
        menu_exit = GameObject.Find("game_exit");
        menu_tutorial = GameObject.Find("tutorial");
        menu_normal = GameObject.Find("normal");
        //--------------------------------------------------------
        Button menu_normal_b = menu_normal.GetComponent<Button>();
        menu_normal_b.onClick.AddListener(menu_normal_on);

        Button menu_hard_b = menu_hard.GetComponent<Button>();
        menu_hard_b.onClick.AddListener(menu_hard_on);

        Button menu_tutorial_b = menu_tutorial.GetComponent<Button>();
        menu_tutorial_b.onClick.AddListener(menu_tutorial_on);

        Button menu_exit_b = menu_exit.GetComponent<Button>();
        menu_exit_b.onClick.AddListener(menu_exit_on);

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void menu_normal_on()
    {
        uimanager.GetComponent<UI_manager>().difficulty_confirm_stage1normal_change();
    }
    public void menu_hard_on()
    {
        uimanager.GetComponent<UI_manager>().difficulty_confirm_stage1hard_change();
    }

    public void menu_tutorial_on()
    {
        SceneManager.LoadScene("Tutorial_Test1");
    }

    public void menu_exit_on()
    {
        Application.Quit();
        if (Application.isPlaying == true)
            Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
    }
}
