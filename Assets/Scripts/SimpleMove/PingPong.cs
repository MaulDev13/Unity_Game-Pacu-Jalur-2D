using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    [SerializeField] private float range = 1.2f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool isOnYCenter = true;
    [SerializeField] private float yCenter = 0f;

    [SerializeField] private bool isAnimFlip = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (isOnYCenter)
        {
            yCenter = transform.position.y;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var currentVelocity = yCenter + Mathf.PingPong(Time.time * 2, range) - (range / 2);
        
        if(isAnimFlip)
        {
            var dir = currentVelocity - transform.position.y;
            if (dir >= 0)
                spriteRenderer.flipY = false;
            else
                spriteRenderer.flipY = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, currentVelocity, transform.position.z), speed * Time.deltaTime);

    }
}
