using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Weapon : MonoBehaviour
{
    //���ݷ�
    public int damage;
    //���ݹ���
    [SerializeField] private BoxCollider2D attackArea;
    //������ �ؽ�Ʈ
    [SerializeField] private GameObject dmgText;
    private string _dmgTextFolderName = "DamageText/dmgText";
    private void Awake()
    {
        attackArea = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
    }
    public IEnumerator AttackAreaOnOff() //���� on/off
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.3f); //�ݶ��̴� ���� �ð�
        attackArea.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Monster"))
        {
            collider.GetComponent<Monster>().Hit(damage);
            DamageText(damage);
        }
    }

    public void DamageText(int damage)
    {
        GameObject dmgtext1 = Instantiate(dmgText);
        dmgtext1.transform.position = attackArea.transform.position;
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//�ڽ��ؽ�Ʈ�� ���� //dmg�� int�ϱ� string���·� �ٲ��ֱ�
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
}
