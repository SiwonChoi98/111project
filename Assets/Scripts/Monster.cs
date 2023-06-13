using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int curHealth; //체력
    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();


        if (collision.gameObject.CompareTag("Player") && !player.isJump) //플레이어가 바닥에 있을 때 닿으면 체력 깎이고 제거
        {
            player.curHealth--;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Floor")) //바닥에 닿았을 때 제거
        {
            Destroy(gameObject);
        }
    }
}
