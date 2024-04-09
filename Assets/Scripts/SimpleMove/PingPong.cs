using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    [SerializeField] private float hight = 1.2f;
    [SerializeField] private float speed = 2f;
    private float yCenter = 0f;

    [SerializeField] private bool isBackward = false;


    private void Start()
    {
        yCenter = transform.position.y;
    }

    private void Update()
    {
        if(!isBackward)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, yCenter + Mathf.PingPong(Time.time * 2, hight) - hight / 2f, transform.position.z), speed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x-1, yCenter + Mathf.PingPong(Time.time * 2, hight) - hight / 2f, transform.position.z), speed * Time.deltaTime);
    }
}
