using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backward : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left, speed * Time.deltaTime);
    }
}
