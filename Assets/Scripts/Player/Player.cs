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
    private bool isJumpUp = false; //��ȭ ���� �� �� �ִ� ��������
    public float curJumpUpGauge; //���� ��ȭ ���� ������
    public float maxJumpUpGauge; //�ִ� ��ȭ ���� ������
    //����
    [Header("����")]
    public float maxShieldCoolTime = 3; //�ִ� ���� ��Ÿ��
    public float curShieldCoolTime = 0; //���� ���� ��Ÿ��
    public bool isShield = false; //���� ���·� �� �� �ִ���
    public bool isShieldState = false; //���� ���� ��������
    public int shieldCount; //���� ����
    //����
    [Header("����")]
    [SerializeField] private Weapon weapon; //���ݹ���
    private float curAttackUpCoolTime; //���� ��ȭ���� ��Ÿ��
    private float maxAttackUpCoolTime; //�ִ� ��ȭ���� ��Ÿ��
    public float curAttackUpGauge; //���� ��ȭ ���� ������
    public float maxAttackUpGauge; //�ִ� ��ȭ ���� ������
    //��ȭ ���� ����
    private bool isUpAttack; //��ȭ ���·� �� �� �ִ���
    public bool isUpAttackState; //��ȭ��������

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapon>();
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
        curAttackUpGauge = 0f;
        maxAttackUpGauge = 8f;
        curAttackUpCoolTime = 0f;
        maxAttackUpCoolTime = 2f;
        curJumpUpGauge = 0;
        maxJumpUpGauge = 5;
    }

    //������ư
    public void Jump()
    {
        if (!isJump) //���� ������ ������ �� ���� ���
        {
            if (isJumpUp)
            {
                GameManager.instance.jumpUpPs.Stop();
                curJumpUpGauge = 0;
                rigid.AddForce(Vector2.up * (jumpPower*2), ForceMode2D.Impulse);
                isJumpUp = false;
                JumpAnim();
            }
            else
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                JumpAnim();
                curJumpUpGauge++;
                JumpUpCheck();
            }
        }
    }
    //���� ���� �Լ� ��Ȱ��
    private void JumpAnim()
    {
        anim.SetTrigger("DoJump");
        SoundManager.instance.SfxPlaySound(2, 0.5f);
        isJump = true;
    }
    //��ȭ ���� ����
    private void JumpUpCheck()
    {
        if(curJumpUpGauge >= maxJumpUpGauge)
        {
            isJumpUp = true;
            GameManager.instance.jumpUpPs.Play();
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
            StartCoroutine(weapon.AttackAreaOnOff(0.08f));
        }
        else //��ȭ ���� ���� �϶�
        {
            isUpAttackState = true;
            GameManager.instance.attackUpGaugePs.Stop();
            GameManager.instance.attackUpPs.Play();
            SoundManager.instance.SfxPlaySound(5);
            rigid.AddForce(Vector2.up * (jumpPower*2), ForceMode2D.Impulse);
            anim.SetTrigger("DoJump");
            anim.SetBool("IsAttackUp", true);
            isJump = true;
            StartCoroutine(weapon.AttackAreaOnOff(3f));
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
            shieldCount = 1;
            anim.SetTrigger("DoShield");
            GameManager.instance.shieldPs.Play();
            SoundManager.instance.SfxPlaySound(6);
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
                isShieldState = false; //�ð� ������ ������¿��� ��������
                GameManager.instance.shieldPs.Stop();
                curShieldCoolTime = 0;
                shieldCount = 0;
            }
        }
    }
    ///��ȭ ���� �ð� üũ
    private void AttackUpTimeCheck()
    {
        if (isUpAttackState)
        {
            curAttackUpCoolTime += Time.deltaTime;
            if(curAttackUpCoolTime >= maxAttackUpCoolTime)
            {
                //��ȭ ���� ����
                anim.SetBool("IsAttackUp", false);
                isUpAttackState = false;
                GameManager.instance.attackUpPs.Stop();
                curAttackUpCoolTime = 0;
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
        AttackUpTimeCheck();
    }
    
    public void Hit(int damage)
    {
        GameManager.instance.healthUpPs.transform.position = GameManager.instance.playerHealth[curHealth - 1].transform.position;
        GameManager.instance.healthUpPs.Play();
        GameManager.instance.playerHealth[curHealth-1].SetActive(false); //ü���̹��� ���ֱ�
        curHealth -= damage; //�������� ���� ü�� ����
        Debug.Log(curHealth);
        if (curHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            GameManager.instance.GameOver();
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ٴڰ� ��Ҵ��� üũ �� ���� ������ ���·� �������
        if (collision.gameObject.CompareTag("Floor"))
        {
            //���� �ƴ� ���·�
            anim.SetTrigger("DoFall");
            isJump = false;
            GameManager.instance.floorPs.Play(); //���� ����Ʈ
            SoundManager.instance.SfxPlaySound(2); //���� �Ҹ�
        }
    }
}
