using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyActive : MonoBehaviour
{
    public Transform Target;
    public UnitState State;
    public EnemySet Creep;
    public bool isAlive;
    public bool isForward;
    public float moveSpeed;
    public float healthPoint;
    public float atkSpeed;
   

    private Animator enemyAnim;
    private AudioSource enemyAudio;
    private float attackTime;

    private void Enemy_Attack()
    {
        var player = Target.GetComponent<PlayerActive>();
        var gm = GameManager.Instance;
        if(attackTime == 0 && player.isAlive)
        {
            enemyAnim.SetTrigger("Attack");
            if(Creep == EnemySet.Witch)
            {
                var location = RandomPosition(transform.position);
                var vfx = Instantiate(gm.Origin_Smoke, location, Quaternion.identity);
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
        var moveToward = Vector3.MoveTowards(transform.position, Target.position, moveSpeed * Time.deltaTime);

        var towardPos = moveToward - transform.position;
        ChangeDirection(towardPos, isForward); //try without trransform position
        if (isForward)
        {
            transform.position += towardPos;
        }
        else
        {
            transform.position -= towardPos;
        }
        enemyAnim.SetBool("isMoving", true);
        State = UnitState.Move;
    }

    private void Enemy_Death()
    {
        if(healthPoint <= 0)
        {
            isAlive = false;
            healthPoint = 0;
            State = UnitState.Dead;
            enemyAnim.SetTrigger("Dead");
            if(Creep == EnemySet.Skeleton)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject, 3f);
            }
       
        }
    }

    private void ChangeDirection(Vector2 direction, bool isForward)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
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

    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        Target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            var distance = Vector3.Distance(transform.position, Target.position);
            if (distance < 1f && isForward) {
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
                    if(distance < 6f)
                    {
                        Enemy_Stance(Enemy_Attack);
                    }
                    if(distance < 5f)
                    {
                        Enemy_Movement(false);
                    }else if(distance > 6f)
                    {
                        Enemy_Movement(true);
                    }
                }
            }
        }

        if(attackTime > 0f)
        {
            attackTime -= Time.deltaTime;
            State = UnitState.Attack;
        }
        if(attackTime < 0f)
        {
            attackTime = 0f;
            State = UnitState.Idle;
        }
        Enemy_Death();
    }
}