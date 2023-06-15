using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    private bool isPlay = false;

    [Header("UI")]
    [SerializeField] private Image shieldTimeImage; //쉴드 UI 쿨타임 이미지
    [SerializeField] private Image attackUpGaugeImage; //강화 공격 이미지
    [SerializeField] private Image jumpUpGaugeImage; //강화 점프 이미지
    public ParticleSystem attackUpGaugePs; //(버튼)강화 공격 파티클
    public ParticleSystem attackUpPs; //(캐릭터)강화 파티클
    public ParticleSystem jumpUpPs; //(버튼) 강화 점프 파티클
    public ParticleSystem floorPs; //(캐릭터)착지 파티클
    public ParticleSystem shieldPs; //(캐릭터)쉴드 파티클
    public ParticleSystem healthUpPs; //(이미지) 체력 회복 시 파티클
    [SerializeField] private GameObject settingPanel; //옵션 판넬
    [SerializeField] private GameObject gameOverPanel; //게임오버 판넬
    [SerializeField] private GameObject GameStartPanel; //게임시작 판넬
    [SerializeField] private Image stageImage; //스테이지 이미지
    [SerializeField] Text stageTxt; //스테이지 텍스트
    [SerializeField] private PlayableDirector stageUpTimeLine; //스테이지 증가 타임라인
    [Header("점수")]
    public int bestScore; //최고점수
    [SerializeField] private Text bestScoreTxt; //최고점수 텍스트
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
    
    [Header("데미지 텍스트")]
    [SerializeField] private GameObject dmgText;
    private string _dmgTextFolderName = "DamageText/dmgText";
    [Header("콤보 텍스트")]
    public int comboCount; //콤보 횟수
    [SerializeField] private GameObject comboText;
    private string _comboTextFolderName = "ComboText/comboText";
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
        SoundManager.instance.BgmPlaySound(1, 0.5f);
        StageManager.instance.LastStageUp();
        gameOverPanel.SetActive(true);
        StopGame();
    }
    public void StartGame()
    {
        UIClickSound();
        isPlay = true;
    }
    public void ResetGame()
    {
        UIClickSound();
        SceneManager.LoadScene("SampleScene");
    }
    public void UIClickSound()
    {
        SoundManager.instance.SfxPlaySound(3);
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
        SoundManager.instance.BgmPlaySound(0, 0.3f);
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
        GameStartPanel.SetActive(true);
        //점수 초기화
        score = 0;
        //스테이지 초기화
        stage = 1;
        //콤보 횟수 초기화
        comboCount = 0;
        //체력 이미지 초기화
        foreach (GameObject health in playerHealth)
            health.SetActive(true);
        //데미지 텍스트 저장
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
        //콤보 텍스트 저장
        comboText = (GameObject)Resources.Load(_comboTextFolderName);
    }
    private void Update()
    {
        if(isPlay)
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
        bestScoreTxt.text = bestScore.ToString();
        //스테이지 텍스트
        stageTxt.text = "WAVE " + stage;
        //쉴드게이지
        if (!player.isShield) 
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0; 
        
        //강화 공격 게이지
        attackUpGaugeImage.fillAmount = Mathf.Lerp(attackUpGaugeImage.fillAmount, (float)player.curAttackUpGauge / player.maxAttackUpGauge / 1 / 1, Time.deltaTime * 5);
        //강화 점프 게이지
        jumpUpGaugeImage.fillAmount = Mathf.Lerp(jumpUpGaugeImage.fillAmount, (float)player.curJumpUpGauge / player.maxJumpUpGauge / 1 / 1, Time.deltaTime * 5);
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
    //다음 스테이지 갈때 해야할 것들
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
        stageUpTimeLine.Play();
        stage++;
        spawnIndex = 0;
        player.isJump = false;
        MonsterSpawnDataSave(); //스테이지 다시 세팅
    }
    //데미지 텍스트
    public void DamageText(int damage, GameObject pos)
    {
        GameObject dmgtext1 = Instantiate(dmgText);
        dmgtext1.transform.position = pos.transform.position;
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//자식텍스트로 들어가서 //dmg는 int니까 string형태로 바꿔주기
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
    //콤보 텍스트
    private void ComboText(int combo, GameObject pos)
    {
        GameObject combotext1 = Instantiate(comboText);
        combotext1.transform.position = pos.transform.position;
        combotext1.GetComponentInChildren<Text>().text = "+"+combo+" Combo";//자식텍스트로 들어가서 //dmg는 int니까 string형태로 바꿔주기
        //dmgtext1.transform.parent 
        Destroy(combotext1, 1f);
    }
    public void ComboTextActive(GameObject pos)
    {
        if(comboCount > 1) //콤보가 1보다 높을 때만 
            ComboText(comboCount, pos);
    }

    
    
}
