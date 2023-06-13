using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform target;
    public Vector3 Offset;

    private void LateUpdate()
    {
        //만약 target이 제대로 설정이 안되었다면, Player를 찾아서 넣어준다.
        if (target == null)
        {
            GameObject go = GameObject.Find("Player");
            target = go.transform;
        }
        transform.position = target.position;

    }
}
