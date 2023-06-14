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
    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Start()
    {
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
        Initialized();
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
