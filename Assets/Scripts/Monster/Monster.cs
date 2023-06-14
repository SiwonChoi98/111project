using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : MonoBehaviour
{
    private Vector3 target; //타겟으로 이동
    private Rigidbody2D rigid; //물리
    [SerializeField] protected int curHealth; //체력
    public int damage; //공격력
    //데미지 텍스트
    [SerializeField] private GameObject dmgText;
    private string _dmgTextFolderName = "DamageText/dmgText";

    //private bool isMove = true;
    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        //isMove = true;
    }
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Floor").transform.position;
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
        Initialized();
    }
    
    protected void Move()
    {
        rigid.velocity = new Vector3(0,target.y,0);
    }
    protected void FixedUpdate()
    {   
         Move();
    }
    //초기화
    public virtual void Initialized() 
    {
    }
    public virtual void Hit(int damage, int index)
    {
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player") && !player.isJump) //플레이어가 바닥에 있을 때 닿으면 데미지
        {
            player.Hit(damage);
        }
        if (collision.gameObject.CompareTag("Player") && player.isShieldState)
        {
            player.isShieldState = false;
        }
    }
    public void DamageText(int damage, GameObject pos)
    {
        GameObject dmgtext1 = Instantiate(dmgText);
        dmgtext1.transform.position = pos.transform.position;
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//자식텍스트로 들어가서 //dmg는 int니까 string형태로 바꿔주기
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
}
