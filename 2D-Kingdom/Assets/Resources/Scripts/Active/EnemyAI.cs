using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    //AI PATHFINDING
    public float nextWaypointDistance = 3f;

    Path path;
    int currentwayPoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;

    public Transform Target;
    public UnitState State;
    public EnemySet Creep;
    public bool isAlive;
    public bool isForward;
    public float moveSpeed;
    public float healthPoint;
    public float atkSpeed;
    public GameObject spawner;
    public int slotNum;

    private Rigidbody2D rb;
    private Animator enemyAnim;
    private AudioSource enemyAudio;
    private float attackTime;
    private GenerateActive generator;
    private GameObject creep;


    private void Awake()
    {

        enemyAnim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();

        //ai
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, .5f);



    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, Target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentwayPoint = 0;
        }
    }
    private void Enemy_Attack()
    {
        var player = Target.GetComponent<PlayerActive>();
        var gm = GameManager.Instance;
        if (attackTime == 0 && player.isAlive)
        {
            enemyAnim.SetTrigger("Attack");
            if (Creep == EnemySet.Witch)
            {
                var location = RandomPosition(transform.position);
                var vfx = Instantiate(gm.Origin_Smoke, location + new Vector3(0f, -0.56f, 0f), Quaternion.identity);
                Instantiate(gm.Origin_SkeletonCreep, location, Quaternion.identity);
                Destroy(vfx, 1f);
            }
            else
            {
                enemyAudio.Play();
            }
            //Cooldown 
            attackTime = atkSpeed;
        }
    }

    private void Enemy_Stance(Action attack = null)
    {
        enemyAnim.SetBool("isMoving", false);
        State = UnitState.Idle;
        attack?.Invoke();

    }

    private void Enemy_Movement(bool isForward)
    {
        /*  var moveToward = Vector3.MoveTowards(transform.position, Target.position, moveSpeed * Time.deltaTime);

          var towardPos = moveToward - transform.position;
          ChangeDirection(towardPos, isForward); //try without trransform position
          if (isForward)
          {
              transform.position += towardPos;
          }
          else
          {
              transform.position -= towardPos;
          }*/
        //AI
        if (path == null)
            return;
        if (currentwayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentwayPoint] - rb.position).normalized;
        Vector2 force = direction * moveSpeed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentwayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentwayPoint++;
        }

        enemyAnim.SetBool("isMoving", true);
        State = UnitState.Move;
    }

    private void Enemy_Death()
    {
        if (healthPoint <= 0)
        {
            isAlive = false;
            healthPoint = 0;
            State = UnitState.Dead;
            enemyAnim.SetTrigger("Dead");
            if (Creep == EnemySet.Skeleton)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 3f);
            }

        }
    }

    private void OnDisable()
    {
        if (!gameObject.scene.isLoaded) return;
        var gm = GameManager.Instance;
        switch (Creep)
        {
            case EnemySet.Damaged:
                var vfx_fire1 = Instantiate(gm.Fire_1, transform.position, Quaternion.identity);
                Destroy(vfx_fire1, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1f);
                Instantiate(gm.Power_Green, transform.position, Quaternion.identity);
                gm.killPoint++;
                break;
            case EnemySet.Native:
                creep = gm.Zombie_Wounded;
                creep.GetComponent<EnemyActive>().spawner = spawner;
                creep.GetComponent<EnemyActive>().slotNum = slotNum;
                generator = spawner.GetComponent<GenerateActive>();
                generator.Creeps[slotNum] = Instantiate(creep, transform.position, Quaternion.identity, spawner.transform);

                Instantiate(gm.Power_Green, transform.position, Quaternion.identity);
                if (UnityEngine.Random.Range(1, 10) <= 5)
                {
                    Instantiate(gm.Elixir, transform.position, Quaternion.identity);
                }
                gm.gamePoint += 5;
                break;
            case EnemySet.Warrior:
                creep = gm.Zombie_Normal;
                creep.GetComponent<EnemyActive>().spawner = spawner;
                creep.GetComponent<EnemyActive>().slotNum = slotNum;
                generator = spawner.GetComponent<GenerateActive>();
                generator.Creeps[slotNum] = Instantiate(creep, transform.position, Quaternion.identity, spawner.transform);

                Instantiate(gm.Power_Red, transform.position, Quaternion.identity);
                if (UnityEngine.Random.Range(1, 10) <= 5)
                {
                    Instantiate(gm.Power_Green, transform.position, Quaternion.identity);
                    Instantiate(gm.Power_Red, transform.position, Quaternion.identity);
                }
                else if (UnityEngine.Random.Range(1, 20) <= 5)
                {
                    Instantiate(gm.Scroll, transform.position, Quaternion.identity);

                }
                else
                {
                    Instantiate(gm.Elixir, transform.position, Quaternion.identity);
                }
                gm.gamePoint += 30;
                break;
            case EnemySet.Witch:
                creep = gm.Zombie_Wounded;
                creep.GetComponent<EnemyActive>().spawner = spawner;
                creep.GetComponent<EnemyActive>().slotNum = slotNum;
                generator = spawner.GetComponent<GenerateActive>();
                generator.Creeps[slotNum] = Instantiate(creep, transform.position, Quaternion.identity, spawner.transform);

                Instantiate(gm.Power_Blue, transform.position, Quaternion.identity);
                if (UnityEngine.Random.Range(1, 8) <= 3)
                {
                    Instantiate(gm.Scroll, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(gm.Elixir, transform.position, Quaternion.identity);
                }
                gm.gamePoint += 15;
                break;
            case EnemySet.Skeleton:
                var vfx_fire2 = Instantiate(gm.Fire_2, transform.position, Quaternion.identity);
                Destroy(vfx_fire2, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + 1f);
                gm.gamePoint += 5;
                break;
        }
    }

    private void ChangeDirection(Vector2 direction, bool isForward)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            var flag = isForward ? direction.x > 0 : direction.x < 0;
            SetAnimParameter(flag ? Vector2.right : Vector2.left);
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            var flag = isForward ? direction.y > 0 : direction.y < 0;
            SetAnimParameter(flag ? Vector2.up : Vector2.down);
        }
    }

    private Vector3 RandomPosition(Vector3 basepos)
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);
        var rand = basepos + new Vector3(x, y);
        return rand;
    }

    private void SetAnimParameter(Vector2 vector)
    {
        enemyAnim.SetFloat("AxisX", vector.x);
        enemyAnim.SetFloat("AxisY", vector.y);
    }


    private void Update()
    {
        Enemy_Death();


    }
    private void FixedUpdate()
    {
        if (isAlive)
        {
            Target = GameObject.Find("Player").transform;
            var distance = Vector3.Distance(transform.position, Target.position);
            if (distance < 1f && isForward)
            {
                Enemy_Stance(Enemy_Attack);
            }
            else
            {
                if (isForward)
                {
                    Enemy_Movement(true);

                }
                else
                {
                    if (distance < 6f)
                    {
                        Enemy_Stance(Enemy_Attack);
                    }
                    if (distance < 5f)
                    {
                        Enemy_Movement(false);
                    }
                    else if (distance > 6f)
                    {
                        Enemy_Movement(true);
                    }
                }
            }



        }

        if (attackTime > 0f)
        {
            attackTime -= Time.deltaTime;
            State = UnitState.Attack;
        }
        if (attackTime < 0f)
        {
            attackTime = 0f;
            State = UnitState.Idle;
        }

    }

}
