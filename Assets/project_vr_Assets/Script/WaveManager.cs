using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    public int KillScore = 0;
    public int WaveMonsterCount = 0;

    [SerializeField] GameObject spawnerN;
    [SerializeField] GameObject spawnerH;
    // Start is called before the first frame update

    void Awake()
    {
    
    }
    void Start()
    {
        spawnerN = GameObject.Find("노말 모드");
        spawnerH = GameObject.Find("하드 모드");
        if (spawnerH&&spawnerH.activeSelf == true)
            WaveMonsterCount = 52;
        if (spawnerN && spawnerN.activeSelf == true)
            WaveMonsterCount = 46;
    }


}