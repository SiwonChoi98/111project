using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    [SerializeField] private Image shieldTimeImage;

    private void Start()
    {
        player.Initialized();
    }
    private void LateUpdate()
    {
        if (!player.isShield)
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0;

        //if (player.isDodgeReady)
        //    playerDodgeCoolImage.fillAmount = 0;
        //else
        //    playerDodgeCoolImage.fillAmount = player.dodgeCoolTime / player.dodgeCoolTimeMax;
    }
}
