using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UnitPoint
{
    public float CurrentPoint;
    public float MaximumPoint;
}
public class PlayerActive : MonoBehaviour
{
    public UnitState State;
    public bool isAlive;
    public bool isGuard;

    public float[] cooldownSkills;

    public float moveSpeed;
    public UnitPoint healthPoint;
    public UnitPoint magicPoint;
    public UnitPoint staminaPoint;
    public float atkSpeed;
   // public Transform attackPoint;
    //public float attackRange = 0.5f;
    public LayerMask enemyLayer;


    public float[] castTime { get; set; }


    private Animator playerAnim;
    private AudioSource playerAudio;
    private AudioSource attackAudio;
    private AudioSource skill_1Audio;
    private AudioSource skill_2Audio;
    private AudioSource skill_2_ShoutAudio;
    private Vector2 direcPos;
    private float attackTime;


    private void Player_Attack()
    {
        playerAnim.SetTrigger("Attack");
        State = UnitState.Attack;

        if (attackTime == 0 && State == UnitState.Idle)
        {
            attackAudio.Play();
            State = UnitState.Attack;
            playerAnim.SetTrigger("Attack");
            
            //Cooldown 
            attackTime = atkSpeed;
        }

        //NEW ATTACK SYSTEM!!
        /*State = UnitState.Attack;
        playerAnim.SetTrigger("Attack");
        playerAudio.Play();
        Collider2D[] hitEnemeis = Physics2D.OverlapCircleAll(direcPos, attackRange, enemyLayer);


        foreach(Collider2D enemy in hitEnemeis)
        {
            Debug.Log("We Hit " + enemy.name);
        }*/
    }

    //for displaying attack pioint
   /* void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/

   private IEnumerator Player_Attack(SkillSet skill, Action action = null)
    {
        if(castTime[(int)skill - 1] == 0 && State == UnitState.Idle)
        {
            State = UnitState.Cast;
            playerAnim.SetTrigger(skill.ToString());
            castTime[(int)skill - 1] = cooldownSkills[(int)skill - 1];

            switch (skill)
            {
                case SkillSet.ArchSword:
                    skill_1Audio.Play();
                    staminaPoint.CurrentPoint -= 20;
                    break;
                case SkillSet.DominusLapidis:
                    skill_2Audio.Play();
                    magicPoint.CurrentPoint -= 50;
                    break;

            }
            yield return new WaitForSeconds(1f);
            action?.Invoke();
        }
    }


    private void Player_Movement(Vector2 moving)
    {
        moving.Normalize();
        playerAnim.SetFloat("AxisX", direcPos.x);
        playerAnim.SetFloat("AxisY", direcPos.y);
        if (moving.x != 0 || moving.y != 0)
        {
            direcPos = moving;
            State = UnitState.Move;
            playerAnim.SetBool("isMoving", true);

            var xAndy = Mathf.Sqrt(Mathf.Pow(moving.x, 2) + Mathf.Pow(moving.y, 2));
            var pos_x = moving.x * moveSpeed * Time.deltaTime / xAndy;
            var pos_y = moving.y * moveSpeed * Time.deltaTime / xAndy;
            var pos_z = transform.position.z;
            transform.Translate(pos_x, pos_y, pos_z, Space.Self);

            if (!playerAudio.isPlaying)
            {
                playerAudio.Play();
            }
        }
        else
        {
            State = UnitState.Idle;
            playerAnim.SetBool("isMoving", false);
            playerAudio.Stop();
        }
    }
    private void Player_Death()
    {
        if (healthPoint.CurrentPoint <= 0) 
        { 

            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Player_Movement(Vector2.zero);
            healthPoint.CurrentPoint = 0;
            State = UnitState.Dead;
            playerAnim.SetTrigger("Dead");
            State = UnitState.Dead;
            isAlive = false;


        }
    }

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        attackAudio = transform.Find("Rogue").GetComponent<AudioSource>();
        skill_1Audio = transform.Find("Abilities").Find("ArchSword").GetComponent<AudioSource>();
        skill_2Audio = transform.Find("Abilities").Find("DominusLapidis").GetComponent<AudioSource>();
        skill_2_ShoutAudio = transform.Find("Abilities").Find("DominusLapidis").Find("Shout").GetComponent<AudioSource>();
        castTime = new float[] { 0f, 0f, 0f };

    }

    //run per frames, for input, timer, non-pHysic object
    private void Update()
    {
        var gm = GameManager.Instance;
        if (isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Space))//for controller
            {
                Player_Attack();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (staminaPoint.CurrentPoint >= 20)
                {
                    StartCoroutine(Player_Attack(SkillSet.ArchSword));
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (magicPoint.CurrentPoint >= 50)
                {
                    StartCoroutine(Player_Attack(SkillSet.DominusLapidis, () =>
                    {
                        var shield = Instantiate(gm.Shell_Effect, transform);
                        Destroy(shield, 10f);
                    }));
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                HitHandles hitHandles = GetComponent<HitHandles>();
                if (magicPoint.CurrentPoint < healthPoint.MaximumPoint && castTime[2] == 0)
                {
                    var effect = Instantiate(gm.Magic_Circle, transform);
                    Destroy(effect, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

                    //heal based on 250% of player base attack
                    healthPoint.CurrentPoint += 200;
                    if (healthPoint.CurrentPoint > healthPoint.MaximumPoint)
                    {
                        healthPoint.CurrentPoint = healthPoint.MaximumPoint;
                    }
                    castTime[2] = cooldownSkills[2];
                }
            }
        }
    }

    //For movement or anything that need Physics
    private void FixedUpdate()
    {
        if (isAlive)
        {
          
            var moveaway = Vector2.zero;
            moveaway.x = Input.GetAxis("Horizontal");
            moveaway.y = Input.GetAxis("Vertical");
            Player_Movement(moveaway);

       

            Player_Death();

        }

        for(int i = 0; i < castTime.Length; i++)
        {
            if (castTime[i] > 0f)
            {
                castTime[i] -= Time.fixedDeltaTime;
            }
            if (castTime[i] < 0f)
            {
                castTime[i] = 0f;
                State = UnitState.Idle;
            }
        }

        if (attackTime > 0f)
        {
            attackTime -= Time.fixedDeltaTime;
        }
        if (attackTime < 0f)
        {
            attackTime = 0f;
            State = UnitState.Idle;
        }

        isGuard = transform.Find("Shell_Effect(Clone)");

    }

}

