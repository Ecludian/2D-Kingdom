using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagesActive : MonoBehaviour
{
    public GameObject[] nestSpawner;
    public GameObject[] holeSpawner;

    private GameManager manager;
    private void generatorTier_1(Transform spawner, string tag)
    {
        var generator = spawner.GetComponent<GenerateActive>();
        for (int i = 0; i < generator.Creeps.Length; i++)
        {
            if (generator.Creeps[i] == null)
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
                creep.GetComponent<EnemyActive>().spawner = spawner.gameObject;
                creep.GetComponent<EnemyActive>().slotNum = i;
               
                generator.Creeps[i] = Instantiate(creep, spawner.position, Quaternion.identity, transform);
                break;
            }

        }
    }
    private void generatorTier_2(Transform spawner, string tag)
    {
        var generator = spawner.GetComponent<GenerateActive>();
        for (int i = 0; i < generator.Creeps.Length; i++)
        {
            if (generator.Creeps[i] == null)
            {
                var gm = GameManager.Instance;
                GameObject creep = null;
                if (tag == "Nest")
                {
                    creep = (i < 4) ? gm.Zombie_Normal : gm.Zombie_Miner;

                }
                else if (tag == "Hole")
                {
                    creep = (i < 1) ? gm.Zombie_Wounded : gm.Witch;
                }
                creep.GetComponent<EnemyActive>().spawner = spawner.gameObject;
                creep.GetComponent<EnemyActive>().slotNum = i;
           
                generator.Creeps[i] = Instantiate(creep, spawner.position, Quaternion.identity, transform);
                break;
            }
        }
    }
    private void generatorTier_3(Transform spawner, string tag)
    {
        var generator = spawner.GetComponent<GenerateActive>();
        for (int i = 0; i < generator.Creeps.Length; i++)
        {
            if (generator.Creeps[i] == null)
            {
                var gm = GameManager.Instance;
                GameObject creep = null;
                if (tag == "Nest")
                {
                    creep = (i < 5) ? gm.Zombie_Normal : gm.Zombie_Miner;

                }
                else if (tag == "Hole")
                {
                    creep = (i < 2) ? gm.Zombie_Normal : gm.Witch;
                }
                creep.GetComponent<EnemyActive>().spawner = spawner.gameObject;
                creep.GetComponent<EnemyActive>().slotNum = i;
                generator.Creeps[i] = Instantiate(creep, spawner.position, Quaternion.identity, transform);
                break;
            }
        }
    }
    private void StageSetup(LevelSet level,
                            Action<Transform, string> action,
                            params int[] stock) {

        manager.levelPoint = level;
        foreach (var nest in nestSpawner)
        {
            var generator = nest.GetComponent<GenerateActive>();
            nest.SetActive(true);
            generator.Stock = stock[0];
            generator.GetAction = action;
        }
        foreach (var hole in holeSpawner)
        {
            var generator = hole.GetComponent<GenerateActive>();
            hole.SetActive(true);
            generator.Stock = stock[1];
            generator.GetAction = action;
        }


    }

    private void LateUpdate()
    {
        if (manager == null)
        {
            manager = GameManager.Instance;
        }

        var level = manager.levelPoint;
        if (manager.killPoint > 99 && level != LevelSet.Hard)
        {
            //set level to hard
            StageSetup(LevelSet.Hard, generatorTier_3, 7, 5);
        }
        else if (manager.killPoint > 30 && manager.killPoint <= 99 && level != LevelSet.Normal)
        {
            //set level to normal
            StageSetup(LevelSet.Normal, generatorTier_2, 5, 3);
        }
        else if (manager.killPoint <= 30 && level != LevelSet.Easy)
        {
            //set level to easy
            StageSetup(LevelSet.Easy, generatorTier_1, 3, 1);

        }
    }
}