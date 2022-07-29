using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class OzAi : MonoBehaviour
{
    public Transform target;
    public Transform targetEnemy;
    public float shootingDistance;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;

    private float timeBtwShots;
    public float startTimeBtwshots;

    public GameObject projectile;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    bool reachedEndOfPath = false;

    public Transform OzGFX;

    Seeker seeker;
    Rigidbody2D rb;

    Path path;
    int currentWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        timeBtwShots = startTimeBtwshots;
        dashTime = startDashTime;


        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetEnemy)
        {
            SearchEnemy();
            
        }

        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(rb.velocity.x >= 0.01f)
        {
            OzGFX.localScale = new Vector3(1f, 1f, 1f);
        }else if(rb.velocity.x <= -0.01f)
        {
            OzGFX.localScale = new Vector3(-1f, 1f, 1f);
        }

        if(Vector2.Distance(transform.position, targetEnemy.position) < shootingDistance && timeBtwShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwshots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
            
        }

        /*if (dashTime <= 0)
        {
            dashTime = startDashTime;
            rb.velocity = Vector2.zero;
        }
        else
        {
            OzDash();
         
        }
*/
    }
   /* public void OzDash()
    {
        dashTime -= Time.deltaTime;
        rb.velocity = Vector2.left * dashSpeed; 
        //rb.velocity = new Vector2(targetEnemy.position.x, targetEnemy.position.y) * dashSpeed;
    }*/

    void SearchEnemy()
    {
        targetEnemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }


}
