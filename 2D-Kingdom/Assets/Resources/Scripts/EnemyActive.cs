using System.Collections;
using System.Collections.Generic;
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

    private void Enemy_Stance()
    {
        enemyAnim.SetBool("isMoving", false);
        State = UnitState.Idle;
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

    private void SetAnimParameter(Vector2 vector)
    {
        enemyAnim.SetFloat("AxisX", vector.x);
        enemyAnim.SetFloat("AxisY", vector.y);
    }

    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAlive)
        {
            var distance = Vector3.Distance(transform.position, Target.position);
            if (distance < 1.5f && isForward) {
                Enemy_Stance();
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
                        Enemy_Stance();
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
        Enemy_Death();
    }
}

public enum EnemySet
{
    Deafult, Damaged, Native, Warrior, Witch, Skeleton
}