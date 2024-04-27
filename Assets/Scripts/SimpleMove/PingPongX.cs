using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongX : MonoBehaviour
{
    [SerializeField] private float range = 1.2f;
    [SerializeField] private float speed = 2f;
    private float xCenter = 0;

    [SerializeField] private bool isAnimFlip = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        xCenter = transform.position.x;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var currentVelocity = xCenter + Mathf.PingPong(Time.time * 2, range) - (range / 2);

        if (isAnimFlip)
        {
            var dir = currentVelocity - transform.position.x;
            if (dir >= 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentVelocity, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }
}
