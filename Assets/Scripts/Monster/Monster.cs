using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : MonoBehaviour
{
    private Vector3 _target; //Ÿ������ �̵�
    private Rigidbody2D _rigid; //����
    [SerializeField] protected int curHealth; //ü��
    public int damage; //���ݷ�
    

    public void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }
    public void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Floor").transform.position;
        Initialized();
    }
    protected void Move()
    {
        _rigid.velocity = new Vector3(0,_target.y,0);
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

        if (collision.gameObject.CompareTag("Player") && player.isShieldState)
        {
            _rigid.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
            player.isShieldState = false;
            SoundManager.instance.SfxPlaySound(4);
            GameManager.instance.shieldPs.Stop();
        }
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
        
    }
   
}
