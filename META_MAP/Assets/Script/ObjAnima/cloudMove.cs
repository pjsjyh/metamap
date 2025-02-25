using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMove : MonoBehaviour
{
    Vector3 pos; //현재위치
    public float delta = 2.0f; // 좌(우)로 이동가능한 (x)최대값
    public float speed = 3.0f; // 이동속도

    public bool upDir = false;
    public bool frontDir = false;

    void Start()
    {
        pos = transform.position;
    }

    void Update()
    {
        Vector3 v = pos;
        if (!frontDir)
            v.x += delta * Mathf.Sin(Time.time * speed);
        else
            v.z += delta * Mathf.Sin(Time.time * speed);

        if (upDir)
            v.y += delta * Mathf.Sin(Time.time * speed);
        // 좌우 이동의 최대치 및 반전 처리
        transform.position = v;
    }
}
