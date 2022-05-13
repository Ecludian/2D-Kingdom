using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitHandler : MonoBehaviour
{
    public float attackPoint;
    public bool HitArea;

  

     private void OnTriggerEnter2D(Collider2D collision)
    {
        var position = collision.transform.position;
        var gm = GameManager.Instance;
       
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            Debug.Log("HIT");
            var player = collision.GetComponent<PlayerActive>();
            if (!player.isGuard)
            {
                player.healthPoint.CurrentPoint -= attackPoint;
                DamageActive.PopupDamage(gm.Damage, position, attackPoint, DamageState.EnemyPhs);
            }
            else
            {
                DamageActive.PopupDamage(gm.Damage, position, 0, DamageState.PlayerPhs);
            }

        }
 
    }
}
