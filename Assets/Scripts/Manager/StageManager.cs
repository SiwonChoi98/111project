using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

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

        //�������� ���� ����
        foreach (var stage in Resources.LoadAll(_folderName))
        {
            stageAssets.Add((StageAsset)stage);
        }
    }

}
