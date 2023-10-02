using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerUI : MonoBehaviour
{
	public Image towerHP_UI;
	public Text towerHP_text;

	public Image Process_UI;
	public Text Process_text;

	GameObject uimanager;

    public void Awake()
    {
		uimanager = GameObject.Find("UI_Manager");
    }
    // Start is called before the first frame update
    public void Update()

	{

		PlayerHPbar();
		Processingbar();

	}



	public void PlayerHPbar()

	{
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01") && GameObject.Find("maintower")
			|| SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1") && GameObject.Find("maintower"))
		{
			float Tower_HP = GameObject.Find("maintower").GetComponent<HP_M>().Hp; //캐릭터 hp를 받아옴

			towerHP_UI.fillAmount = Tower_HP / GameObject.Find("maintower").GetComponent<tower_originalHP>().t_originalHP;

			towerHP_text.text = string.Format("타워체력 {0}/" + GameObject.Find("maintower").GetComponent<tower_originalHP>().t_originalHP, Tower_HP);
		}
	}

	public void Processingbar()
    {
		if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("map01")
			|| SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial_Test1"))

		{
			float Kill_Count = uimanager.GetComponent<WaveManager>().KillScore;

			Process_UI.fillAmount = Kill_Count / uimanager.GetComponent<WaveManager>().WaveMonsterCount;

			Process_text.text = string.Format("처치한 몬스터 {0}/" + uimanager.GetComponent<WaveManager>().WaveMonsterCount, Kill_Count);
        }
		
    }
}

