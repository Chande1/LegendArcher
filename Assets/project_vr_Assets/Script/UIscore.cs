using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIscore : MonoBehaviour
{
    // Start is called before the first frame update

    Text Score;


    void Awake()
    {
        Score = GetComponent<Text>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WaveManager wavemanager = GameObject.Find("UI_Manager").GetComponent<WaveManager>();
        Score.text = "REMAINED MONSTERS " +(wavemanager.KillScore.ToString()) + "/" + (wavemanager.WaveMonsterCount.ToString());
    }
}
