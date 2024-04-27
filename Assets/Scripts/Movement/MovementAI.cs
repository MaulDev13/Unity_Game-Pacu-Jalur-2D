using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jenis movement AI. Dayung akan bergerak secara konstan.
/// </summary>

public class MovementAI : Movement
{
    [Header("Pingpong Movement")]
    [SerializeField] private float hight = 1.2f;
    [SerializeField] private bool isOnYCenter = true;
    [SerializeField] private float yCenter = 0f;

    [SerializeField] private float handicapTime = 2f;
    private float currentHandicapTime = 0;

    protected override void Init()
    {
        base.Init();

        isPlayer = false;

        if(isOnYCenter)
        {
            yCenter = transform.position.y;
        }


        AnimCheck();
    }

    protected override void Move()
    {
        if(currentHandicapTime < handicapTime)
        {
            currentHandicapTime += Time.deltaTime;
            currentSpeed = Random.Range(movementSpeed, runSpeed / 2);
        } else
        {
            currentSpeed = Random.Range(movementSpeed, runSpeed);
        }
        
        if (isEnd && !isMoveAfterEnd)
            currentSpeed = 0;

        if (boostPowerOn)
            currentSpeed *= boostSpeedMultiplier;
        else
            currentSpeed = Mathf.Clamp(currentSpeed - debuffSpeed, idleSpeed, 99f);

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + 1f, yCenter + Mathf.PingPong(Time.time * 2, hight) - hight / 2f, transform.position.z), currentSpeed * Time.deltaTime); ;

        velocity = new Vector2(0f, currentSpeed);
    }
}