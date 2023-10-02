using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<GameObject> pooledObjects_go;
    public List<GameObject> pooledObjects_ba;
    public GameObject objectToPool_go;
    public GameObject objectToPool_ba;
    public int amountToPool_go;
    public int amountToPool_ba;
    

    
    private void Awake()
    {
        Instance = this;
    }

    /*public void GetPoolGoblinPool() //몬스터 풀 생성
    {
        //GameObject go = new GameObject();
        pooledObjects_go = new List<GameObject>();
        GameObject go;
        for (int i = 0; i < amountToPool; i++)
        {
            go = Instantiate(objectToPool_go);
            go.SetActive(false);
            pooledObjects_go.Add(go);
        }
        //return go;
    }*/

    /*public GameObject GetPoolBaPool()
    {
        GameObject go = new GameObject();
        pooledObjects_ba = new List<GameObject>();
        //GameObject go;
        for (int i = 0; i < amountToPool_ba; i++)
        {
            go = Instantiate(objectToPool_ba);
            go.SetActive(false);
            pooledObjects_ba.Add(go);
        }
        return go;
    }*/



    //pooledobjects_go에 있는 고블린이 활성화되면 다음배열의 고블린을 활성화시킨다.
    public GameObject CreateGoblinPool()//int _amount 
    {
            for (int i = 0; i < amountToPool_go; i++)
            {
                if (!pooledObjects_go[i].activeInHierarchy)
                {
                    return pooledObjects_go[i];
                }
            }
            return null;
    }
    //밴딧가져오기
    public GameObject CreateBaPool() //int _amount 
    {
        for (int i = 0; i < amountToPool_ba; i++)
        {
            if (!pooledObjects_ba[i].activeInHierarchy)
            {
                return pooledObjects_ba[i];
            }
        }
        return null;
    }





    public void ReturnGoblinPool(GameObject go)
    {
    }

    public void ReturnBaPool(GameObject go)
    {
    }

    /*public GameObject GenerateMonster()
    {
        // GGB 
        GetPoolGoblinPool();
        GetPoolGoblinPool();
        GetPoolBaPool();


        // GGBB


    }*/


    private void Start()
    {
        /*// go pool create
        CreateGoblinPool(amountToPool);

        // ba pool create
        CreateBaPool(amountToPool);*/








        //고블린풀 생성
        //GameObject go = new GameObject();
        pooledObjects_go = new List<GameObject>();
        GameObject go;
        for (int i = 0; i < amountToPool_go; i++)
        {
            go = Instantiate(objectToPool_go);
            go.SetActive(false);
            pooledObjects_go.Add(go);
        }

        //밴딧풀 생성
        pooledObjects_ba = new List<GameObject>();
        GameObject ba;
        for (int i = 0; i < amountToPool_ba; i++)
        {
            ba = Instantiate(objectToPool_ba);
            ba.SetActive(false);
            pooledObjects_ba.Add(ba);
        }
        //return go;
        /*pooledObjects = new List<GameObject>();
        GameObject go;
        for(int i=0; i<amountToPool; i++)
        {
            go = Instantiate(objectToPool);
            go.SetActive(false);
            pooledObjects.Add(go);
        }*/
    }

    /*
    public GameObject GetPooledObject()
    {
        for (int i=0; i<amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
    */
}

