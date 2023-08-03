using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapRotate : MonoBehaviour
{

    public float speed = 0.5f;  // 속도
    public float limitY = 10.0f;
    float overy = 0;
    float downy = 0;
    float calY = 0;
    bool right = true;
    float startY = 0;
    public bool isTurn = true;
    void Start()
    {
        startY = transform.rotation.eulerAngles.y;
        overy = startY + limitY;
        downy = startY - limitY;
    }
    void Update()
    {
        startY = transform.rotation.eulerAngles.y;
        if (startY >= overy)
        {
            right = false;
        }
        else if (startY <= downy)
        {
            right = true;
        }
        if (right)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        }
        else
        {
            transform.Rotate(Vector3.down * Time.deltaTime * speed);
        }

    }

}
