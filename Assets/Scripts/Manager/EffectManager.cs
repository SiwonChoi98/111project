using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EffectStruct
{
    public string name;
    public GameObject effect;
}
[Serializable]
public class UIStruct
{
    public string name;
    public GameObject effect;
}
public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public List<EffectStruct> effectList = new List<EffectStruct>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(int index, GameObject obj, float time)
    {
        var effect = Instantiate(effectList[index].effect);
        effect.transform.position = obj.transform.position;
        effect.transform.rotation = obj.transform.rotation;
        Destroy(effect, time);

    }
}
