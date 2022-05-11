using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageActive : MonoBehaviour
{
    public static DamageActive PopupDamage(GameObject popup, Vector3 position, float amount, DamageState state)
    {
        var damage = Instantiate(popup, position, Quaternion.identity);
        var damaageActive = damage.GetComponent<DamageActive>();
        damaageActive.Setup((int)amount, state);
        return damaageActive;

    }
  public void Setup(int damage, DamageState state)
    {
        timer = 1f;
        switch (state)
        {
            case DamageState.PlayerPhs:
                textColor = Color.red;
                moveVector = new Vector3(Random.Range(0.5f, 1.5f), 1) * Random.Range(4f, 6f);
                break;
            case DamageState.PlayerMag:
                textColor = Color.blue;
                moveVector = new Vector3(Random.Range(0.5f, 1.5f), 1) * Random.Range(4f, 6f);
                break;
            case DamageState.EnemyPhs:
                textColor = Color.yellow;
                moveVector = new Vector3(Random.Range(-0.5f, -1.5f), 1) * Random.Range(4f, 6f);
                break;
            case DamageState.AllyHeal:
                textColor = Color.green;
                moveVector = new Vector3(Random.Range(-0.5f, -1.5f), 1) * Random.Range(4f, 6f);
                break;

        }
        txtPopup.SetText(damage.ToString());
        txtPopup.color = textColor;
    }

    private TextMeshPro txtPopup;
    private float timer;
    private Vector3 moveVector;
    private Color textColor;

    private void Awake()
    {
        txtPopup = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        var speed = 4f;
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * speed * Time.deltaTime;
        if (timer > 0.5f)
        {
            transform.localScale += Vector3.one * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * Time.deltaTime;
        }

        timer -= Time.deltaTime;
        if(timer < 0)
        {
            textColor.a -= speed * Time.deltaTime;
            txtPopup.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
