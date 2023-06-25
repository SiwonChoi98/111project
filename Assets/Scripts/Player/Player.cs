using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    
    private Rigidbody2D _rigid; //물리
    private Animator _anim; //애니메이션

    //체력
    [Header("체력")]
    public int curHealth; //현재체력
    //점프
    [Header("점프")]
    private const float _jumpPower = 60; //점프 힘
    public bool isJump = false; //점프 가능한지 체크
    private bool _isJumpUp = false; //강화 점프 할 수 있는 상태인지
    public float curJumpUpGauge; //현재 강화 점프 게이지
    public float maxJumpUpGauge; //최대 강화 점프 게이지
    //쉴드
    [Header("쉴드")]
    public float maxShieldCoolTime = 3; //최대 쉴드 쿨타임
    public float curShieldCoolTime = 0; //현재 쉴드 쿨타임
    public bool isShield = false; //쉴드 상태로 갈 수 있는지
    public bool isShieldState = false; //현재 쉴드 상태인지
    public int shieldCount; //쉴드 갯수
    //공격
    [Header("공격")]
    [SerializeField] private Weapon _weapon; //공격무기
    private float _curAttackUpCoolTime; //현재 강화상태 쿨타임
    private float _maxAttackUpCoolTime; //최대 강화상태 쿨타임
    public float curAttackUpGauge; //현재 강화 공격 게이지
    public float maxAttackUpGauge; //최대 강화 공격 게이지
    //강화 공격 상태
    private bool _isUpAttack; //강화 상태로 갈 수 있는지
    public bool isUpAttackState; //강화상태인지

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
    //초기화
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

    //점프버튼
    public void Jump()
    {
        if (!isJump) //점프 가능한 상태일 때 점프 기능
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
    //점프 관련 함수 재활용
    private void JumpAnim()
    {
        _anim.SetTrigger("DoJump");
        SoundManager.instance.SfxPlaySound(2, 0.5f);
        isJump = true;
    }
    //강화 점프 조건
    private void JumpUpCheck()
    {
        if(curJumpUpGauge >= maxJumpUpGauge)
        {
            _isJumpUp = true;
            GameManager.instance.jumpUpPs.Play();
        }
    }
    //공격버튼
    public void Attack() 
    {
        SoundManager.instance.SfxPlaySound(1);
        if (!_isUpAttack) //강화 공격 상태가 아닐때
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
        else //강화 공격 상태 일때
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
            _isUpAttack = false; //강화 상태 해제
        }
    }
    
    //쉴드버튼
    public void Shield()
    {
        if (isShield) //쉴드가 가능한 상태일때 
        {
            isShield = false;
            isShieldState = true;
            shieldCount = 1;
            _anim.SetTrigger("DoShield");
            GameManager.instance.shieldPs.Play();
            SoundManager.instance.SfxPlaySound(6);
        }
    }
    //쉴드시간체크
    private void ShieldTimeCheck() 
    {
        if (!isShield) 
        {
            curShieldCoolTime += Time.deltaTime;
            if (curShieldCoolTime >= maxShieldCoolTime)
            {
                isShield = true;
                isShieldState = false; //시간 지나도 쉴드상태에서 빠져나감
                GameManager.instance.shieldPs.Stop();
                curShieldCoolTime = 0;
                shieldCount = 0;
            }
        }
    }
    ///강화 상태 시간 체크
    private void AttackUpTimeCheck()
    {
        if (isUpAttackState)
        {
            _curAttackUpCoolTime += Time.deltaTime;
            if(_curAttackUpCoolTime >= _maxAttackUpCoolTime)
            {
                //강화 상태 해제
                _anim.SetBool("IsAttackUp", false);
                isUpAttackState = false;
                GameManager.instance.attackUpPs.Stop();
                _curAttackUpCoolTime = 0;
            }
        }
    }
    //강화 공격 게이지 증가
    public void AttackUpGauge()
    {
        curAttackUpGauge++;
        if (curAttackUpGauge >= maxAttackUpGauge)
        {
            curAttackUpGauge = 0;
            _isUpAttack = true; //최대치 보다 증가하면 강화 공격 되는 상태 만들기
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
        GameManager.instance.playerHealth[curHealth-1].SetActive(false); //체력이미지 없애기
        curHealth -= damage; //데미지의 따른 체력 감소
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
        //바닥과 닿았는지 체크 후 점프 가능한 상태로 만들어줌
        if (collision.gameObject.CompareTag("Floor"))
        {
            //점프 아닌 상태로
            _anim.SetTrigger("DoFall");
            isJump = false;
            GameManager.instance.floorPs.Play(); //착지 이펙트
            SoundManager.instance.SfxPlaySound(2); //착지 소리
        }
    }
}
