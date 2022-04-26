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
    public GameObject Fire_1 => Resources.Load<GameObject>("Prefabs/Stuffs/Fire_Styles1");
    public GameObject Fire_2 => Resources.Load<GameObject>("Prefabs/Stuffs/Fire_Styles2");
    public GameObject Flag => Resources.Load<GameObject>("Prefabs/Stuffs/Flag");
    public GameObject Magic_Circle => Resources.Load<GameObject>("Prefabs/Stuffs/Magic_Circle");
    public GameObject Shell_Effect => Resources.Load<GameObject>("Prefabs/Stuffs/Shell_Effect");


    //use for later
    /* public GameObject Zombie_Normal => Resources.Load<GameObject>("Prefabs/Enemies/Zombie_Normal");
     public GameObject Zombie_Miner => Resources.Load<GameObject>("Prefabs/Enemies/Zombie_Miner");
     public GameObject Zombie_Wounded => Resources.Load<GameObject>("Prefabs/Enemies/Zombie_Wounded");
     public GameObject Witch => Resources.Load<GameObject>("Prefabs/Stuffs/Enemies/Zombie_Mage");
     public GameObject Skeleton => Resources.Load<GameObject>("Prefabs/Stuff/Enemies/Skeleton");
 */


    public int gamePoint;
    public int killPoint;

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

public enum SkillSet 
{
    Default, ArchSword, DominusLapidis
}

