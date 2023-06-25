using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    private Player _player;
    //���ݷ�
    public int damage;
    //���ݹ���
    [SerializeField] private BoxCollider2D _attackArea;

    private void Awake()
    {
        _attackArea = GetComponent<BoxCollider2D>();
        _player = GetComponentInParent<Player>();
    }
    //���� on/off
    public IEnumerator AttackAreaOnOff(float time) 
    {
        _attackArea.enabled = true;
        yield return new WaitForSeconds(time); //�ݶ��̴� ���� �ð�
        _attackArea.enabled = false;

    }


    //������ ó��
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            if (!_player.isUpAttackState)  //��ȭ���� ���°� �ƴ� ��
            { 
                collider.GetComponent<Monster>().Hit(damage, 0);
                _player.AttackUpGauge(); //��ȭ ������ ����
            }
            else
            {
                collider.GetComponent<Monster>().Hit(damage * 2, 1);
            }
        }
    }

}
