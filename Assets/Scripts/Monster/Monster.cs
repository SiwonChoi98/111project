using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int curHealth; //ü��
    public int damage; //���ݷ�

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
            Debug.Log("���� ���");
            GameManager.instance.score += 100;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player") && !player.isJump) //�÷��̾ �ٴڿ� ���� �� ������ ü�� ���̰� ����
        {
            player.Hit(damage);
        }
    }
}
