using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rigid; //물리

    //점프
    private const float jumpPower = 45; //점프 힘
    private bool isJump = false; //점프 가능한지 체크
    //쉴드
    public float maxShieldCoolTime = 5;
    public float curShieldCoolTime = 0;
    public bool isShield = false; //쉴드 상태로 갈 수 있는지
    public bool isShieldState = false; //현재 쉴드 상태인지
    
    //공격
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Initialized()
    {
        isShield = true; //첫 시작 시 쉴드가 가능한 상태로 시작
    }
    public void Jump()
    {
        if (!isJump) //점프 가능한 상태일 때 점프 기능
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            isJump = true;
        }
    }
    public void Attack()
    {
        Debug.Log("Attack");
    }
    public void Shield()
    {
        if (isShield) //쉴드가 가능한 상태일때 
        {
            isShield = false;
            isShieldState = true;
            Debug.Log("Shield");
        }
        
      
    }
    private void ShieldTimeCheck() //쉴드시간체크
    {
        if (!isShield) 
        {
            curShieldCoolTime += Time.deltaTime;
            if (curShieldCoolTime >= maxShieldCoolTime)
            {
                isShield = true;
                curShieldCoolTime = 0;
            }
        }
    }
    private void Update()
    {
        ShieldTimeCheck();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //바닥과 닿았는지 체크 후 점프 가능한 상태로 만들어줌
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }
}
