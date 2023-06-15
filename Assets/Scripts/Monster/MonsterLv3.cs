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
        GameManager.instance.DamageText(damage, this.gameObject);
        GameManager.instance.ComboTextActive(this.gameObject);
        if (curHealth <= 0)
        {
            gameObject.SetActive(false);
            SoundManager.instance.SfxPlaySound(0, 0.7f);
            GameManager.instance.comboCount++;
            GameManager.instance.score += 1 + GameManager.instance.comboCount;
            GameManager.instance.currentMonsterCount--;
            if (index == 0) //��ȭ����Ʈ
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
