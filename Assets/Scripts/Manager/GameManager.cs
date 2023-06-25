using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Text;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;
    private bool _isPlay = false;

    [Header("UI")]
    [SerializeField] private Image _shieldTimeImage; //���� UI ��Ÿ�� �̹���
    [SerializeField] private Image _attackUpGaugeImage; //��ȭ ���� �̹���
    [SerializeField] private Image _jumpUpGaugeImage; //��ȭ ���� �̹���
    //�̺κ��� ����Ʈ �Ŵ������� ����ϴ°� ȿ�����̴�.
    public ParticleSystem attackUpGaugePs; //(��ư)��ȭ ���� ��ƼŬ
    public ParticleSystem attackUpPs; //(ĳ����)��ȭ ��ƼŬ
    public ParticleSystem jumpUpPs; //(��ư) ��ȭ ���� ��ƼŬ
    public ParticleSystem floorPs; //(ĳ����)���� ��ƼŬ
    public ParticleSystem shieldPs; //(ĳ����)���� ��ƼŬ
    public ParticleSystem healthUpPs; //(�̹���) ü�� ȸ�� �� ��ƼŬ
    [SerializeField] private GameObject _settingPanel; //�ɼ� �ǳ�
    [SerializeField] private GameObject _gameOverPanel; //���ӿ��� �ǳ�
    [SerializeField] private GameObject _gameStartPanel; //���ӽ��� �ǳ�
    [SerializeField] private Image _stageImage; //�������� �̹���
    [SerializeField] private Text _stageTxt; //�������� �ؽ�Ʈ
    [SerializeField] private PlayableDirector _stageUpTimeLine; //�������� ���� Ÿ�Ӷ���
    [Header("����")]
    public int bestScore; //�ְ�����
    [SerializeField] private Text _bestScoreTxt; //�ְ����� �ؽ�Ʈ
    public int score; //����
    [SerializeField] private Text _scoreTxt; //���� �ؽ�Ʈ
    [Header("ü��")]
    public GameObject[] playerHealth; //ü�� �̹���
    [Header("��������")]
    public int stage; //���� ��������
    public List<Monster> monsters; //���� ����Ʈ
    public int spawnCount; //���� �������� ���� ���� ��
    private int _spawnIndex; //���� ���� �ε���
    public int currentMonsterCount; //���� ���������� �Ѿ ���� ��
    [SerializeField] private Transform _monsterSpawnTrans; //���� ������ġ
    public float spawnTime = 0; //���� �����ð�
    public float maxSpawnTime = 3; //���� ���� �ʱ�ȭ �ð�
    
    [Header("������ �ؽ�Ʈ")]
    [SerializeField] private GameObject _dmgText;
    private string _dmgTextFolderName = "DamageText/dmgText";
    [Header("�޺� �ؽ�Ʈ")]
    public int comboCount; //�޺� Ƚ��
    [SerializeField] private GameObject _comboText;
    private string _comboTextFolderName = "ComboText/comboText";

    private StringBuilder sb; //��Ʈ�� ������ �������ϸ� �������ش�.
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
        _gameOverPanel.SetActive(true);
        StopGame();
    }
    public void StartGame()
    {
        UIClickSound();
        _isPlay = true;
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
        //sb = new StringBuilder();
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
            monsters[i].transform.parent = _monsterSpawnTrans;
        }
        currentMonsterCount = StageManager.instance.stageAssets[stage - 1].monsterCount;
        spawnCount = StageManager.instance.stageAssets[stage-1].monsterCount;
    }
    public void Initialized()
    {
        _gameStartPanel.SetActive(true);
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
        _dmgText = (GameObject)Resources.Load(_dmgTextFolderName);
        //�޺� �ؽ�Ʈ ����
        _comboText = (GameObject)Resources.Load(_comboTextFolderName);
    }
    private void Update()
    {
        if(_isPlay)
            MonsterSpwan();
    }
    private void LateUpdate()
    {
        GUI();
    }
    private void GUI()
    {
        //���� 
        _scoreTxt.text = score.ToString();
        _bestScoreTxt.text = bestScore.ToString();
        //�������� �ؽ�Ʈ
        _stageTxt.text = "WAVE " + stage;
        //���������
        if (!player.isShield) 
            _shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            _shieldTimeImage.fillAmount = 0; 
        
        //��ȭ ���� ������
        _attackUpGaugeImage.fillAmount = Mathf.Lerp(_attackUpGaugeImage.fillAmount, (float)player.curAttackUpGauge / player.maxAttackUpGauge / 1 / 1, Time.deltaTime * 5);
        //��ȭ ���� ������
        _jumpUpGaugeImage.fillAmount = Mathf.Lerp(_jumpUpGaugeImage.fillAmount, (float)player.curJumpUpGauge / player.maxJumpUpGauge / 1 / 1, Time.deltaTime * 5);
        //�������� �̹���
        _stageImage.fillAmount = Mathf.Lerp(_stageImage.fillAmount, (float)stage / 9 / 1 / 1, Time.deltaTime * 5);
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
                monsters[_spawnIndex].gameObject.SetActive(true);
                monsters[_spawnIndex].transform.position = _monsterSpawnTrans.position;
                
                spawnCount--;
                _spawnIndex++;
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
            for (int i=0; i< _monsterSpawnTrans.childCount; i++) 
                Destroy(_monsterSpawnTrans.GetChild(i).gameObject);

            //�������� ��
            StartCoroutine(StageUp()); 
        }
    }
    private IEnumerator StageUp()
    {
        yield return new WaitForSeconds(2f);
        _stageUpTimeLine.Play();
        stage++;
        _spawnIndex = 0;
        player.isJump = false;
        MonsterSpawnDataSave(); //�������� �ٽ� ����
    }
    //������ �ؽ�Ʈ
    public void DamageText(int damage, GameObject pos)
    {
        GameObject dmgtext1 = Instantiate(_dmgText);
        dmgtext1.transform.position = pos.transform.position;
        dmgtext1.GetComponentInChildren<Text>().text = damage.ToString();//�ڽ��ؽ�Ʈ�� ���� //dmg�� int�ϱ� string���·� �ٲ��ֱ�
        //dmgtext1.transform.parent 
        Destroy(dmgtext1, 0.7f);
    }
    //�޺� �ؽ�Ʈ
    private void ComboText(int combo, GameObject pos)
    {
        GameObject combotext1 = Instantiate(_comboText);
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
