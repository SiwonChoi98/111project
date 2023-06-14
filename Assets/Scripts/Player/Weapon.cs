using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    private Player player;
    //���ݷ�
    public int damage;
    //���ݹ���
    [SerializeField] private BoxCollider2D attackArea;

    private void Awake()
    {
        attackArea = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<Player>();
    }
    //���� on/off
    public IEnumerator AttackAreaOnOff() 
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.1f); //�ݶ��̴� ���� �ð�
        attackArea.enabled = false;
    }

   
    //������ ó��
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            player.AttackUpGauge(); //��ȭ ������ ����

            if (!player.isUpAttackState) //��ȭ���� ���°� �ƴ� ��
                collider.GetComponent<Monster>().Hit(damage);
            else
            {
                collider.GetComponent<Monster>().Hit(damage * 5);
                player.isUpAttackState = false; //��ȭ ���� ����
                player.isUpAttack = false; //��ȭ ���� ����
            }
        }
    }

}
