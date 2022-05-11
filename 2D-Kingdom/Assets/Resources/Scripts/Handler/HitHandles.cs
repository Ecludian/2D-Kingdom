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
        var position = collision.transform.position;
        var gm = GameManager.Instance;
        if (collision.CompareTag("Enemy") && collision.isTrigger)
        {
            var enemy = collision.GetComponent<EnemyActive>();
            if (HitArea)
            {

                enemy.healthPoint -= attackPoint;
                DamageActive.PopupDamage(gm.Damage, position, attackPoint, DamageState.PlayerMag);
            }
            else
            {
                if (count == 0)
                {
                    enemy.healthPoint -= attackPoint;
                    var player = transform.parent.parent.GetComponent<PlayerActive>();
                    player.staminaPoint.CurrentProp += 10;
                    DamageActive.PopupDamage(gm.Damage, position, attackPoint, DamageState.PlayerPhs);
                }
                count++;
            }
        }
    }

   private void OnDisable()
    {
        count = 0;
    }
}
