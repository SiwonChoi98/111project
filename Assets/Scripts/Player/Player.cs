using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    
    private Rigidbody2D _rigid; //����
    private Animator _anim; //�ִϸ��̼�

    //ü��
    [Header("ü��")]
    public int curHealth; //����ü��
    //����
    [Header("����")]
    private const float _jumpPower = 60; //���� ��
    public bool isJump = false; //���� �������� üũ
    private bool _isJumpUp = false; //��ȭ ���� �� �� �ִ� ��������
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
    [SerializeField] private Weapon _weapon; //���ݹ���
    private float _curAttackUpCoolTime; //���� ��ȭ���� ��Ÿ��
    private float _maxAttackUpCoolTime; //�ִ� ��ȭ���� ��Ÿ��
    public float curAttackUpGauge; //���� ��ȭ ���� ������
    public float maxAttackUpGauge; //�ִ� ��ȭ ���� ������
    //��ȭ ���� ����
    private bool _isUpAttack; //��ȭ ���·� �� �� �ִ���
    public bool isUpAttackState; //��ȭ��������

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _weapon = GetComponentInChildren<Weapon>();
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
        _weapon.damage = 1;
        curAttackUpGauge = 0f;
        maxAttackUpGauge = 8f;
        _curAttackUpCoolTime = 0f;
        _maxAttackUpCoolTime = 2f;
        curJumpUpGauge = 0;
        maxJumpUpGauge = 5;
    }

    //������ư
    public void Jump()
    {
        if (!isJump) //���� ������ ������ �� ���� ���
        {
            if (_isJumpUp)
            {
                GameManager.instance.jumpUpPs.Stop();
                curJumpUpGauge = 0;
                _rigid.AddForce(Vector2.up * (_jumpPower*2), ForceMode2D.Impulse);
                _isJumpUp = false;
                JumpAnim();
            }
            else
            {
                _rigid.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
                JumpAnim();
                curJumpUpGauge++;
                JumpUpCheck();
            }
        }
    }
    //���� ���� �Լ� ��Ȱ��
    private void JumpAnim()
    {
        _anim.SetTrigger("DoJump");
        SoundManager.instance.SfxPlaySound(2, 0.5f);
        isJump = true;
    }
    //��ȭ ���� ����
    private void JumpUpCheck()
    {
        if(curJumpUpGauge >= maxJumpUpGauge)
        {
            _isJumpUp = true;
            GameManager.instance.jumpUpPs.Play();
        }
    }
    //���ݹ�ư
    public void Attack() 
    {
        SoundManager.instance.SfxPlaySound(1);
        if (!_isUpAttack) //��ȭ ���� ���°� �ƴҶ�
        {
            int ran = Random.Range(0,2);
            switch (ran)
            {
                case 0:
                    _anim.SetTrigger("DoAttack1");
                    break;
                case 1:
                    _anim.SetTrigger("DoAttack2");
                    break;
                default:
                    return;
            }
            StartCoroutine(_weapon.AttackAreaOnOff(0.08f));
        }
        else //��ȭ ���� ���� �϶�
        {
            isUpAttackState = true;
            GameManager.instance.attackUpGaugePs.Stop();
            GameManager.instance.attackUpPs.Play();
            SoundManager.instance.SfxPlaySound(5);
            _rigid.AddForce(Vector2.up * (_jumpPower*2), ForceMode2D.Impulse);
            _anim.SetTrigger("DoJump");
            _anim.SetBool("IsAttackUp", true);
            isJump = true;
            StartCoroutine(_weapon.AttackAreaOnOff(3f));
            _isUpAttack = false; //��ȭ ���� ����
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
            _anim.SetTrigger("DoShield");
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
            _curAttackUpCoolTime += Time.deltaTime;
            if(_curAttackUpCoolTime >= _maxAttackUpCoolTime)
            {
                //��ȭ ���� ����
                _anim.SetBool("IsAttackUp", false);
                isUpAttackState = false;
                GameManager.instance.attackUpPs.Stop();
                _curAttackUpCoolTime = 0;
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
            _isUpAttack = true; //�ִ�ġ ���� �����ϸ� ��ȭ ���� �Ǵ� ���� �����
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
            _rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            GameManager.instance.GameOver();
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�ٴڰ� ��Ҵ��� üũ �� ���� ������ ���·� �������
        if (collision.gameObject.CompareTag("Floor"))
        {
            //���� �ƴ� ���·�
            _anim.SetTrigger("DoFall");
            isJump = false;
            GameManager.instance.floorPs.Play(); //���� ����Ʈ
            SoundManager.instance.SfxPlaySound(2); //���� �Ҹ�
        }
    }
}
