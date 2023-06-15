using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MonsterHealth : Monster
{
    public override void Initialized()
    {
        damage = 0;
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
            SoundManager.instance.SfxPlaySound(7, 0.7f);
            manager.comboCount++;
            manager.score += 1 + manager.comboCount;
            manager.currentMonsterCount--;
            if(manager.player.curHealth < 3)
            {
                manager.player.curHealth++;
                manager.playerHealth[manager.player.curHealth- 1].SetActive(true);
                manager.playerHealth[manager.player.curHealth - 1].transform.DOShakeScale(0.3f, 3);
                manager.playerHealth[manager.player.curHealth - 1].transform.DOShakePosition(0.3f, 3);
            }
            if (index == 0) //°­È­ÀÌÆåÆ®
                EffectManager.instance.PlayEffect(0, gameObject, 0.7f);
            else
                EffectManager.instance.PlayEffect(1, gameObject, 0.7f);

            manager.NextStage();
        }
        else
        {
            EffectManager.instance.PlayEffect(2, gameObject, 0.7f);
        }
    }
}
