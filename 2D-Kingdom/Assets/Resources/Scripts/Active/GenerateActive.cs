using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerateActive : MonoBehaviour
{
    public GameObject[] Creeps;
    public float delayTime;

    public bool isReadySpawn
    {
        set
        {
            if (value)
            {
                if (spawnTime > 0)
                {
                    spawnTime -= Time.deltaTime;
                }
                if (spawnTime < 0)
                {
                    spawnTime = 1;
                    GetAction?.Invoke(transform.tag);
                }
            }
        }
    }
    public int Stock
    {
        get
        {
            return stock;
        }
        set
        {
            stock = value;
            Creeps = new GameObject[stock];
        }
    }
    private int stock;

    public Action<string> GetAction { get; set; }

    private float castTime;
    private float spawnTime;

    private void generatorTier_1(string tag)
    {
        for (int i = 0; i < Creeps.Length; i++)
        {
            if (Creeps[i] == null)
            {
                var gm = GameManager.Instance;
                GameObject creep = null;
                if (tag == "Nest")
                {
                    creep = gm.Zombie_Normal;

                }
                else if (tag == "Hole")
                {
                    creep = gm.Zombie_Wounded;
                }
                creep.GetComponent<EnemyActive>().spawner = gameObject;
                creep.GetComponent<EnemyActive>().slotNum = i;
                var generator = GetComponent<GenerateActive>();
                generator.Creeps[i] = Instantiate(creep, transform.position, Quaternion.identity, transform);
                break;
            }

        }
    }
    private void generatorTier_2(string tag)
    {
        for (int i = 0; i < Creeps.Length; i++)
        {
            if (Creeps[i] == null)
            {
                var gm = GameManager.Instance;
                GameObject creep = null;
                if (tag == "Nest")
                {
                    creep = (i < 4) ? gm.Zombie_Normal : gm.Zombie_Miner;

                }
                else if (tag == "Hole")
                {
                    creep = (i < 4) ? gm.Zombie_Wounded : gm.Witch;
                }
                creep.GetComponent<EnemyActive>().spawner = gameObject;
                creep.GetComponent<EnemyActive>().slotNum = i;
                var generator = GetComponent<GenerateActive>();
                generator.Creeps[i] = Instantiate(creep, transform.position, Quaternion.identity, transform);
                break;
            }
        }
    }
    private void generatorTier_3(string tag)
    {
        for (int i = 0; i < Creeps.Length; i++)
        {
            if (Creeps[i] == null)
            {
                var gm = GameManager.Instance;
                GameObject creep = null;
                if (tag == "Nest")
                {
                    creep = (i < 5) ? gm.Zombie_Normal : gm.Zombie_Miner;

                }
                else if (tag == "Hole")
                {
                    creep = (i < 3) ? gm.Zombie_Normal : gm.Witch;
                }
                creep.GetComponent<EnemyActive>().spawner = gameObject;
                creep.GetComponent<EnemyActive>().slotNum = i;
                var generator = GetComponent<GenerateActive>();
                generator.Creeps[i] = Instantiate(creep, transform.position, Quaternion.identity, transform);
                break;
            }
        }
    }
    private void Start()
    {
        castTime = delayTime;
        stock = 5;
        GetAction = generatorTier_2;
    }
    private void Update()
    {
        if (castTime > 0)
        {
            castTime -= Time.deltaTime;
        }
        if (castTime < 0)
        {
            castTime = 0;
            spawnTime = 1;
        }

        isReadySpawn = transform.childCount < stock;

    }
}
