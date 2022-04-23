using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandles : MonoBehaviour
{
    public float attackPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            Debug.Log("HIT");
            var player = collision.GetComponent<PlayerActive>();
            player.healthPoint -= attackPoint;
        }
    }
}
