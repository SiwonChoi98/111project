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
    public IEnumerator AttackAreaOnOff(int count) 
    {
        for(int i=0; i<count; i++)
        {
            attackArea.enabled = true;
            yield return new WaitForSeconds(0.08f); //�ݶ��̴� ���� �ð�
            attackArea.enabled = false;
        }
    }

   
    //������ ó��
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            if (!player.isUpAttackState)  //��ȭ���� ���°� �ƴ� ��
            { 
                collider.GetComponent<Monster>().Hit(damage, 0);
                player.AttackUpGauge(); //��ȭ ������ ����
            }
            else
            {
                collider.GetComponent<Monster>().Hit(damage * 5, 1);
            }
        }
    }

}
