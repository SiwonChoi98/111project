using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rigid; //����
    private Animator anim; //�ִϸ��̼�
    //����
    private const float jumpPower = 60; //���� ��
    private bool isJump = false; //���� �������� üũ
    //����
    public float maxShieldCoolTime = 3;
    public float curShieldCoolTime = 0;
    public bool isShield = false; //���� ���·� �� �� �ִ���
    public bool isShieldState = false; //���� ���� ��������
    
    //����
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            anim.SetTrigger("DoJump");
            isJump = true;
        }
    }
    public void Attack()
    {
        Debug.Log("Attack");
        int ran = Random.Range(0,3);
        switch (ran)
        {
            case 0:
                anim.SetTrigger("DoAttack1");
                break;
            case 1:
                anim.SetTrigger("DoAttack2");
                break;
            case 2:
                anim.SetTrigger("DoAttack3");
                break;
            default:
                return;
        }
    }
    public void Shield()
    {
        if (isShield) //���尡 ������ �����϶� 
        {
            isShield = false;
            isShieldState = true;
            anim.SetTrigger("DoShield");
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
                isShieldState = false;
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
            anim.SetTrigger("DoFall");
            isJump = false;
        }
    }
}
