using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rigid; //����

    //����
    private const float jumpPower = 45; //���� ��
    private bool isJump = false; //���� �������� üũ
    //����
    public float maxShieldCoolTime = 5;
    public float curShieldCoolTime = 0;
    public bool isShield = false; //���� ���·� �� �� �ִ���
    public bool isShieldState = false; //���� ���� ��������
    
    //����
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Initialized()
    {
        isShield = true; //ù ���� �� ���尡 ������ ���·� ����
    }
    public void Jump()
    {
        if (!isJump) //���� ������ ������ �� ���� ���
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
        if (isShield) //���尡 ������ �����϶� 
        {
            isShield = false;
            isShieldState = true;
            Debug.Log("Shield");
        }
        
      
    }
    private void ShieldTimeCheck() //����ð�üũ
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
        //�ٴڰ� ��Ҵ��� üũ �� ���� ������ ���·� �������
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }
}
