using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    [Header("몬스터 생성")]
    [SerializeField] private Transform monsterSpawnTrans; //몬스터 생성위치
    public GameObject testMonster; //테스트 몬스터 
    public float spawnTime = 0; //몬스터 생성시간
    public float maxSpawnTime = 3; //몬스터 생성 초기화 시간
    [Header("UI")]
    [SerializeField] private Image shieldTimeImage; //쉴드 UI 쿨타임 이미지
    [SerializeField] private GameObject settingPanel; //옵션 판넬
    [Header("점수")]
    public int score; //점수
    [SerializeField] private Text scoreTxt;
    public void StopGame()
    {
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
    }
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Initialized();
    }
    public void Initialized()
    {
        score = 0;
    }
    private void Update()
    {
        Spawn();
    }
    private void LateUpdate()
    {
        GUI();
    }
    private void GUI()
    {
        if (!player.isShield)
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0;

        scoreTxt.text = score.ToString();
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
