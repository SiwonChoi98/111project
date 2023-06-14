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
    [SerializeField] private Image shieldTimeImage; //���� UI ��Ÿ�� �̹���
    [SerializeField] private Image attackUpGaugeImage; //��ȭ ���� �̹���
    public ParticleSystem attackUpGaugePs; //(��ư)��ȭ ���� ��ƼŬ
    public ParticleSystem attackUpPs; //(ĳ����)��ȭ ��ƼŬ
    public ParticleSystem floorPs; //(ĳ����)���� ��ƼŬ
    public ParticleSystem shieldPs; //(ĳ����)���� ��ƼŬ
    [SerializeField] private GameObject settingPanel; //�ɼ� �ǳ�
    public GameObject gameOverPanel; //���ӿ��� �ǳ�
    [SerializeField] private Image stageImage;
    [SerializeField] Text stageTxt;
    [Header("����")]
    public int score; //����
    [SerializeField] private Text scoreTxt; //���� �ؽ�Ʈ
    [Header("ü��")]
    public GameObject[] playerHealth; //ü�� �̹���
    [Header("��������")]
    public int stage; //���� ��������
    public List<Monster> monsters; //���� ����Ʈ
    public int spawnCount; //���� �������� ���� ���� ��
    private int spawnIndex; //���� ���� �ε���
    public int currentMonsterCount; //���� ���������� �Ѿ ���� ��
    [SerializeField] private Transform monsterSpawnTrans; //���� ������ġ
    public float spawnTime = 0; //���� �����ð�
    public float maxSpawnTime = 3; //���� ���� �ʱ�ȭ �ð�
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

    //���� Ǯ ����
    private void MonsterSpawnDataSave()
    {
        for (int i = 0; i < StageManager.instance.stageAssets[stage-1].monsterCount; i++) //�������� �� Ǯ ����
        {
            monsters.Add(StageManager.instance.stageAssets[stage-1].monsters[i]); //���� 
            monsters[i] = Instantiate(monsters[i]);
            monsters[i].gameObject.SetActive(false);
            monsters[i].transform.parent = monsterSpawnTrans;
        }
        currentMonsterCount = StageManager.instance.stageAssets[stage - 1].monsterCount;
        spawnCount = StageManager.instance.stageAssets[stage-1].monsterCount;
    }
    public void Initialized()
    {
        //���� �ʱ�ȭ
        score = 0;
        //�������� �ʱ�ȭ
        stage = 1;
        //ü�� �̹��� �ʱ�ȭ
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
        //���� 
        scoreTxt.text = score.ToString();
        //�������� �ؽ�Ʈ
        stageTxt.text = "WAVE " + stage;
        //���������
        if (!player.isShield) 
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0; 
        
        //��ȭ ���� ������
        attackUpGaugeImage.fillAmount = Mathf.Lerp(attackUpGaugeImage.fillAmount, (float)player.curAttackUpGauge / player.maxAttackUpGauge / 1 / 1, Time.deltaTime * 5);

        //�������� �̹���
        stageImage.fillAmount = Mathf.Lerp(stageImage.fillAmount, (float)stage / 9 / 1 / 1, Time.deltaTime * 5);
    }
    //���� ����
    public void MonsterSpwan()
    {
        if (spawnCount > 0) //���� ���� ���� 0���� Ŭ���� ���� 
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
            //����Ʈ �������
            monsters.RemoveRange(0, StageManager.instance.stageAssets[stage - 1].monsterCount);

            //������Ʈ ����
            for (int i=0; i< monsterSpawnTrans.childCount; i++) 
                Destroy(monsterSpawnTrans.GetChild(i).gameObject);

            //�������� ��
            StartCoroutine(StageUp()); 
        }
    }
    private IEnumerator StageUp()
    {
        yield return new WaitForSeconds(2f);
        stage++;
        spawnIndex = 0;
        MonsterSpawnDataSave(); //�������� �ٽ� ����
    }
}
