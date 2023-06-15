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
    public IEnumerator AttackAreaOnOff(float time) 
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(time); //�ݶ��̴� ���� �ð�
        attackArea.enabled = false;

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
                collider.GetComponent<Monster>().Hit(damage * 2, 1);
            }
        }
    }

}
