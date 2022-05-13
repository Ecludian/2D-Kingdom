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
                    GetAction?.Invoke(transform, transform.tag);
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

    public Action<Transform, string> GetAction { get; set; }

    private float castTime;
    private float spawnTime;

    
    private void Start()
    {
        castTime = delayTime;
        
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
