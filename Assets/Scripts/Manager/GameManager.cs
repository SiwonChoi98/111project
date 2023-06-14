using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Image attackUpGaugeImage; //강화 공격 이미지
    public ParticleSystem attackUpGaugePs; //(버튼)강화 공격 파티클
    public ParticleSystem attackUpPs; //(캐릭터)강화 파티클
    public ParticleSystem floorPs; //(캐릭터)착지 파티클
    [SerializeField] private GameObject settingPanel; //옵션 판넬
    public GameObject gameOverPanel; //게임오버 판넬
   
    [Header("점수")]
    public int score; //점수
    [SerializeField] private Text scoreTxt;
    [Header("체력")]
    public GameObject[] playerHealth;
    public void StopGame()
    {
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        StopGame();
    }
    public void ResetGame()
    {
        SceneManager.LoadScene("SampleScene");
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
        PlayGame();
    }
    public void Initialized()
    {
        //점수 초기화
        score = 0; 
        //체력 이미지 초기화
        foreach (GameObject health in playerHealth)
            health.SetActive(true);
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
        //점수 
        scoreTxt.text = score.ToString();
        
        //쉴드게이지
        if (!player.isShield) 
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0; 
        
        //강화 공격 게이지
        attackUpGaugeImage.fillAmount = Mathf.Lerp(attackUpGaugeImage.fillAmount, (float)player.curAttackUpGauge / player.maxAttackUpGauge / 1 / 1, Time.deltaTime * 5);
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
