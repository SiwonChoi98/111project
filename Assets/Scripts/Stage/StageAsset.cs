using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage / Create New Stage")]
public class StageAsset : ScriptableObject
{
    public int monsterCount; //���� ��
    public List<Monster> monsters; //���� ����
    
}
