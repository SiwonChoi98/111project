using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rigid; //����
    private Animator anim; //�ִϸ��̼�

    //ü��
    [Header("ü��")]
    public int curHealth; //����ü��
    //����
    [Header("����")]
    private const float jumpPower = 60; //���� ��
    public bool isJump = false; //���� �������� üũ
    //����
    [Header("����")]
    public float maxShieldCoolTime = 3; //�ִ� ���� ��Ÿ��
    public float curShieldCoolTime = 0; //���� ���� ��Ÿ��
    public bool isShield = false; //���� ���·� �� �� �ִ���
    public bool isShieldState = false; //���� ���� ��������
    //����
    [Header("����")]
    public Weapon weapon; //���ݹ���
    public float curAttackUpGauge; //���� ��ȭ ���� ������
    public float maxAttackUpGauge; //�ִ� ��ȭ ���� ������
    //��ȭ ���� ����
    public bool isUpAttack; //��ȭ ���·� �� �� �ִ���
    public bool isUpAttackState; //��ȭ��������

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
        isShield = true;
        curHealth = 3;
        weapon.damage = 1;
        curAttackUpGauge = 0;
        maxAttackUpGauge = 5;
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
        SoundManager.instance.SfxPlaySound(1);
        if (!isUpAttack) //��ȭ ���� ���°� �ƴҶ�
        {
            int ran = Random.Range(0,2);
            switch (ran)
            {
                case 0:
                    anim.SetTrigger("DoAttack1");
                    break;
                case 1:
                    anim.SetTrigger("DoAttack2");
                    break;
                default:
                    return;
            }
            StartCoroutine(weapon.AttackAreaOnOff(1));
        }
        else //��ȭ ���� ���� �϶�
        {
            isUpAttackState = true;
            GameManager.instance.attackUpGaugePs.Stop();

            rigid.AddForce(Vector2.up * (jumpPower*2), ForceMode2D.Impulse);
            anim.SetTrigger("DoJump");
            anim.SetBool("IsAttackUp", true);
            isJump = true;
            StartCoroutine(weapon.AttackAreaOnOff(30));
            isUpAttack = false; //��ȭ ���� ����
        }
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
    //����ð�üũ
    private void ShieldTimeCheck() 
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

    //��ȭ ���� ������ ����
    public void AttackUpGauge()
    {
        curAttackUpGauge++;
        if (curAttackUpGauge >= maxAttackUpGauge)
        {
            curAttackUpGauge = 0;
            isUpAttack = true; //�ִ�ġ ���� �����ϸ� ��ȭ ���� �Ǵ� ���� �����
            GameManager.instance.attackUpGaugePs.Play();
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

            anim.SetBool("IsAttackUp", false);
            isUpAttackState = false; //��ȭ ���� ����
        }
    }
}
