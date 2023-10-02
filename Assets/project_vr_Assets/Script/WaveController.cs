using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaveController : MonoBehaviour
{
    public MonsterSpawnerIndex[] wave; //MonsterSpanwerIndex를 wave로 지정
    public int maxwavecount = 0; //최대웨이브 수
    public int wavecount = 0; //MonsterSpanwerIndex의 배열값

    // Start is called before the first frame update
    void Awake()
    {
        wave[0].enabled = true;
        //GetComponent<MonsterSpawnerIndex>();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if(GetComponent<MonsterSpawnerIndex>().i == GetComponent<MonsterSpawnerIndex>().MaxSpawn)
        //wave[0].enabled = false;
        if (wave[wavecount].SpawnCount >= wave[wavecount].MaxSpawn) //MonsterSpawnerIndex의 스폰카운트가 맥스스폰보다 크거나 같으면
        {
            wave[wavecount].enabled = false; //현재 활성화되있는 배열을 비활성화하고
            wavecount++; //MonsterSpawnerIndex의 배열값 +1


            if (wave[wavecount - 1].enabled == false) //만약 현재배열-1이 비활성화 되어있으면
            {
                wave[wavecount].enabled = true; // 현재배열을 활성화 한다.
            }
        }
        
        if (maxwavecount == wavecount)
        {
            this.enabled = false;
        }

        
    }


}
