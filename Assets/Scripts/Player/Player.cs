using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    private Rigidbody2D rigid; //물리
    private Animator anim; //애니메이션

    //체력
    [Header("체력")]
    public int curHealth; //현재체력
    //점프
    [Header("점프")]
    private const float jumpPower = 60; //점프 힘
    public bool isJump = false; //점프 가능한지 체크
    //쉴드
    [Header("쉴드")]
    public float maxShieldCoolTime = 3; //최대 쉴드 쿨타임
    public float curShieldCoolTime = 0; //현재 쉴드 쿨타임
    public bool isShield = false; //쉴드 상태로 갈 수 있는지
    public bool isShieldState = false; //현재 쉴드 상태인지
    //공격
    [Header("공격")]
    public Weapon weapon; //공격무기
    public float curAttackUpGauge; //현재 강화 공격 게이지
    public float maxAttackUpGauge; //최대 강화 공격 게이지
    //강화 공격 상태
    public bool isUpAttack; //강화 상태로 갈 수 있는지
    public bool isUpAttackState; //강화상태인지

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        weapon.damage = 1;
        curAttackUpGauge = 0;
        maxAttackUpGauge = 5;
    }

    //점프버튼
    public void Jump()
    {
        if (!isJump) //점프 가능한 상태일 때 점프 기능
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetTrigger("DoJump");
            isJump = true;
        }
    }
    //공격버튼
    public void Attack() 
    {
        SoundManager.instance.SfxPlaySound(1);
        if (!isUpAttack) //강화 공격 상태가 아닐때
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
        else //강화 공격 상태 일때
        {
            isUpAttackState = true;
            GameManager.instance.attackUpGaugePs.Stop();

            rigid.AddForce(Vector2.up * (jumpPower*2), ForceMode2D.Impulse);
            anim.SetTrigger("DoJump");
            anim.SetBool("IsAttackUp", true);
            isJump = true;
            StartCoroutine(weapon.AttackAreaOnOff(30));
            isUpAttack = false; //강화 상태 해제
        }
    }
    
    //쉴드버튼
    public void Shield()
    {
        if (isShield) //쉴드가 가능한 상태일때 
        {
            isShield = false;
            isShieldState = true;
            anim.SetTrigger("DoShield");
            Debug.Log("Shield");
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
                isShieldState = false;
                curShieldCoolTime = 0;
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
            isUpAttack = true; //최대치 보다 증가하면 강화 공격 되는 상태 만들기
            GameManager.instance.attackUpGaugePs.Play();
        }
    }
    private void Update()
    {
        ShieldTimeCheck();
    }

    public void Hit(int damage)
    {
        GameManager.instance.playerHealth[curHealth-1].SetActive(false); //체력이미지 없애기
        curHealth -= damage; //데미지의 따른 체력 감소
        Debug.Log(curHealth);
        if (curHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            rigid.constraints = RigidbodyConstraints2D.FreezePositionY;
            GameManager.instance.GameOver();
            Debug.Log("캐릭터 사망");
        }
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //바닥과 닿았는지 체크 후 점프 가능한 상태로 만들어줌
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetTrigger("DoFall");
            isJump = false;

            anim.SetBool("IsAttackUp", false);
            isUpAttackState = false; //강화 상태 해제
        }
    }
}
