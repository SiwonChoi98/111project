using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Monster : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] protected int curHealth; //ü��
    public int damage; //���ݷ�
    //������ �ؽ�Ʈ
    [SerializeField] private GameObject dmgText;
    private string _dmgTextFolderName = "DamageText/dmgText";
    public virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public virtual void Start()
    {
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
        Initialized();
    }
    //�ʱ�ȭ
    public virtual void Initialized() 
    {
        //�⺻ 1
        damage = 1; 
        curHealth = 1; 
    }
    
    public virtual void Hit(int damage)
    {
        curHealth -= damage;
        DamageText(damage, this.gameObject);
        if (curHealth <= 0)
        {
            //GetComponent<Collider2D>().enabled = false;
            Destroy(gameObject);
            Debug.Log("���� ���");
            SoundManager.instance.SfxPlaySound(0);
            GameManager.instance.score += 100;
            EffectManager.instance.PlayEffect(0, gameObject, 0.7f);
        }
    }
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player") && !player.isJump) //�÷��̾ �ٴڿ� ���� �� ������ ������
        {
            player.Hit(damage);
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
