using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Origin_SkeletonCreep
    {
        get
        {
            return Resources.Load<GameObject>("Prefabs/Enemies/Skeleton");
        }
    }
    //using lambda
    public GameObject Origin_Smoke => Resources.Load<GameObject>("Prefabs/Stuffs/Smoke");

    public static GameManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }
}

public enum EnemySet
{
    Deafult, Damaged, Native, Warrior, Witch, Skeleton
}

public enum UnitState
{
    Idle, Move, Attack, Cast, Dead
}
