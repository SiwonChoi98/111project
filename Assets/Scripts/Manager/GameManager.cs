using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Player player;

    [Header("���� ����")]
    [SerializeField] private Transform monsterSpawnTrans; //���� ������ġ
    public GameObject testMonster; //�׽�Ʈ ���� 
    public float spawnTime = 0; //���� �����ð�
    public float maxSpawnTime = 3; //���� ���� �ʱ�ȭ �ð�
    [Header("UI")]
    [SerializeField] private Image shieldTimeImage; //���� UI ��Ÿ�� �̹���
    [SerializeField] private Image attackUpGaugeImage; //��ȭ ���� �̹���
    public ParticleSystem attackUpGaugePs; //(��ư)��ȭ ���� ��ƼŬ
    public ParticleSystem attackUpPs; //(ĳ����)��ȭ ��ƼŬ
    public ParticleSystem floorPs; //(ĳ����)���� ��ƼŬ
    [SerializeField] private GameObject settingPanel; //�ɼ� �ǳ�
    public GameObject gameOverPanel; //���ӿ��� �ǳ�
   
    [Header("����")]
    public int score; //����
    [SerializeField] private Text scoreTxt;
    [Header("ü��")]
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
        //���� �ʱ�ȭ
        score = 0; 
        //ü�� �̹��� �ʱ�ȭ
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
        //���� 
        scoreTxt.text = score.ToString();
        
        //���������
        if (!player.isShield) 
            shieldTimeImage.fillAmount = player.curShieldCoolTime / player.maxShieldCoolTime;
        else
            shieldTimeImage.fillAmount = 0; 
        
        //��ȭ ���� ������
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
