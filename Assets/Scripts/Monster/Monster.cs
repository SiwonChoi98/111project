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
    //������ �ؽ�Ʈ
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
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//�ڽ��ؽ�Ʈ�� ���� //dmg�� int�ϱ� string���·� �ٲ��ֱ�
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
}
