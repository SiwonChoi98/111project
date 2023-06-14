using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLv3 : Monster
{
    public override void Initialized()
    {
        damage = 2;
        curHealth = 3;
    }
    public override void Hit(int damage, int index)
    {
        curHealth -= damage;
        DamageText(damage, this.gameObject);
        if (curHealth <= 0)
        {
            //GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
            Debug.Log("∏ÛΩ∫≈Õ ªÁ∏¡");
            SoundManager.instance.SfxPlaySound(0);
            GameManager.instance.score += 100;
            GameManager.instance.currentMonsterCount--;
            if (index == 0) //∞≠»≠¿Ã∆Â∆Æ
                EffectManager.instance.PlayEffect(0, gameObject, 0.7f);
            else
                EffectManager.instance.PlayEffect(1, gameObject, 0.7f);

            GameManager.instance.NextStage();
        }
        else
        {
            EffectManager.instance.PlayEffect(3, gameObject, 0.7f);
        }
    }
}
