using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLv1 : Monster
{
    public override void Initialized()
    {
        damage = 1;
        curHealth = 1;
    }
    public override void Hit(int damage, int index)
    {
        curHealth -= damage;
        GameManager manager = GameManager.instance;
        manager.DamageText(damage, this.gameObject);
        manager.ComboTextActive(this.gameObject);
        if (curHealth <= 0)
        {
            gameObject.SetActive(false);
            SoundManager.instance.SfxPlaySound(0, 0.7f);
            manager.comboCount++;
            manager.score += 1 + manager.comboCount;
            manager.currentMonsterCount--;
            if (index == 0) //°­È­ÀÌÆåÆ®
                EffectManager.instance.PlayEffect(0, gameObject, 0.7f);
            else
                EffectManager.instance.PlayEffect(1, gameObject, 0.7f);

            manager.NextStage();
        }
    }
}
