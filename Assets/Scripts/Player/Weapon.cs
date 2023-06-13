using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //공격력
    public int damage;
    //공격범위
    [SerializeField] private BoxCollider2D attackArea;

    private void Awake()
    {
        attackArea = GetComponent<BoxCollider2D>();
    }
    public IEnumerator AttackAreaOnOff() //공격 on/off
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.3f); //콜라이더 지속 시간
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
