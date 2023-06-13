using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rigid; //����
    private Animator anim; //�ִϸ��̼�
    
    //ü��
    public int curHealth; //����ü��
    //����
    private const float jumpPower = 60; //���� ��
    public bool isJump = false; //���� �������� üũ
    //����
    public float maxShieldCoolTime = 3;
    public float curShieldCoolTime = 0;
    public bool isShield = false; //���� ���·� �� �� �ִ���
    public bool isShieldState = false; //���� ���� ��������
    //����
    public Weapon weapon; //���ݹ���
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Initialized();
    }
    //�ʱ�ȭ
    public void Initialized()
    {
        isShield = true; //ù ���� �� ���尡 ������ ���·� ����
        curHealth = 3;
        weapon.damage = 1;
    }

    //������ư
    public void Jump()
    {
        if (!isJump) //���� ������ ������ �� ���� ���
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("DoJump");
            isJump = true;
        }
    }
    //���ݹ�ư
    public void Attack() 
    {
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
        StartCoroutine(weapon.AttackAreaOnOff());
        SoundManager.instance.SfxPlaySound(1);
    }
    
    //�����ư
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

    public void Hit(int damage)
    {
        GameManager.instance.playerHealth[curHealth-1].SetActive(false); //ü���̹��� ���ֱ�
        curHealth -= damage; //�������� ���� ü�� ����
        Debug.Log(curHealth);
        if (curHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            GameManager.instance.GameOver();
            Debug.Log("ĳ���� ���");
        }
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
