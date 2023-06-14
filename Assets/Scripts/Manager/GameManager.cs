using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    [Header("UI")]
    [SerializeField] private Image shieldTimeImage; //쉴드 UI 쿨타임 이미지
    [SerializeField] private Image attackUpGaugeImage; //강화 공격 이미지
    public ParticleSystem attackUpGaugePs; //(버튼)강화 공격 파티클
    public ParticleSystem attackUpPs; //(캐릭터)강화 파티클
    public ParticleSystem floorPs; //(캐릭터)착지 파티클
    public ParticleSystem shieldPs; //(캐릭터)쉴드 파티클
    [SerializeField] private GameObject settingPanel; //옵션 판넬
    public GameObject gameOverPanel; //게임오버 판넬
    [SerializeField] private Image stageImage;
    [SerializeField] Text stageTxt;
    [Header("점수")]
    public int score; //점수
    [SerializeField] private Text scoreTxt; //점수 텍스트
    [Header("체력")]
    public GameObject[] playerHealth; //체력 이미지
    [Header("스테이지")]
    public int stage; //현재 스테이지
    public List<Monster> monsters; //몬스터 리스트
    public int spawnCount; //현재 스테이지 스폰 몬스터 수
    private int spawnIndex; //스폰 몬스터 인덱스
    public int currentMonsterCount; //다음 스테이지로 넘어갈 몬스터 수
    [SerializeField] private Transform monsterSpawnTrans; //몬스터 생성위치
    public float spawnTime = 0; //몬스터 생성시간
    public float maxSpawnTime = 3; //몬스터 생성 초기화 시간
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

        monsters = new List<Monster>();
    }
    private void Start()
    {
        Initialized();
        PlayGame();
        MonsterSpawnDataSave();
    }

    //몬스터 풀 저장
    private void MonsterSpawnDataSave()
    {
        for (int i = 0; i < StageManager.instance.stageAssets[stage-1].monsterCount; i++) //스테이지 별 풀 저장
        {
            monsters.Add(StageManager.instance.stageAssets[stage-1].monsters[i]); //몬스터 
            monsters[i] = Instantiate(monsters[i]);
            monsters[i].gameObject.SetActive(false);
            monsters[i].transform.parent = monsterSpawnTrans;
        }
        currentMonsterCount = StageManager.instance.stageAssets[stage - 1].monsterCount;
        spawnCount = StageManager.instance.stageAssets[stage-1].monsterCount;
    }
    public void Initialized()
    {
        //점수 초기화
        score = 0;
        //스테이지 초기화
        stage = 1;
        //체력 이미지 초기화
        foreach (GameObject health in playerHealth)
            health.SetActive(true);   
    }
    private void Update()
    {
        MonsterSpwan();
    }
    private void LateUpdate()
    {
        GUI();
    }
    private void GUI()
    {
        //점수 
        scoreTxt.text = score.ToString();
        //스테이지 텍스트
        stageTxt.text = "WAVE " + stage;
        //쉴드게이지
        if (!player.isShield) 
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0; 
        
        //강화 공격 게이지
        attackUpGaugeImage.fillAmount = Mathf.Lerp(attackUpGaugeImage.fillAmount, (float)player.curAttackUpGauge / player.maxAttackUpGauge / 1 / 1, Time.deltaTime * 5);

        //스테이지 이미지
        stageImage.fillAmount = Mathf.Lerp(stageImage.fillAmount, (float)stage / 9 / 1 / 1, Time.deltaTime * 5);
    }
    //몬스터 생성
    public void MonsterSpwan()
    {
        if (spawnCount > 0) //몬스터 생성 수가 0보다 클때만 생성 
        {
            if (spawnTime < maxSpawnTime)
            {
                spawnTime += Time.deltaTime;
            }
            else
            {
                monsters[spawnIndex].gameObject.SetActive(true);
                monsters[spawnIndex].transform.position = monsterSpawnTrans.position;
                
                spawnCount--;
                spawnIndex++;
            }
        }
    }
    public void NextStage()
    {
        if(currentMonsterCount <= 0)
        {
            //리스트 목록제거
            monsters.RemoveRange(0, StageManager.instance.stageAssets[stage - 1].monsterCount);

            //오브젝트 제거
            for (int i=0; i< monsterSpawnTrans.childCount; i++) 
                Destroy(monsterSpawnTrans.GetChild(i).gameObject);

            //스테이지 업
            StartCoroutine(StageUp()); 
        }
    }
    private IEnumerator StageUp()
    {
        yield return new WaitForSeconds(2f);
        stage++;
        spawnIndex = 0;
        MonsterSpawnDataSave(); //스테이지 다시 세팅
    }
}
