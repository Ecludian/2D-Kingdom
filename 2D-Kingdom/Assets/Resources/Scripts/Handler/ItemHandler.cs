using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public ItemSet Item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var gm = GameManager.Instance;
        if(collision.CompareTag("Player") && collision.isTrigger)
        {
            var player = collision.GetComponent<PlayerActive>();
            switch (Item)
            {
                case ItemSet.Power_Red:
                    player.staminaPoint.CurrentProp += 15;
                    break;
                case ItemSet.Power_Green:
                    player.healthPoint.CurrentProp += 20;
                    DamageActive.PopupDamage(gm.Damage, transform.position, 20, DamageState.AllyHeal);

                    break;
                case ItemSet.Power_Blue:
                    player.magicPoint.CurrentProp += 35;
                    break;
                case ItemSet.Scroll:
                    gm.Item_Scroll++;
                    break;
                case ItemSet.Elixir:
                    gm.Item_Elixir++;
                    break;
            }
            Destroy(gameObject);
        }
    }
}
