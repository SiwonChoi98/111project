using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //���ݷ�
    public int damage;
    //���ݹ���
    [SerializeField] private BoxCollider2D attackArea;

    private void Awake()
    {
        attackArea = GetComponent<BoxCollider2D>();
    }
    public IEnumerator AttackAreaOnOff() //���� on/off
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.3f); //�ݶ��̴� ���� �ð�
        attackArea.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            collider.GetComponent<Monster>().Hit(damage);
        }
    }
}
