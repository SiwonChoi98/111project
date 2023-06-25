using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    private Player _player;
    //공격력
    public int damage;
    //공격범위
    [SerializeField] private BoxCollider2D _attackArea;

    private void Awake()
    {
        _attackArea = GetComponent<BoxCollider2D>();
        _player = GetComponentInParent<Player>();
    }
    //공격 on/off
    public IEnumerator AttackAreaOnOff(float time) 
    {
        _attackArea.enabled = true;
        yield return new WaitForSeconds(time); //콜라이더 지속 시간
        _attackArea.enabled = false;

    }


    //데미지 처리
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            if (!_player.isUpAttackState)  //강화공격 상태가 아닐 때
            { 
                collider.GetComponent<Monster>().Hit(damage, 0);
                _player.AttackUpGauge(); //강화 게이지 증가
            }
            else
            {
                collider.GetComponent<Monster>().Hit(damage * 2, 1);
            }
        }
    }

}
