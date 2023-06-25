using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    private const string _BEST_SCORE_KEY = "BEST_SCORE";
    [SerializeField] private string _folderName = "Stage";
    public List<StageAsset> stageAssets;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        //스테이지 정보 저장
        foreach (var stage in Resources.LoadAll(_folderName))
        {
            stageAssets.Add((StageAsset)stage);
        }
    }
    private void Start()
    {
        //PlayerPrefs.SetInt(BEST_SCORE_KEY, GameManager.instance.score = 0); //최고기록 초기화
        GameManager.instance.bestScore = PlayerPrefs.GetInt(_BEST_SCORE_KEY);
    }

    public void LastStageUp()
    {
        if (GameManager.instance.score >= GameManager.instance.bestScore)
        {
            GameManager.instance.bestScore = GameManager.instance.score;
            PlayerPrefs.SetInt(_BEST_SCORE_KEY, GameManager.instance.bestScore);
        }


    }
}
