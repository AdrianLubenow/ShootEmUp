using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [Header("--- Enemy ---")]
    [SerializeField] private GameObject EnemyObject;
    [SerializeField] private int enemyAmountToPool;

    [Header("--- MiniBoss ---")]
    [SerializeField] private GameObject MiniBossObject;
    [SerializeField] private int miniBossAmountToPool;

    [Header("--- Boss ---")]
    [SerializeField] private GameObject BossObject;
    [SerializeField] private int bossAmountToPool;

    private readonly List<GameObject> playerPooledObjects = new List<GameObject>();
    private readonly List<GameObject> enemyPooledObjects = new List<GameObject>();
    private readonly List<GameObject> miniBossPooledObjects = new List<GameObject>();
    private readonly List<GameObject> bossPooledObjects = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        Initialize();
    }

    void Initialize()
    {
        GameObject tmp;
        if (EnemyObject != null)
        {
            for (int i = 0; i < enemyAmountToPool; i++)
            {
                tmp = Instantiate(EnemyObject, transform);
                tmp.SetActive(false);
                enemyPooledObjects.Add(tmp);
            }
        }
        if (MiniBossObject != null)
        {
            for (int i = 0; i < miniBossAmountToPool; i++)
            {
                tmp = Instantiate(MiniBossObject, transform);
                tmp.SetActive(false);
                miniBossPooledObjects.Add(tmp);
            }
        }
        if (BossObject != null)
        {
            for (int i = 0; i < bossAmountToPool; i++)
            {
                tmp = Instantiate(BossObject, transform);
                tmp.SetActive(false);
                bossPooledObjects.Add(tmp);
            }
        }
    }

    public void SetPlayerObjectPool(GameObject go, int amount)
    {
        if (go != null)
        {
            GameObject tmp;
            foreach (var item in playerPooledObjects)
            {
                Destroy(item);
            }

            playerPooledObjects.Clear();

            for (int i = 0; i < amount; i++)
            {
                tmp = Instantiate(go, transform);
                tmp.SetActive(false);
                playerPooledObjects.Add(tmp);
            }
        }
    }

    public GameObject GetPlayerPooledObject()
    {
        for (int i = 0; i < playerPooledObjects.Count; i++)
        {
            if (!playerPooledObjects[i].activeInHierarchy)
            {
                return playerPooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetEnemyPooledObject()
    {
        for (int i = 0; i < enemyPooledObjects.Count; i++)
        {
            if (!enemyPooledObjects[i].activeInHierarchy)
            {
                return enemyPooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetMiniBossPooledObject()
    {
        for (int i = 0; i < miniBossPooledObjects.Count; i++)
        {
            if (!miniBossPooledObjects[i].activeInHierarchy)
            {
                return miniBossPooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetBossPooledObject()
    {
        for (int i = 0; i < bossPooledObjects.Count; i++)
        {
            if (!bossPooledObjects[i].activeInHierarchy)
            {
                return bossPooledObjects[i];
            }
        }
        return null;
    }
}