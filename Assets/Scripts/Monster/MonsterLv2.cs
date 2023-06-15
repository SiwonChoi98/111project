using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLv2 : Monster
{
    public override void Initialized()
    {
        damage = 1;
        curHealth = 2;
    }

    public override void Hit(int damage, int index)
    {
        curHealth -= damage;
        GameManager.instance.DamageText(damage, this.gameObject);
        GameManager.instance.ComboTextActive(this.gameObject);
        if (curHealth <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("���� ���");
            SoundManager.instance.SfxPlaySound(0);
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
            EffectManager.instance.PlayEffect(2, gameObject, 0.7f);
        }
    }
}
