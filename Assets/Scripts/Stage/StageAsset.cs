using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage / Create New Stage")]
public class StageAsset : ScriptableObject
{
    public int monsterCount; //몬스터 수
    public List<Monster> monsters; //몬스터 종류
    
}
