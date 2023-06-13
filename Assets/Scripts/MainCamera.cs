using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Transform target;
    public Vector3 Offset;

    private void LateUpdate()
    {
        //���� target�� ����� ������ �ȵǾ��ٸ�, Player�� ã�Ƽ� �־��ش�.
        if (target == null)
        {
            GameObject go = GameObject.Find("Player");
            target = go.transform;
        }
        transform.position = target.position;

    }
}
