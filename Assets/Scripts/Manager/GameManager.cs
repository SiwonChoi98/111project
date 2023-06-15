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
    [SerializeField] private Image shieldTimeImage; //���� UI ��Ÿ�� �̹���
    [SerializeField] private Image attackUpGaugeImage; //��ȭ ���� �̹���
    [SerializeField] private Image jumpUpGaugeImage; //��ȭ ���� �̹���
    public ParticleSystem attackUpGaugePs; //(��ư)��ȭ ���� ��ƼŬ
    public ParticleSystem attackUpPs; //(ĳ����)��ȭ ��ƼŬ
    public ParticleSystem jumpUpPs; //(��ư) ��ȭ ���� ��ƼŬ
    public ParticleSystem floorPs; //(ĳ����)���� ��ƼŬ
    public ParticleSystem shieldPs; //(ĳ����)���� ��ƼŬ
    public ParticleSystem healthUpPs; //(�̹���) ü�� ȸ�� �� ��ƼŬ
    [SerializeField] private GameObject settingPanel; //�ɼ� �ǳ�
    [SerializeField] private GameObject gameOverPanel; //���ӿ��� �ǳ�
    [SerializeField] private GameObject GameStartPanel; //���ӽ��� �ǳ�
    [SerializeField] private Image stageImage; //�������� �̹���
    [SerializeField] Text stageTxt; //�������� �ؽ�Ʈ
    [SerializeField] private PlayableDirector stageUpTimeLine; //�������� ���� Ÿ�Ӷ���
    [Header("����")]
    public int bestScore; //�ְ�����
    [SerializeField] private Text bestScoreTxt; //�ְ����� �ؽ�Ʈ
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
    
    [Header("������ �ؽ�Ʈ")]
    [SerializeField] private GameObject dmgText;
    private string _dmgTextFolderName = "DamageText/dmgText";
    [Header("�޺� �ؽ�Ʈ")]
    public int comboCount; //�޺� Ƚ��
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
        GameStartPanel.SetActive(true);
        //���� �ʱ�ȭ
        score = 0;
        //�������� �ʱ�ȭ
        stage = 1;
        //�޺� Ƚ�� �ʱ�ȭ
        comboCount = 0;
        //ü�� �̹��� �ʱ�ȭ
        foreach (GameObject health in playerHealth)
            health.SetActive(true);
        //������ �ؽ�Ʈ ����
        dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
        //�޺� �ؽ�Ʈ ����
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
        //���� 
        scoreTxt.text = score.ToString();
        bestScoreTxt.text = bestScore.ToString();
        //�������� �ؽ�Ʈ
        stageTxt.text = "WAVE " + stage;
        //���������
        if (!player.isShield) 
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0; 
        
        //��ȭ ���� ������
        attackUpGaugeImage.fillAmount = Mathf.Lerp(attackUpGaugeImage.fillAmount, (float)player.curAttackUpGauge / player.maxAttackUpGauge / 1 / 1, Time.deltaTime * 5);
        //��ȭ ���� ������
        jumpUpGaugeImage.fillAmount = Mathf.Lerp(jumpUpGaugeImage.fillAmount, (float)player.curJumpUpGauge / player.maxJumpUpGauge / 1 / 1, Time.deltaTime * 5);
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
    //���� �������� ���� �ؾ��� �͵�
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
        stageUpTimeLine.Play();
        stage++;
        spawnIndex = 0;
        player.isJump = false;
        MonsterSpawnDataSave(); //�������� �ٽ� ����
    }
    //������ �ؽ�Ʈ
    public void DamageText(int damage, GameObject pos)
    {
        GameObject dmgtext1 = Instantiate(dmgText);
        dmgtext1.transform.position = pos.transform.position;
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//�ڽ��ؽ�Ʈ�� ���� //dmg�� int�ϱ� string���·� �ٲ��ֱ�
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
    //�޺� �ؽ�Ʈ
    private void ComboText(int combo, GameObject pos)
    {
        GameObject combotext1 = Instantiate(comboText);
        combotext1.transform.position = pos.transform.position;
        combotext1.GetComponentInChildren<Text>().text = "+"+combo+" Combo";//�ڽ��ؽ�Ʈ�� ���� //dmg�� int�ϱ� string���·� �ٲ��ֱ�
        //dmgtext1.transform.parent 
        Destroy(combotext1, 1f);
    }
    public void ComboTextActive(GameObject pos)
    {
        if(comboCount > 1) //�޺��� 1���� ���� ���� 
            ComboText(comboCount, pos);
    }

    
    
}
