using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int curHealth; //ü��
    void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();


        if (collision.gameObject.CompareTag("Player") && !player.isJump) //�÷��̾ �ٴڿ� ���� �� ������ ü�� ���̰� ����
        {
            player.curHealth--;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Floor")) //�ٴڿ� ����� �� ����
        {
            Destroy(gameObject);
        }
    }
}
