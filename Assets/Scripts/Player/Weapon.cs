using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Weapon : MonoBehaviour
{
    //공격력
    public int damage;
    //공격범위
    [SerializeField] private BoxCollider2D attackArea;
    //데미지 텍스트
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
    public IEnumerator AttackAreaOnOff() //공격 on/off
    {
        attackArea.enabled = true;
        yield return new WaitForSeconds(0.3f); //콜라이더 지속 시간
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
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//자식텍스트로 들어가서 //dmg는 int니까 string형태로 바꿔주기
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
}
