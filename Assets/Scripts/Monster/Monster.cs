using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : MonoBehaviour
{
    private Vector3 target; //Ÿ������ �̵�
    private Rigidbody2D rigid; //����
    [SerializeField] protected int curHealth; //ü��
    public int damage; //���ݷ�
    

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Floor").transform.position;
        Initialized();
    }
    protected void Move()
    {
        rigid.velocity = new Vector3(0,target.y,0);
    }
    protected void FixedUpdate()
    {   
        if(!GameManager.instance.player.isShieldState) //isMove && 
            Move();
    }
    //�ʱ�ȭ
    public virtual void Initialized() 
    {
    }
    public virtual void Hit(int damage, int index)
    {
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player") && !player.isJump) //�÷��̾ �ٴڿ� ���� �� ������ ������
        {
            if(player.shieldCount > 0)
            {
                player.shieldCount = 0;
                return;
            }
            player.Hit(damage);
            GameManager.instance.comboCount = 0;
        }
        if (collision.gameObject.CompareTag("Player") && player.isShieldState)
        {
            rigid.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
            player.isShieldState = false;
            GameManager.instance.shieldPs.Stop();
        }
    }
   
}
