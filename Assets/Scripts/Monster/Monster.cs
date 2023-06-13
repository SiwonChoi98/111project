using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int curHealth; //체력
    public int damage; //공격력

    private void Start()
    {
        Initialized();
    }
    private void Initialized()
    {
        damage = 1;
    }
    public virtual void Hit(int damage)
    {
        curHealth -= damage;
        if (curHealth <= 0)
        {
            //GetComponent<Collider2D>().enabled = false;
            //SoundManager.instance.PlaySound(1, "CookieBreak1Sound");
            Destroy(gameObject);
            Debug.Log("몬스터 사망");
            GameManager.instance.score += 100;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player") && !player.isJump) //플레이어가 바닥에 있을 때 닿으면 체력 깎이고 제거
        {
            player.Hit(damage);
        }
    }
}
