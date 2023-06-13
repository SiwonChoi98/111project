using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    [Header("MonsterSpawn")]
    [SerializeField] private Transform monsterSpawnTrans;
    public GameObject testMonster;
    public float spawnTime = 0;
    public float maxSpawnTime = 3;
    [Header("UI")]
    [SerializeField] private Image shieldTimeImage;
    [SerializeField] private GameObject settingPanel;

    public void StopGame()
    {
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
    }
    private void Start()
    {
        player.Initialized();
    }
    private void Update()
    {
        Spawn();
    }
    private void LateUpdate()
    {
        Gui();
    }
    private void Gui()
    {
        if (!player.isShield)
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0;
    }

    public void Spawn()
    {
        if (spawnTime < maxSpawnTime)
        {
            spawnTime += Time.deltaTime;
        }
        else
        {   
            spawnTime = 0;
            GameObject gameObject = Instantiate(testMonster);
            gameObject.transform.position = monsterSpawnTrans.position;
            gameObject.transform.parent = monsterSpawnTrans;
           
        }
        
    }

}
