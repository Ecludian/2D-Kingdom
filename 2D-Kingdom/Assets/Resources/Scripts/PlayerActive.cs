using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActive : MonoBehaviour
{
    public UnitState State;
    public bool isAlive;
    public float moveSpeed;

    private Animator playerAnim;
    private AudioSource playerAudio;

    private void Player_Movement(Vector2 moving)
    {
        moving.Normalize();
        playerAnim.SetFloat("AxisX", moving.x);
        playerAnim.SetFloat("AxisY", moving.y);
        if (moving.x != 0 || moving.y != 0)
        {
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

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isAlive)
        {
            var moveaway = Vector2.zero;
            moveaway.x = Input.GetAxis("Horizontal");
            moveaway.y = Input.GetAxis("Vertical");
            Player_Movement(moveaway);
        }
    }

}

public enum UnitState
{
    Idle, Move, Attack, Cast, Dead
}
