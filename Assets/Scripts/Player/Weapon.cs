using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    private Player player;
    //공격력
    public int damage;
    //공격범위
    [SerializeField] private BoxCollider2D attackArea;

    private void Awake()
    {
        attackArea = GetComponent<BoxCollider2D>();
        player = GetComponentInParent<Player>();
    }
    //공격 on/off
    public IEnumerator AttackAreaOnOff(float time) 
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(time); //콜라이더 지속 시간
        attackArea.enabled = false;

    }


    //데미지 처리
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            if (!player.isUpAttackState)  //강화공격 상태가 아닐 때
            { 
                collider.GetComponent<Monster>().Hit(damage, 0);
                player.AttackUpGauge(); //강화 게이지 증가
            }
            else
            {
                collider.GetComponent<Monster>().Hit(damage * 2, 1);
            }
        }
    }

}
