using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandles : MonoBehaviour
{
    public float attackPoint;
    public bool HitArea;


    private int count;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            var enemy = collision.GetComponent<EnemyActive>();
            if (HitArea)
            {
                enemy.healthPoint -= attackPoint;
            }
            else
            {
                if(count == 0)
                {
                    enemy.healthPoint -= attackPoint;
                    var player = transform.parent.parent.GetComponent<PlayerActive>();
                    player.staminaPoint.CurrentPoint += 10;
                }
                count++;
            }
        }
       else if (collision.CompareTag("Player") && collision.isTrigger)
        {
            Debug.Log("HIT");
            var player = collision.GetComponent<PlayerActive>();
            if (!player.isGuard)
            {
                player.healthPoint.CurrentPoint -= attackPoint;
            }

        }
    }

   private void OnDisable()
    {
        count = 0;
    }
}
